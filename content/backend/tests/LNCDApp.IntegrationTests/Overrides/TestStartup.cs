using LeanCode.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LNCDApp.IntegrationTests.Overrides
{
    public class TestStartup : MainApp.Startup
    {
        public TestStartup(IWebHostEnvironment hostEnv, IConfiguration config)
            : base(hostEnv, config)
        {
            Modules = base.Modules
                .Prepend(new TestOverridesPreModule())
                .Append(new TestOverridesPostModule())
                .ToArray();
        }

        protected override IAppModule[] Modules { get; }
    }
}
