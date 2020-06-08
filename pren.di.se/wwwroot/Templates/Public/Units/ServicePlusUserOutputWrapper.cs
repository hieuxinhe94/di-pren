using System;
using System.Collections.Generic;
using System.Linq;

using DIClassLib.BonnierDigital;

namespace PrenDiSe.Templates.Public.Units
{
    public class ServicePlusUserOutputWrapper : UserOutput
    {
        public string Token { get; set; }
        public string SelectedCampaign { get; set; }

        public void SetUserOutputData(UserOutput userOutput)
        {
            httpResponseCode = userOutput.httpResponseCode;
            requestId = userOutput.requestId;
            user = userOutput.user;
        }
    }
}