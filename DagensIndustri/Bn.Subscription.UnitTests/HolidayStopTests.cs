using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bn.Subscription.UnitTests
{
    [TestClass]
    public class HolidayStopTests
    {    
        //[TestMethod]
        //public void HolidayStop_DoAll_ResultSuccess()
        //{
        //    var api = GetSubscriptionApi();

        //    var getHolidayStopsResult = api.HolidayStop.GetHolidayStopsAsync(
        //        "di", 
        //        4022614, 
        //        7051412, 
        //        0);

        //    Assert.IsTrue(getHolidayStopsResult.Result.Result == "success");

        //    var createHolidayResult = api.HolidayStop.CreateHolidayStopAsync(
        //        "di", 
        //        4022614, 
        //        7051412, 
        //        0, 
        //        new DateTime(2016, 11, 02), 
        //        new DateTime(2016, 11, 10));

        //    Assert.IsTrue(createHolidayResult.Result.Result == "success");

        //    var changeHolidayResult = api.HolidayStop.ChangeHolidayStopAsync(
        //        "di",
        //        4022614,
        //        7051412,
        //        0,
        //        new DateTime(2016, 07, 20),
        //        new DateTime(2016, 11, 02),
        //        new DateTime(2016, 07, 25),
        //        new DateTime(2016, 11, 10));

        //    Assert.IsTrue(changeHolidayResult.Result.Result == "success");


        //    var deleteHolidayResult = api.HolidayStop.DeleteHolidayStopAsync(
        //        "di", 
        //        4022614, 
        //        7051412, 
        //        0, 
        //        new DateTime(2016, 07, 20), 
        //        new DateTime(2016, 07, 25));

        //    Assert.IsTrue(deleteHolidayResult.Result.Result == "success");
        //}

        //[TestMethod]
        //public void Test()
        //{

        //    var api = GetSubscriptionApi();

        //    var result = api.Subscription.GetSubscriptionsAsync("di", 4022614, false);

        //    var tmp = result.Result;

        //    Assert.IsTrue(tmp.Result == "success");

        //}

        //private SubscriptionApi GetSubscriptionApi()
        //{
        //    return new SubscriptionApi("http://api.local/");
        //}
    }
}
