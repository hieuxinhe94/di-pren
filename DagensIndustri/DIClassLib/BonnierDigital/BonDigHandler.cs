using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using System.Configuration;
using System.Data;
using DIClassLib.DbHandlers;
using System.Web;


namespace DIClassLib.BonnierDigital
{
    public static class BonDigHandler
    {
        /// <summary>
        /// Return: 
        /// 1=new passwd saved to S+, 
        /// 2=existing passwd not changed in S+, 
        /// -1=S+ productId not found, 
        /// -2=S+ username does not exist, 
        /// -3=S+ username not available, 
        /// -4=save user to S+ failed, 
        /// -5=S+ userId missing, 
        /// -6=failed to create S+ import
        /// </summary>
        public static int TryAddCustAndSubToBonDig(string paperCode, string productNo, long cusno, long subsno, string email, string firstName, 
                                                   string lastName, string phoneNumber, string passwd, bool addSubToExistingDiAccount, bool isHybrid = false)
        {
            var productId = Settings.GetBonDigProductId(paperCode, productNo);
            var hybridExtraIpadProductId = Settings.GetBonDigProductId(Settings.PaperCode_IPAD, "01");
            if (string.IsNullOrEmpty(productId))
            {
                new Logger("TryAddCustAndSubToBonDig() / GetBonDigProductId() failed: papercode:" + paperCode + ", productno:" + productNo, "Not an exception.");
                return -1;
            }

            string userId = null;

            //try find user by email (is unique in Bon Dig system)
            bool userExists = UserExistInServicePlus(email, out userId);

            //if (!userExists && !addSubToExistingDiAccount)  new + new
            //if (userExists && addSubToExistingDiAccount)    old + add
            
            if (!userExists && addSubToExistingDiAccount)
                return -2;

            if (userExists && !addSubToExistingDiAccount)
                return -3;

            if (string.IsNullOrEmpty(userId))
            {
                UserOutput usr = CreateUser(cusno, email, firstName, lastName, phoneNumber, passwd);
                if (usr == null)
                    return -4;

                userId = usr.user.id;
            }


            if (string.IsNullOrEmpty(userId))
            {
                new Logger("TryAddCustAndSubToBonDig() - userId is empty. Very strange. Try to debug.", "Not an exception.");
                return -5;
            }

            var imp = CreateImport(productId, userId, cusno, subsno);
            if (imp == null)
            {
                return -6;
            }
            if (isHybrid)
            {
                if (string.IsNullOrEmpty(hybridExtraIpadProductId))
                {
                    new Logger("TryAddCustAndSubToBonDig() hybridExtraIpadProductId failed: ", "Could not add extra IPAD product on hybrid");
                    return -1;
                }
                var impHybrid = CreateImport(productId, userId, cusno, subsno);
                if (impHybrid == null)
                {
                    return -6;
                }
            }
            return userExists ? 2 : 1;
        }


        public static UserOutput CreateUser(long cusno, string email, string firstName, string lastName, string phoneNumber, string passwd)
        {
            try
            {
                User usr = new User();
                UserOutput uo = usr.CreateUser(email, firstName, lastName, phoneNumber, passwd);
                return uo;
            }
            catch (Exception ex)
            {
                new Logger("CreateUser() - failed for cusno: " + cusno.ToString(), ex.ToString());
            }

            return null;
        }


        public static ImportOutput CreateImport(string productId, string userId, long cusno, long subsno)
        {
            try
            {
                Import imp = new Import();
                ImportOutput op = imp.CreateImport(productId, userId, cusno.ToString(), subsno.ToString());
                return op;
            }
            catch (Exception ex)
            {
                new Logger("CreateImport() - failed for productId:" + productId + ", userId:" + userId + ", cusno:" + cusno + ", subsno:" + subsno, ex.ToString());
                return null;
            }
        }


        //130617 - newer used, import seems to do the work
        //public static EntitlementOutput CreateEntitlement(string productId, string userId, long cusno, long subsno, DateTime subsStart, DateTime subsEnd)
        //{
        //    try
        //    {
        //        Entitlement ent = new Entitlement();
        //        EntitlementOutput eo = ent.CreateEntitlement(productId,
        //                                                     userId,
        //                                                     cusno.ToString(),
        //                                                     subsno.ToString(),
        //                                                     BonDigMisc.GetMsSince1970(subsStart.Date.AddHours(-1)),
        //                                                     BonDigMisc.GetMsSince1970(subsEnd.Date.AddDays(1)));
        //        return eo;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("CreateTmpEntitlement() - failed for userId:" + userId + ", cusno:" + cusno + ", subsno:" + subsno, ex.ToString());
        //    }

