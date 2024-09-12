using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.Remote.Models.Responses.Employments;

public class EmploymentResponse
{
    [Display("Employment ID"), JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("country")]
    public CountryResponse Country { get; set; } = new();

    [Display("Full name"), JsonProperty("full_name")]
    public string FullName { get; set; } = string.Empty;

    [Display("Job title"), JsonProperty("job_title")]
    public string JobTitle { get; set; } = string.Empty;

    [Display("Personal email"), JsonProperty("personal_email")]
    public string PersonalEmail { get; set; } = string.Empty;

    [Display("Department ID"), JsonProperty("department_id")]
    public string DepartmentId { get; set; } = string.Empty;

    [Display("Pricing plan details"), JsonProperty("pricing_plan_details")]
    public PricingPlanDetailsResponse PricingPlanDetails { get; set; } = new();

    [JsonProperty("contract_details"), DefinitionIgnore]
    public JObject? ContractDetails { get; set; }

    [Display("Contract details"), JsonProperty("contract_details_response")]
    public ContractDetailsResponse ContractDetailsResponse { get; set; } = new();
    
    public void SetContractDetails()
    {
        ContractDetailsResponse = ContractDetails?.ToObject<ContractDetailsResponse>()!;
    }
}

public class PricingPlanDetailsResponse
{
    [JsonProperty("frequency")]
    public string Frequency { get; set; } = string.Empty;
}

public class ContractDetailsResponse
{
    [Display("Annual gross salary"), JsonProperty("annual_gross_salary")]
    public int AnnualGrossSalary { get; set; }
}
