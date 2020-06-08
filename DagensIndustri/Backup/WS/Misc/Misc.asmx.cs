using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using System.Data;
using DIClassLib.Membership;


namespace WS.Misc
{
    /// <summary>
    /// Summary description for Misc
    /// </summary>
    [WebService(Namespace = "http://ws.dagensindustri.se/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Misc : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// Check if customer number belongs to a customer that is Di Gold member
        /// </summary>
        /// <param name="cusno">Cirix cusno</param>
        /// <returns>Bool</returns>
        [WebMethod]
        public bool CustomerIsDiGoldMember(long cusno)
        {
            try
            {
                DataSet ds = MsSqlHandler.GetCustomerInRoleRow(cusno, 2);  //2 = roleid for diGold
                if (DbHelpMethods.DataSetHasRows(ds))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                new Logger("CustomerIsDiGoldMember() failed for cusno: " + cusno.ToString(), ex.ToString());
                return false;
            }
        }
    }
}
