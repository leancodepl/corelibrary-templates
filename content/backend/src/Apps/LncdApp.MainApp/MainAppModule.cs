using Autofac;
using LeanCode.AzureIdentity;
using LeanCode.Components;
using LeanCode.IdentityServer.KeyVault;
using LeanCode.OpenTelemetry;
using LncdApp.DomainName.Services.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace LncdApp.MainApp;

internal class MainAppModule : AppModule
{
    internal const string MainAppCorsPolicy = "MainApp";

    private readonly IConfiguration config;
    private readonly IWebHostEnvironment hostEnv;

    public MainAppModule(IConfiguration config, IWebHostEnvironment hostEnv)
    {
        this.config = config;
        this.hostEnv = hostEnv;
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(ConfigureCORS);
        services.AddRouting();
        services.AddHealthChecks()
            .AddDbContextCheck<DomainNameDbContext>();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        var zipkin = Config.Telemetry.ZipkinEndpoint(config);
        var otlp = Config.Telemetry.OtlpEndpoint(config);

        if (!string.IsNullOrWhiteSpace(zipkin) || !string.IsNullOrWhiteSpace(otlp))
        {
            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation(opts => opts.Filter = ctx =>
                    {
                        return !ctx.Request.Path.StartsWithSegments("/live");
                    })
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation(opts => opts.SetDbStatementForText = true)
                    .AddLeanCodeTelemetry()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService("LncdApp.MainApp"));

                if (!string.IsNullOrWhiteSpace(otlp))
                {
                    builder.AddOtlpExporter(cfg => cfg.Endpoint = new(otlp));
                }

                if (!string.IsNullOrWhiteSpace(zipkin))
                {
                    builder.AddZipkinExporter(cfg => cfg.Endpoint = new(zipkin));
                }
            });
        }

        services.AddAzureClients(cfg =>
        {
            cfg.AddBlobServiceClient(Config.BlobStorage.ConnectionString(config));

            if (!hostEnv.IsDevelopment())
            {
                cfg.AddKeyClient(new(Config.KeyVault.VaultUrl(config)));
                cfg.AddIdentityServerTokenSigningKey(new(Config.KeyVault.KeyId(config)));
                cfg.UseCredential(DefaultLeanCodeCredential.Create(config));
            }
        });
    }

    protected override void Load(ContainerBuilder builder)
    {
        Config.RegisterMappedConfiguration(builder, config, hostEnv);

        builder.RegisterType<AppRoles>()
            .AsImplementedInterfaces();
    }

    private void ConfigureCORS(CorsOptions opts)
    {
        opts.AddPolicy(MainAppCorsPolicy, cfg =>
        {
            cfg
                .WithOrigins(Config.Services.AllowedOrigins(config))
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromMinutes(60));
        });
    }
}
