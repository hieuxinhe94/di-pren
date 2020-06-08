using System.Globalization;
using EPiServer.Framework.Localization;

namespace Pren.Web.Business.Utils.Translate
{
	public class TranslateUtil
	{
	    public static string GetLocalizedText(string key)
	    {
	        return LocalizationService.Current.GetString(key);
	    }

        public static string GetLocalizedTextByCulture(string key, CultureInfo culture)
        {
            return LocalizationService.Current.GetStringByCulture(key, culture);
        }
	}
}