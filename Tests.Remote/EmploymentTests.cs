using Apps.Remote.Actions;
using Apps.Remote.Models.Requests.Employments;
using Remote.Base;

namespace Tests.Remote
{
    [TestClass]
    public class EmploymentTests : TestBase
    {
        [TestMethod]
        public async Task Search_employments_ReturnsResponse()
        {
            var action = new EmploymentActions(InvocationContext);

            var response = await action.SearchEmployments(
                new SearchEmploymentsRequest {  });


            foreach (var item in response.Employments)
            {
                Console.WriteLine($"Id: {item.Id}, Short Id: {item.ShortId}, Name: {item.FullName}");
            }
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public async Task Find_employments_ReturnsResponse()
        {
            var action = new EmploymentActions(InvocationContext);

            var response = await action.FindEmployment(
                new SearchEmploymentsRequest { ShortId= "GMQ1HR" });

            Console.WriteLine($"Id: {response.Id}, Short Id: {response.ShortId}, Name: {response.FullName}");

            Assert.IsNotNull(response);
        }

    }
}
