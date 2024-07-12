using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.Remote;

public class Application : IApplication, ICategoryProvider
{
    public string Name
    {
        get => "Remote";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ApplicationCategory> Categories
    {
        get => new []{ ApplicationCategory.TranslationBusinessManagement, ApplicationCategory.Fintech };
        set { }
    }
}