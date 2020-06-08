using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Di.ServicePlus.Utils
{
    /// <summary>
    /// Utility for generating ServicePlus URL signature
    /// Overall structure of the signature. toHex(HMACSHA256(TIMESTAMP|PARAMETER VALUES SORTED ON KEY In NATURAL ORDER|TIMESTAMP))
    /// </summary>
    public class ServicePlusSignatureUtils
    {
        // URL parameters to ignore when generating the signature
        private static readonly List<string> ExcludeParameters = new List<string>{"s", "ts", "callback", "access_token"};

        public static string Sign(string secretKey, long timestamp, IDictionary<string, string> paramValues)
        {
            string signature = null;
            var str = new StringBuilder();
            str.Append(timestamp);

            var keys = new SortedSet<string>(paramValues.Keys);

            foreach (var key in keys)
            {
                var ignore = ExcludeParameters.Any(parameter => key == parameter);
                if (!ignore)
                {
                    str.Append(paramValues[key]);
                }
            }

            str.Append(timestamp);
            try
            {
                signature = SignWithHMAC(secretKey, str.ToString());
            }
            catch (Exception ex)
            {
                return string.Empty;
                //ServiceLocator.Current.GetInstance<ILogService>().Log("Failed to SignWithHMAC", ex.ToString());
            }

            return signature;
        }

        private static string SignWithHMAC(string key, string data)
        {
            Encoding encoding = new UTF8Encoding();
            var keyBytes = encoding.GetBytes(key);
            var dataBytes = encoding.GetBytes(data.ToCharArray());
            var hmac = new HMACSHA256(keyBytes);

            // hmac.Initialize();
            return ConvertToHex(hmac.ComputeHash(dataBytes));
        }

        private static string ConvertToHex(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();

            foreach (var character in bytes)
            {
                sb.Append(character.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
