using System;
using System.Net;
using System.Net.Http;
using Di.Subscription.Logic.Campaign;
using Di.Subscription.Logic.IssueDate;
using Pren.Web.Business.Controllers;

namespace Pren.Web.Controllers.ApiControllers
{
    public class StartDateController : ExtendedApiController
    {
        private readonly IIssueDateHandler _issueDateHandler;
        private readonly ICampaignHandler _campaignHandler;
        

        public StartDateController(IApiReferrerCheck apiReferrerCheck, IIssueDateHandler issueDateHandler, ICampaignHandler campaignHandler)
            : base(apiReferrerCheck)
        {
            _issueDateHandler = issueDateHandler;
            _campaignHandler = campaignHandler;
        }

        public HttpResponseMessage Get(string campId)
        {
            if (!VerifyDomain())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var campaign = _campaignHandler.GetCampaign(campId);
            var issueDate =
                _issueDateHandler.Retriever.GetNextIssuedate(campaign.PaperCode,
                    campaign.ProductNumber, DateTime.Now).ToString("yyyy-MM-dd");

            return Request.CreateResponse(HttpStatusCode.OK, issueDate);            
        }
    }


}