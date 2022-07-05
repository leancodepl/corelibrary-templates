using LncdApp.DomainName.Services.CQRS.Example;
using Xunit;

namespace LncdApp.DomainName.Services.Tests.CQRS.Example
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
