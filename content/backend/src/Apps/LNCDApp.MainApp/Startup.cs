using LeanCode.Cache.AspNet;
using LeanCode.Components;
using LeanCode.Components.Startup;
using LeanCode.CQRS.Cache;
using LeanCode.CQRS.Default;
using LeanCode.CQRS.RemoteHttp.Server;
using LeanCode.CQRS.Security;
using LeanCode.CQRS.Validation;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.MassTransitRelay;
using LeanCode.DomainModels.MassTransitRelay.Middleware;
using LeanCode.IdentityServer.KeyVault;
using LeanCode.Localization;
using LeanCode.OpenTelemetry;
using LeanCode.ViewRenderer.Razor;
using LNCDApp.DomainName.Contracts.Example;
using LNCDApp.DomainName.Domain.Example.Events;
using LNCDApp.DomainName.Services;
using LNCDApp.MainApp.Auth;
using LNCDApp.MainApp.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LNCDApp.MainApp
{
    public class Startup : LeanStartup
    {
        private static readonly RazorViewRendererOptions ViewOptions = new("Templates");

        private static readonly TypesCatalog AllHandlers = new(typeof(DomainNameContext));
        private static readonly TypesCatalog Api = new(typeof(ExampleCommand));
        private static readonly TypesCatalog Domain = new(typeof(ExampleEvent));

        private readonly IWebHostEnvironment hostEnv;

        protected override IAppModule[] Modules { get; }

        public Startup(IWebHostEnvironment hostEnv, IConfiguration config)
            : base(config)
        {
            this.hostEnv = hostEnv;
            Modules = ConfigureModules(hostEnv, config);
        }

        protected static IAppModule[] ConfigureModules(IWebHostEnvironment hostEnv, IConfiguration config)
        {
            var dbConnStr = Config.SqlServer.ConnectionString(config);

            var modules = new List<IAppModule>
            {
                new MainAppModule(config, hostEnv),
                new DomainNameModule(dbConnStr),
                new AuthModule(hostEnv, config),

                new OpenTelemetryModule(),
                new CQRSModule()
                    .WithCustomPipelines<DomainNameContext>(
                        AllHandlers,
                        c => c.Trace().Secure().Validate().StoreAndPublishEvents(),
                        q => q.Trace().Secure().Cache()),
                new FluentValidationModule(AllHandlers),
                new InMemoryCacheModule(),
                new MassTransitRelayModule(AllHandlers, Domain, MassTransitConfiguration.ConfigureBus(config, hostEnv)),
                new LocalizationModule(LocalizationConfiguration.For<Strings.Strings>()),
            };

            if (!hostEnv.IsDevelopment())
            {
                modules.Add(new IdentityServerKeyVaultModule());
            }

            return modules.ToArray();
        }

        protected override void ConfigureApp(IApplicationBuilder app)
        {
            app
                .UseRouting()
                .UseForwardedHeaders()
                .UseCors(MainAppModule.MainAppCorsPolicy);

            app.Map("/auth", auth => auth.UseIdentityServer());

            app.Map("/api", api =>
                    api
                        .UseAuthentication()
                        .UseRemoteCQRS(
                            Api,
                            DomainNameContext.FromHttp));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", VersionHandler.HandleAsync);
                endpoints.MapHealthChecks("/live/health");
                endpoints.MapGet("/live/ready", ReadinessProbe.HandleAsync);
            });
        }
    }
}
