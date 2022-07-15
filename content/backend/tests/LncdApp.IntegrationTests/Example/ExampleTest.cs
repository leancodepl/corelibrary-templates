using LncdApp.DomainName.Contracts.Example;
using Xunit;

namespace LncdApp.IntegrationTests.Example
{
    public class ExampleTest : TestsBase<UnauthenticatedLncdAppTestApp>
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
