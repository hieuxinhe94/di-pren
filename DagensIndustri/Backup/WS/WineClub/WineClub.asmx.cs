using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DIClassLib.Security;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;


namespace WS.WineClub
{
    /// <summary>
    /// Summary description for WineClub
    /// </summary>
    [WebService(Namespace = "http://ws.dagensindustri.se/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WineClub : System.Web.Services.WebService
    {

        /// <summary>
        /// Use method to get customer number (cusno) for active member of DI Gold. 
        /// Customer must have an active subscription AND choosen to be part of DI Gold.
        /// </summary>
        /// <param name="encryptedString">Encrypted Swedish birth number</param>
        /// <returns>"123456" (cusno) / "false" (could not find an active di gold member) / "-1" (on error)</returns>
        [WebMethod]
        public string GetDiGoldMemberCusno(string encryptedString)
        {
            try
            {
                EncryptDecrypt sec = new EncryptDecrypt();
                string birthNo = sec.Decrypt(encryptedString);

                foreach (long cusno in MembershipDbHandler.GetCusnosByBirthNo(birthNo))
                {
                    if (MembershipDbHandler.CustHasActiveSubs(cusno) && MembershipDbHandler.IsInRole(cusno, "DiGold"))
                        return cusno.ToString();
                }
                
                return "false";
            }
            catch (Exception ex)
            {
                new Logger("GetDiGoldMemberCusno() failed", ex.ToString());
                return "-1";
            }

        }
    }
}
