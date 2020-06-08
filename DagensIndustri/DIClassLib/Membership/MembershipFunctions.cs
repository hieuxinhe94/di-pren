using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DIClassLib.Membership
{
    public static class MembershipFunctions
    {

        public static bool UserAllowedToSeePage(List<string> rolesAllowed)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return false;

            foreach(string role in rolesAllowed)
            {
                if (!HttpContext.Current.User.IsInRole(role))
                    return false;
            }

            return true;
        }


        public static bool UserAllowedToSeePage(string roleAllowed)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return false;
            
            return HttpContext.Current.User.IsInRole(roleAllowed);
        }


        public static bool UserIsLoggedInWithProvider(string providerName)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                MembershipUser usr = System.Web.Security.Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
                return (usr.ProviderName.ToString() == providerName);
            }
            return false;
        }
    }
}