using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.DataSourceHandlers.Static;

public class JobCategoryDataSource : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return new List<DataSourceItem>
        {
            new DataSourceItem("operations", "Operations"),
            new DataSourceItem("finance", "Finance"),
            new DataSourceItem("legal", "Legal/Paralegal"),
            new DataSourceItem("engineering_it", "Engineering/IT"),
            new DataSourceItem("growth_marketing", "Growth & Marketing"),
            new DataSourceItem("sales", "Sales"),
            new DataSourceItem("customer_experience_support", "Customer Experience/Support"),
            new DataSourceItem("people_mobility", "People and mobility"),
            new DataSourceItem("techops_supply_chain", "Techops/Supply chain"),
            new DataSourceItem("business_process_improvement_product_management", "Business Process Improvement/Product Management"),
            new DataSourceItem("research_development", "Research & Development"),
            new DataSourceItem("onboarding_payroll", "Onboarding & Payroll"),
        };
    }
}
