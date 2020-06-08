using Di.Subscription.Logic.Package.Retrievers;

namespace Di.Subscription.Logic.Package
{
    public interface IPackageHandler
    {
        IPackageRetriever PackageRetriever { get; }
    }
}