        //    return null;
        //}

        //public static EntitlementOutput CreateTmpEntitlement(string productId, string userId, long cusno, long subsno)
        //{
        //    try
        //    {
        //        Entitlement ent = new Entitlement();
        //        EntitlementOutput eo = ent.CreateEntitlement(productId,
        //                                                     userId,
        //                                                     "tmp-" + cusno.ToString(),
        //                                                     "tmp-" + subsno.ToString(),
        //                                                     BonDigMisc.GetMsSince1970(DateTime.Now.AddHours(-5)),
        //                                                     BonDigMisc.GetMsSince1970(DateTime.Now.AddDays(6)));
        //        return eo;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("CreateTmpEntitlement() - failed for userId:" + userId + ", cusno:tmp-" + cusno.ToString(), ex.ToString());
        //    }

        //    return null;
        //}


        public static bool TryAddSubsAsBonDigImports(string token, long cirixCusno, UserOutput user)
        {
            string metName = "TryAddSubsAsBonDigImports()";

            if (user == null || user.user == null || string.IsNullOrEmpty(user.user.id))
            {
                new Logger(metName + " - user==null || user.id=='' for token:" + token + ", cirixCusno:" + cirixCusno, "not an exception");
                return false;
            }

            if (string.IsNullOrEmpty(token) || cirixCusno < 1)
            {
                return false;
            }
            var importAdded = false;
            try
            {
                DataSet ds = SubscriptionController.GetSubscriptions(cirixCusno, true);
                if (!DbHelpMethods.DataSetHasRows(ds))
                {
                    new Logger(metName + " / GetSubscriptions() - no subs for cusno: " + cirixCusno, "not an exception");
                }
                else
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        long subsno = long.Parse(dr["SUBSNO"].ToString());
                        long subsCusno = long.Parse(dr["SUBSCUSNO"].ToString());
                        string productNo = dr["PRODUCTNO"].ToString();
                        string paperCode = dr["PAPERCODE"].ToString();

                        #region using Entitlement instead of Import
                        //DateTime startDate = DateTime.Parse(dr["SUBSSTARTDATE"].ToString());
                        //DateTime endDate = DateTime.Parse(dr["SUBSENDDATE"].ToString());

                        //DateTime invStart = DbHelpMethods.SetDateFromDbFieldName(dr, "INVSTARTDATE");
                        //DateTime subsStart = DbHelpMethods.SetDateFromDbFieldName(dr, "SUBSSTARTDATE");
                        //DateTime start = (MiscFunctions.DateHasBeenSet(invStart)) ? invStart : subsStart;

                        //DateTime subsEnd = DbHelpMethods.SetDateFromDbFieldName(dr, "SUBSENDDATE");
                        //DateTime susp = DbHelpMethods.SetDateFromDbFieldName(dr, "SUSPENDDATE");
                        //DateTime unp = DbHelpMethods.SetDateFromDbFieldName(dr, "UNPBREAKDATE");

                        //DateTime end = subsEnd;
                        //if (MiscFunctions.DateHasBeenSet(susp) && susp < end)   end = susp;
                        //if (MiscFunctions.DateHasBeenSet(unp) && unp < end)     end = unp;

                        //if (BonDigHandler.CreateEntitlement(prodId, user.user.id, subsCusno, subsno, start, end) != null)
                        #endregion

                        if (cirixCusno == subsCusno)
                        {
                            string prodId = Settings.GetBonDigProductId(paperCode, productNo);
                            if (!string.IsNullOrEmpty(prodId))
                            {
                                if (CreateImport(prodId, user.user.id, subsCusno, subsno) != null)
                                    importAdded = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger(metName + " - failed for cusno:" + cirixCusno + "token:" + token + ", user.user.id" + user.user.id, ex.ToString());
            }
            return importAdded;
        }


        public static string TryGetCustHasDiffCusnosInBonDigMess(string token, long cirixCusno)
        {
            StringBuilder sb = new StringBuilder();
            foreach (long cn in RequestHandler.TryGetCirixCusnosFromBonDig(token))
            {
                if (cn != cirixCusno)
                    sb.Append(cn.ToString() + " ");
            }

            if (sb.ToString().Length > 0)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append("Di-kontot kunde tyvärr inte kopplas till ditt kundnummer " + cirixCusno.ToString() + " ");
                sb2.Append("eftersom det redan är kopplat till kundnummer ");
                sb2.Append(sb.ToString());
                sb2.Append("<br>Var god kontakta kundtjänst. Ha gärna dessa uppgifter till hands.");

                new Logger("GetCustHasDiffCusnosInBonDigMess() - found other external cusnos in S+ for cusno: " + cirixCusno, "Other external cusnos in S+ " + sb.ToString());

                return sb2.ToString();
            }

            return string.Empty;
        }

        public static bool UserExistInServicePlus(string emailAddress)
        {
            string unusedString;
            return UserExistInServicePlus(emailAddress, out unusedString);
        }

        public static bool UserExistInServicePlus(string emailAddress, out string userId)
        {
            // Try to find Service Plus user by email (is unique) and return true and set userId
            userId = string.Empty;
            var servicePlusSearchResult = RequestHandler.SearchByEmail(emailAddress);
            int totalItems;
            if (int.TryParse(servicePlusSearchResult.totalItems, out totalItems))
            {
                if (totalItems > 0)
                {
                    userId = servicePlusSearchResult.users.id;
                    return totalItems > 0;
                }
            }
            return false;
        }

        public static List<long> GetUserEntitlementsExternalIds(string servicePlusUserId)
        {
            return RequestHandler.GetUserEntitlementsExternalIds(servicePlusUserId);
        }

        //public static void SendBonDigWelcomeMail(bool userAddedToBonDig, string email, string passwd)
        //{
        //    StringBuilder sb = new StringBuilder();
            
        //    sb.Append("Hej,<br><br>");
        //    sb.Append("Här kommer dina personliga inloggningsuppgifter till de digitala versionerna av Dagens industri. Där kan du bland annat läsa tidningarna i din läsplatta och som pdf på webben.<br><br>");
        //    sb.Append("Användarnamn: " + email + "<br>");
        //    sb.Append("Lösenord: ");
            
        //    if (userAddedToBonDig)
        //        sb.Append(passwd);
        //    else
        //        sb.Append("<i>Samma som tidigare.</i> <a href='http://login.di.se/password/forgot-password' target='_blank'>Glömt lösenordet?</a>");

        //    sb.Append("<br><br><br>Gör så här:<br>");
        //    sb.Append("1. Ladda ned appen Dagens industri kostnadsfritt i Appstore eller Google Play<br>");
        //    sb.Append("2. Logga in med dina inloggningsuppgifter högst upp i vänstra hörnet i appen<br>");
        //    sb.Append("3. Gå in på <a href='http://www.di.se/tidningen' target='_blank'>www.di.se/tidningen</a> för att läsa tidningen som pdf<br><br><br>");
        //    sb.Append("Trevlig läsning!<br><br>");
        //    sb.Append("Vänliga hälsningar,<br>");
        //    sb.Append("Dagens industri<br>");
        //    sb.Append("Di Kundtjänst<br>");
        //    sb.Append("Tel: 08-573 651 00");

        //#region old mail
        //sb.Append("Hej,<br><br>");
        //sb.Append("Här kommer dina personliga inloggningsuppgifter till Dagens industri i läsplattan.<br><br>");
        //sb.Append("Användarnamn: " + email + "<br>");
        //sb.Append("Lösenord: ");
        //if (userAddedToBonDig)
        //    sb.Append(passwd);
        //else
        //    sb.Append("<i>Samma som tidigare.</i> <a href='http://login.di.se/password/forgot-password' target='_blank'>Glömt lösenordet?</a>");
        //sb.Append("<br><br>Du hittar applikationen genom att i din läsplatta gå in på AppStore eller Google Play och ladda ");
        //sb.Append("ner Dagens industri. När du sedan startar applikationen hittar du \"Logga in\" ");
        //sb.Append("högst uppe i vänstra hörnet.<br><br>");
        //sb.Append("Vänliga hälsningar,<br><br>");
        //sb.Append("Di Kundtjänst<br>");
        //sb.Append("Dagens industri<br>");
        //sb.Append("Tel: 08-573 651 00<br>");
        //sb.Append("Hemsida: www.di.se/tidningen");
        //#endregion

        //    MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), email, "Välkommen till Dagens industri digitalt", sb.ToString(), true);
        //}



        //private static SubsOutput CreateSubs(long cusno, string userId)
        //{
        //    try
        //    {
        //        Subs sub = new Subs();
        //        SubsOutput so = sub.CreateSubs(userId, cusno.ToString(), "");
        //        return so;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("CreateSubs() - failed for cusno: " + cusno.ToString(), ex.ToString());
        //    }

        //    return null;
        //}


    }
}
