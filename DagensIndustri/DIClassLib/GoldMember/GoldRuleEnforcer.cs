using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using DIClassLib.DbHelpers;


namespace DIClassLib.GoldMember
{
    public static class GoldRuleEnforcer
    {

        //todo: find out if student paper subscribers (new paper) should be able to join gold
        public static bool UserPassesGoldRules(long cusno)
        {
            CustomerPropertyHandler cph = new CustomerPropertyHandler(cusno);
            if (!cph.UserPassesGoldRules())
                return false;


            Person p = new Person();
            p.Cusno = cusno;

            foreach (Subscription s in p.SubsHistory)
            {
                if (SubIsFriEx(s))
                    return true;

                if (SubIsExpired(s) || SubIsWeekend(s) || !SubIsActive(s) || !SubLengthIsOk(s))
                    continue;

                return true;
            }

            return false;
        }

        private static bool SubIsFriEx(Subscription s)
        {
            return (s.SubsKind == Settings.SubsKind_friex) ? true : false;
        }

        private static bool SubIsExpired(Subscription s)
        {
            return (s.SubsRealEndDate < DateTime.Now.Date) ? true : false;
        }

        private static bool SubIsWeekend(Subscription s)
        {
            return (s.ProductNo == Settings.ProductNo_Weekend && s.PaperCode == Settings.PaperCode_DI) ? true : false;
        }

        private static bool SubIsActive(Subscription s)
        {
            return Settings.SubsStateActiveValues.Contains(s.SubsState);
        }

        private static bool SubLengthIsOk(Subscription s)
        {
            return (s.SubsLenMons >= 9) ? true : false;
        }

    }
}
