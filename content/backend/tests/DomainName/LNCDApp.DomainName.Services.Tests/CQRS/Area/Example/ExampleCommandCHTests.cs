using LNCDApp.DomainName.Services.CQRS.Example;
using Xunit;

namespace LNCDApp.DomainName.Services.Tests.CQRS.Example
{
    public class ExampleCommandCHTests
    {
        private readonly ExampleCommandCH handler;

        public ExampleCommandCHTests()
        {
            handler = new ExampleCommandCH();
        }

        [Fact]
        public void ImplementMe()
        {
            Assert.True(true);
        }
    }
}
