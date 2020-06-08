namespace Pren.Web.Business.Configuration
{
    public interface ISiteConfiguration
    {
        string GetSetting(string key, string defaultValue = "");
    }
}
