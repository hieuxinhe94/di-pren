namespace Pren.Web.Business.Package
{
    public interface IPackageRelationManager
    {
        string GetParameters(PackageRelationTypeEnum packageRelationType, string packageId);
    }
}
