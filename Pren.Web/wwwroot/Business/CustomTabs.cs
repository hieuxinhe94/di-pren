using EPiServer.DataAbstraction;
using EPiServer.Security;

namespace Pren.Web.Business
{
    public static class CustomTabs
    {
        public const string Content = "Information"; //DEFAULT TAB
        public static TabDefinition ContentTab = new TabDefinition
        {
            Name = Content,
            DisplayName = "Content",
            RequiredAccess = AccessLevel.Edit,
        };

        public const string Advanced = "Advanced"; 
        public static TabDefinition AdvancedTab = new TabDefinition
        {
            Name = Advanced,
            DisplayName = "Settings",
            RequiredAccess = AccessLevel.Edit,
        };

        public const string ThankYou = "Tacksida"; 
        public static TabDefinition ThankYouTab = new TabDefinition
        {
            Name = ThankYou,
            DisplayName = "Tacksida",
            RequiredAccess = AccessLevel.Edit,
        };

        public const string Campaign = "Kampanj"; 
        public static TabDefinition CampaignTab = new TabDefinition
        {
            Name = Campaign,
            DisplayName = "Kampanj",
            RequiredAccess = AccessLevel.Edit,
        };

        public const string Fallback = "Fallback";

        public static TabDefinition FallbackTab = new TabDefinition
        {
            Name = Fallback,
            DisplayName = "Fallback",
            RequiredAccess = AccessLevel.Edit,
        };

        public const string Script = "Script";
        public static TabDefinition ScriptTab = new TabDefinition
        {
            Name = Script,
            DisplayName = "Script",
            RequiredAccess = AccessLevel.Edit,
        };
    }
}
