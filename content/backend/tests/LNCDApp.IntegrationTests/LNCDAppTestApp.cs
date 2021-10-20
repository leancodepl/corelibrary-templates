using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using IdentityModel.Client;
using LeanCode.Components;
using LeanCode.Components.Startup;
using LeanCode.CQRS.RemoteHttp.Client;
using LeanCode.IntegrationTestHelpers;
using LNCDApp.DomainName.Contracts;
using LNCDApp.DomainName.Services.DataAccess;
using LNCDApp.DomainName.Services.DataAccess.Entities;
using LNCDApp.IntegrationTests.Overrides;
using LNCDApp.MainApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Events;

namespace LNCDApp.IntegrationTests
{
    public class LNCDAppTestApp : LeanCodeTestFactory<Startup>
    {
        protected override ConfigurationOverrides Configuration { get; }
            = new ConfigurationOverrides(LogEventLevel.Debug, true);

        static LNCDAppTestApp()
        {
            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("WAIT_FOR_DEBUGGER")))
            {
                Console.WriteLine("Waiting for debugger to be attached...");

                while (!Debugger.IsAttached)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        public const string UserEmail = "test@leancode.pl";
        public const string UserPassword = "long_test_password123!";

        protected override IEnumerable<Assembly> GetTestAssemblies()
        {
            yield return typeof(LNCDAppTestApp).Assembly;
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return LeanProgram
                .BuildMinimalHost<TestStartup>()
                .ConfigureDefaultLogging(
                    projectName: "LNCDApp-tests",
                    destructurers: new TypesCatalog(typeof(Program)))
                .UseEnvironment(Environments.Development);
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            using (var scope = Services.CreateScope())
            {
                await CreateTestUserAsync(scope.ServiceProvider);
            }
        }

        public Task<bool> AuthenticateAsync()
        {
            return AuthenticateAsync(new PasswordTokenRequest
            {
                UserName = UserEmail,
                Password = UserPassword,
                Scope = $"profile openid {Auth.Scopes.InternalMainApp}",

                ClientId = Auth.Clients.ClientApp,
                ClientSecret = string.Empty,
            });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                services.AddTransient<DbContext>(sp => sp.GetService<DomainNameDbContext>());
            });
        }

        private async Task CreateTestUserAsync(IServiceProvider services)
        {
            var userId = Guid.NewGuid();

            // var user = User.Create();
            // var dbContext = services.GetService<DomainNameDbContext>();
            // dbContext.Users.Add(user);
            // await dbContext.SaveChangesAsync();

            var userManager = services.GetService<UserManager<AuthUser>>();
            var authUser = new AuthUser
            {
                Id = userId,
                Email = UserEmail,
                UserName = UserEmail,
                Claims =
                {
                    new IdentityUserClaim<Guid>() { ClaimType = Auth.KnownClaims.Role, ClaimValue = Auth.Roles.User },
                },
            };
            await userManager.CreateAsync(authUser, UserPassword);
        }
    }

    public class AuthenticatedLNCDAppTestApp : LNCDAppTestApp
    {
        public HttpQueriesExecutor Query { get; private set; } = default!;
        public HttpCommandsExecutor Command { get; private set; } = default!;

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            if (!await AuthenticateAsync())
            {
                throw new Xunit.Sdk.XunitException("Authentication failed.");
            }

            Query = CreateQueriesExecutor();
            Command = CreateCommandsExecutor();
        }

        public override async ValueTask DisposeAsync()
        {
            Command = default!;
            Query = default!;
            await base.DisposeAsync();
        }
    }

    public class UnauthenticatedLNCDAppTestApp : LNCDAppTestApp
    {
        public HttpQueriesExecutor Query { get; private set; } = default!;
        public HttpCommandsExecutor Command { get; private set; } = default!;

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Query = CreateQueriesExecutor();
            Command = CreateCommandsExecutor();
        }

        public override async ValueTask DisposeAsync()
        {
            Command = default!;
            Query = default!;
            await base.DisposeAsync();
        }
    }
}
