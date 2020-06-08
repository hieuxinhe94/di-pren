using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIClassLib.Membership
{
    public static class MembershipSettings
    {
        public static string DiMembershipProviderName { get { return "DiMembershipProvider"; } }
        public static string DiRoleProviderName       { get { return "DiRoleProvider"; } }

        public static string EpiMembershipProviderName { get { return "SqlServerMembershipProvider"; } }
        public static string EpiRoleProviderName       { get { return "SqlServerRoleProvider"; } }
    }
}