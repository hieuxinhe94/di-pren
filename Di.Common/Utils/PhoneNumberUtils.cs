using System;

namespace Di.Common.Utils
{
    public class PhoneNumberUtils
    {
        public static bool IsSamePhoneNumberExceptCountryPrefix(string phoneNumber1, string phoneNumber2, string countryPrefix)
        {
            if (string.IsNullOrEmpty(phoneNumber1) && string.IsNullOrEmpty(phoneNumber2))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(phoneNumber1) && string.IsNullOrEmpty(phoneNumber2))
            {
                return false;
            }
            if (string.IsNullOrEmpty(phoneNumber1) && !string.IsNullOrEmpty(phoneNumber2))
            {
                return false;
            }

            phoneNumber1 = phoneNumber1.TrimStart('0').Replace(countryPrefix, string.Empty);
            phoneNumber2 = phoneNumber2.TrimStart('0').Replace(countryPrefix, string.Empty);

            return phoneNumber1 == phoneNumber2;
        }

        /// <summary>
        ///  Format phone number to match Kayak requirements. 
        ///  It has to start with a country code, have a max no of digits and only contain digits and a '+'.
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="maxNoOfDigits"></param>
        /// <param name="doMobileCheck"></param>
        /// <returns></returns>
        public static string FormatPhoneNumber(string phoneNo, int maxNoOfDigits, bool doMobileCheck)
        {
            var num = string.Empty;
            if (!string.IsNullOrEmpty(phoneNo))
            {
                num = phoneNo.Trim();
                num = num.Replace("-", "");
                num = num.Replace("/", "");
                num = num.Replace(@"\", "");
                num = num.Replace(" ", "");

                if (!num.StartsWith("+"))
                {
                    if (num.StartsWith("00"))
                    {
                        num = string.Format("+{0}", num.Substring(2));
                    }
                    else if (num.StartsWith("0"))
                    {
                        num = num.Substring(1);
                        num = string.Format("+46{0}", num);
                    }
                }

                if (doMobileCheck && num.StartsWith("+46"))
                {
                    string phoneNoCountryCode = num.Substring(3);

                    //Check if mobile number without country code, starts with 1 or 7 (start digits of a mobile number)
                    if (!phoneNoCountryCode.StartsWith("1") && !phoneNoCountryCode.StartsWith("7"))
                        return null;
                }

                //Check if phone number only has digits (excluding + in country code) and it doesn't exceed max no of digits
                Int64 parsedPhoneNo;
                if (!Int64.TryParse(num.Substring(1), out parsedPhoneNo) || num.Length > maxNoOfDigits)
                    num = null;
            }

            return num;
        }
    }
}
