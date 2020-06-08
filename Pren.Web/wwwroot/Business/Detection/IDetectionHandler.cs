using System.Web;
using Di.Common.WebRequests;

namespace Pren.Web.Business.Detection
{
    public interface IDetectionHandler
    {
        bool IsHttpMethod(RequestMethod requestMethod, HttpContext context = null);

        bool IsMobileDevice(HttpContext context = null);

        bool IsNumeric(string valueToCheck);

        bool IsValidEmail(string email);

        bool IsValidSwePhoneNum(string num);

        bool IsInEditMode();
    }
}