using System;

namespace Di.Common.Utils
{
    public class DateTimeUtils
    {
        public static long GetUnixTimeStampInMilliseconds(DateTime date)
        {
            var dateTime = new DateTime(date.Year,date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Local);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return  (long)(dateTime.ToUniversalTime() - epoch).TotalSeconds * 1000;
        }

        public static DateTime? UnixTimeStampToDateTime(string unixTimeStampMillis)
        {
            double unixTimeStampMillisDouble;
            if (string.IsNullOrEmpty(unixTimeStampMillis) || !double.TryParse(unixTimeStampMillis, out unixTimeStampMillisDouble))
            {
                return null;
            }

            return UnixTimeStampToDateTime(unixTimeStampMillisDouble);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStampMillis)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(Math.Round(unixTimeStampMillis)).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Takes a string and tries to convert to DateTime or null if no parse can be made
        /// </summary>
        /// <param name="dateTime">A string containing the date and time to convert</param>
        /// <returns>Converted DateTime? or null</returns>
        public static DateTime? TryParseToNullableDateTime(string dateTime)
        {
            DateTime parsedDateTime;

            if (!DateTime.TryParse(dateTime, out parsedDateTime))
            {
                return null;
            }

            return parsedDateTime;
        }
    }
}
