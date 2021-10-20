using Autofac;
using LeanCode.Components;
using LeanCode.IntegrationTestHelpers;
using LNCDApp.DomainName.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LNCDApp.IntegrationTests.Overrides
{
    public sealed class TestOverridesPreModule : AppModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<DbContextInitializer<DomainNameDbContext>>();
        }
    }

    public sealed class TestOverridesPostModule : AppModule
    {
        public TestOverridesPostModule()
        { }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => c.Resolve<DomainNameDbContext>()).As<DbContext>();
        }
    }
}
