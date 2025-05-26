using Apps.Remote.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Remote.Base;

namespace Tests.Remote
{
    [TestClass]
    public class DataSources : TestBase
    {
        [TestMethod]
        public async Task EmploymentDataHandler_ReturnsValues()
        {
            var handler= new EmploymentDataSource(InvocationContext);
            var result = await handler.GetDataAsync(new DataSourceContext { SearchString = "" }, CancellationToken.None);

            foreach (var item in result)
            {
                Console.WriteLine($"Id: {item.Key}, Name: {item.Value}");
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task EmploymentShortIdDataHandler_ReturnsValues()
        {
            var handler = new EmploymentShortIdDataSource(InvocationContext);
            var result = await handler.GetDataAsync(new DataSourceContext { SearchString = "" }, CancellationToken.None);

            foreach (var item in result)
            {
                Console.WriteLine($"Id: {item.Key}, Name: {item.Value}");
            }
            Assert.IsNotNull(result);
        }

    }
}
