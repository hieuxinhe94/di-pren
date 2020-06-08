using System.Globalization;

namespace Di.Common.Utils
{
    public static class DecimalUtils
    {
        /// <summary>
        /// Takes a string and tries to convert to decimal or null if no parse can be made
        /// </summary>
        /// <param name="value">A string containing the decimal to convert</param>
        /// <returns>Converted decimal? or null</returns>
        public static decimal? TryParseToNullableDecimal(string value)
        {
            decimal parsedDecimal;

            if (!decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out parsedDecimal))
            {
                return null;
            }

            return parsedDecimal;
        }
    }
}
