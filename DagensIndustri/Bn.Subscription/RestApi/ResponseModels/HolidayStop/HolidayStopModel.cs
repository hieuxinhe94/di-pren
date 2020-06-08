using System;
using Newtonsoft.Json;

namespace Bn.Subscription.RestApi.ResponseModels.HolidayStop
{
    public class HolidayStopModel
    {
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("stopDate")]
        public DateTime StopDate { get; set; }
    }
}
