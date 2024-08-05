using Apps.Remote.Models.Responses;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Requests.Users;

public class CompanyManagersPaginationResponse : PaginationResponse<UserResponse>
{
    [JsonProperty("company_managers")]
    public override IEnumerable<UserResponse>? Items { get; set; }
}