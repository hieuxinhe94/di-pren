using System;

namespace Pren.Web.Business.Session
{
    [Serializable]
    public class AuthenticatedCheck
    {
        public string Token { get; set; }
        public string SelectedCampaign { get; set; }
        public string PrePopulateCode { get; set; }
        public long CustomerNumber { get; set; }
        public string TargetGroup { get; set; }
        public string BipMessage { get; set; }
    }
}
