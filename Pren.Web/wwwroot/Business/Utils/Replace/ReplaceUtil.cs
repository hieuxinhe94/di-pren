using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pren.Web.Business.Utils.Replace
{

    public class ReplaceUtil
    {            
        public static string ReplacePlaceholderWithImage(string text)
        {
            return text
                .Replace("[DIWEEKEND]", "<img class=\"usp-logo\" src=\"/pren/Static/css/img/usplogos/di-weekend.png\" />")
                .Replace("[DIDIMENSION]", "<img class=\"usp-logo\" src=\"/pren/Static/css/img/usplogos/di-dimension.png\" />")
                .Replace("[DIPLUS]", "<img class=\"usp-logo\" src=\"/pren/Static/css/img/usplogos/di-plus.png\" />")
                .Replace("[DIETIDNING]", "<img class=\"usp-logo\" src=\"/pren/Static/css/img/usplogos/di-e-tidning.png\" />")
                .Replace("[DN]", "<img class=\"usp-logo\" src=\"/pren/Static/css/img/usplogos/dn.png\" />")
                .Replace("[DNDIGITALT]", "<img class=\"usp-logo\" src=\"/pren/Static/css/img/usplogos/dn-digitalt.png\" />")
                .Replace("[DNPRIO]", "<img class=\"usp-logo\" src=\"/pren/Static/css/img/usplogos/dn-prio.png\" />");
        }

        /// <summary>
        /// Replaces all placeholders found i dictionary.
        /// </summary>
        /// <param name="text">The text with placeholders to replace.</param>
        /// <param name="placeholderDictionary">The dictionary with value pairs to replace {"[replaceme]", "withthis"}.</param>
        /// <returns>Text with placeholder replaced.</returns>
        public static string ReplacePlaceHolders(string text, Dictionary<string, string> placeholderDictionary)
        {
            return placeholderDictionary.Aggregate(text, (current, placeholder) => Regex.Replace(current, "\\" + placeholder.Key, placeholder.Value, RegexOptions.IgnoreCase));
        }
    }

}
