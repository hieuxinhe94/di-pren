using System.Collections.Generic;
using Di.Common.Conversion.Types;

namespace Di.Common.Utils
{
    public static class JsonUtils
    {
        /// <summary>
        /// Converts JSON to mapped object
        /// </summary>
        /// <typeparam name="T">The mapped object</typeparam>
        /// <param name="jsonString">The JSON string</param>
        /// <returns>Mapped object build up from provided JSON string</returns>
        public static T ConvertToObject<T>(this string jsonString)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// Converts object to JSON
        /// </summary>
        /// <typeparam name="T">The type of the object to Convert</typeparam>
        /// <param name="objectToConvert">The object to convert</param>
        /// <returns>A JSON string of version of the provided object</returns>
        public static string ConvertToJson<T>(this T objectToConvert) 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(objectToConvert);
        }
    }
}
