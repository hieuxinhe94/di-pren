using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Di.Common.Utils.ByteArray
{
    public class ByteArrayUtils
    {
        public static string ConvertToHex(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();

            foreach (var character in bytes)
            {
                sb.Append(character.ToString("x2"));
            }

            return sb.ToString();
        }

        public static byte[] ConvertToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
