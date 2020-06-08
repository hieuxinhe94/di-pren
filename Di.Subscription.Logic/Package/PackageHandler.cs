using Di.Subscription.Logic.Package.Retrievers;

namespace Di.Subscription.Logic.Package
{
    public class PackageHandler : IPackageHandler
    {
        public IPackageRetriever PackageRetriever { get; private set; }

        public PackageHandler(IPackageRetriever packageRetriever)
        {
            PackageRetriever = packageRetriever;
        }
    }
}