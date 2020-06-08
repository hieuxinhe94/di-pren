using System;
using System.Text.RegularExpressions;
using System.Web;
using Di.Common.Utils.Context;
using Di.Common.WebRequests;
using EPiServer.Editor;

namespace Pren.Web.Business.Detection
{
    public class DetectionHandler : IDetectionHandler
    {
        public bool IsHttpMethod(RequestMethod requestMethod, HttpContext context = null)
        {
            return HttpContextUtils.GetHttpMethod(context) == requestMethod;
        }

        public bool IsMobileDevice(HttpContext context = null)
        {
            return HttpContextUtils.IsMobileDevice(context);
        }

        public bool IsNumeric(string valueToCheck)
        {
            Int64 i;
            return Int64.TryParse(valueToCheck, out i);
        }

        public bool IsValidSwePhoneNum(string num)
        {
            return !string.IsNullOrEmpty(num) && num.Length >= 7 && num.Length <= 20 && num.StartsWith("+46");
        }

        public bool IsInEditMode()
        {
            return PageEditing.PageIsInEditMode;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.ToLower();

            //not valid chars
            if (Regex.IsMatch(email, @"[^a-z0-9_\-\.\@]"))
                return false;

            //valid before @
            if (!Regex.IsMatch(email, @"^([_a-z0-9-])+(\.[_a-z0-9-]+)*@"))
                return false;

            //one @
            if (Regex.Matches(email, @"\@").Count != 1)
                return false;

            //valid after @
            if (!Regex.IsMatch(email, @"\@([a-z0-9]+[\-\.]?)+"))
                return false;

            //find (..) (.-) (-.) (--) after @
            if (Regex.IsMatch(email, @"\@(.)*(\.\.|\.\-|\-\.|\-\-){1}"))
                return false;

            //valid ending (x.xx) (x.xxx) (x.xxxx)
            return Regex.IsMatch(email, @"([a-z0-9]{1,}\.([a-z]{2,4}))$");
        }
    }
}