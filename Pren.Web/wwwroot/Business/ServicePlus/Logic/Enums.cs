namespace Pren.Web.Business.ServicePlus.Logic
{
    // ReSharper disable InconsistentNaming
    internal enum UserType
    {        
        CUSTOMER,
        API,
        BACKOFFICE        
    }

    public enum EntitlementState
    {
        Valid,
        Invalid,
        Syncing,
        Undefined
    }

    internal enum ExternalUserIdSystem
    {
        KAYAKBiz
    } 
    // ReSharper restore InconsistentNaming
}
