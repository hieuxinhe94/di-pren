using System.Security.Principal;
using System.Web;
using EPiServer.Personalization.VisitorGroups;

namespace Pren.Web.Business.Personalization.Criteria.UserAgent
{
    [VisitorGroupCriterion(
        Category = "User Criteria",
        DisplayName = "UserAgent",
        Description = "Criterion that matches user agent of the user's browser")]
    public class UserAgentCriterion : CriterionBase<UserAgentSettings>
    {
        public override bool IsMatch(IPrincipal principal, HttpContextBase httpContext)
        {
            var userAgent = httpContext.Request.UserAgent;

            return MatchUserAgent(userAgent);
        }

        protected virtual bool MatchUserAgent(string userAgent)
        {
            switch (Model.Contains)
            {
                case CustomCompare.Equal:
                    return userAgent.Contains(Model.UserAgentValue);
                case CustomCompare.NotEqual:
                    return !userAgent.Contains(Model.UserAgentValue);
                default:
                    return false;
            }
        }
    }
}