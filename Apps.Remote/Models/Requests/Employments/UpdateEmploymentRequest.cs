using Apps.Remote.DataSourceHandlers;
using Apps.Remote.DataSourceHandlers.Static;
using Apps.Remote.Models.Identifiers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Requests.Employments;

public class UpdateEmploymentRequest : CountryIdentifier
{
    [Display("Employment ID"), DataSource(typeof(EmploymentDataSource))]
    public string EmploymentId { get; set; } = string.Empty;
    
    [Display("Full name")] 
    public string? FullName { get; set; }

    [Display("Job title")]
    public string? JobTitle { get; set; }
    
    [Display("Personal email")]
    public string? PersonalEmail { get; set; }

    [Display("Pricing plan frequency"), StaticDataSource(typeof(PricingPlanFrequencyDataSource))]
    public string? PricingPlanFrequency { get; set; }
    
    [Display("Contract details keys"), DataSource(typeof(ContractDetailsKeysDataSource))]
    public IEnumerable<string>? ContractDetailsKeys { get; set; }
    
    [Display("Contract details values")]
    public IEnumerable<string>? ContractDetailsValues { get; set; }

    public bool ShouldChangeContractDetails()
    {
        return ContractDetailsKeys != null && ContractDetailsKeys.Any();
    }
    
    public Dictionary<string, string> MergeContractDetails()
    {
        var contractDetails = new Dictionary<string, string>();
        
        if(ContractDetailsKeys?.Count() != ContractDetailsValues?.Count())
        {
            throw new ArgumentException("Contract details keys and values must have the same length");
        }
        
        if (ContractDetailsKeys != null && ContractDetailsValues != null)
        {
            for (var i = 0; i < ContractDetailsKeys.Count(); i++)
            {
                contractDetails.Add(ContractDetailsKeys.ElementAt(i), ContractDetailsValues.ElementAt(i));
            }
        }
        else
        {
            throw new ArgumentException("Contract details keys and values must not be null");
        }

        return contractDetails;
    }
}