namespace Di.ServicePlus.Utils
{
    internal static class ServicePlusSpecificJsonUtils
    {
        /// <summary>
        /// Converts S+ JSON to mapped object
        /// </summary>
        /// <typeparam name="T">The mapped object</typeparam>
        /// <param name="jsonString">The JSON string</param>
        /// <returns>Mapped object build up from provided JSON string</returns>
        internal static T ConvertServicePlusJsonToObject<T>(this string jsonString)
        {
            return Common.Utils.JsonUtils.ConvertToObject<T>(jsonString.CleanUpServicePlusJson());
        }

        /// <summary>
        /// Cleans up bad data from S+ JSON response
        /// </summary>
        /// <param name="jsonString">the JSON string returned by S+</param>
        /// <returns>Cleaned up JSON string</returns>
        private static string CleanUpServicePlusJson(this string jsonString)
        {
            return jsonString
                .Replace("@httpResponseCode", "httpResponseCode")
                .Replace("@requestId", "requestId")
                .Replace("@errorCode", "errorCode");
        }
    }
}
