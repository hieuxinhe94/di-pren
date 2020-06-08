using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.PlugIn;
using System.Web.Security;
using DIClassLib.Membership;
using DIClassLib.DbHelpers;

namespace DagensIndustri.Tools.Jobs.SMS
{
    [ScheduledPlugIn(DisplayName = "Remove SMS users", Description = "Remove SMS users older than 24 hours")]
    public class RemoveSMSUsers
    {
        public static string Execute()
        {
            return RemoveSmsUserFromEpiProvider();
        }

        private static string RemoveSmsUserFromEpiProvider()
        {
            string status = "NOT OK";

            //use epi membership provider and epi role provider
            MembershipProvider memProv = Membership.Providers[MembershipSettings.EpiMembershipProviderName];
            RoleProvider roleProv = Roles.Providers[MembershipSettings.EpiRoleProviderName];

            string[] users = roleProv.GetUsersInRole(DiRoleHandler.RoleDiSms24Hour);

            if (users.Length > 0)
            {
                try
                {
                    foreach (string userName in users)
                    {
                        MembershipUser user = memProv.GetUser(userName, false);

                        if (user == null || user.CreationDate.AddHours(24) < DateTime.Now)
                        {
                            if (!memProv.DeleteUser(userName, true))
                                new Logger("RemoveSmsUserFromMssqlProvider() - failed for user: " + userName, "Not an exception");
                        }
                    }

                    status = "All SMS users older than 24 hours removed";
                }
                catch (Exception ex)
                {
                    new Logger("RemoveSmsUserFromMssqlProvider() - failed", ex.ToString());
                    status = "Remove SMS users failed";
                }
            }
            else
            {
                status = "There are no SMSGroup users";
            }

            return status;
        }

        //private static string RemoveSmsUserFromDefaultProvider()
        //{
        //    string status = "NOT OK";

        //    string[] users = Roles.GetUsersInRole(DiRoleHandler.RoleDiSms24Hour);

        //    if (users.Length > 0)
        //    {
        //        try
        //        {
        //            foreach (string user in users)
        //            {
        //                MembershipUser memUser = Membership.GetUser(user);

        //                if (memUser.CreationDate.AddHours(24) < DateTime.Now)
        //                {
        //                    Membership.DeleteUser(memUser.UserName);
        //                }
        //            }

        //            status = "All SMS users older than 24 hours removed";
        //        }
        //        catch (Exception ex)
        //        {
        //            new DIClassLib.DbHelpers.Logger("RemoveSMSUsersFailed() - failed", ex.ToString());
        //            status = "Remove SMS users failed";
        //        }
        //    }
        //    else
        //    {
        //        status = "There is no SMSGroup users";
        //    }

        //    return status;
        //}
    }
}
       