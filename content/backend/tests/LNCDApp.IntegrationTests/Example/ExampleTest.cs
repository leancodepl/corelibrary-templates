using LNCDApp.DomainName.Contracts.Example;
using Xunit;

namespace LNCDApp.IntegrationTests.Example
{
    public class ExampleTest : TestsBase<UnauthenticatedLNCDAppTestApp>
    {
        [Fact]
        public async Task Example_test()
        {
            var result = await App.Command.RunAsync(new ExampleCommand
            {
                Arg = "foo",
            });

            Assert.True(result.WasSuccessful);
        }
    }
}
