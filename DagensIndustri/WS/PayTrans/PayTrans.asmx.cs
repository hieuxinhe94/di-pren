using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Services;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;

namespace WS.PayTrans
{
    /// <summary>
    /// Summary description for PayTrans
    /// </summary>
    [WebService(Namespace = "http://ws.dagensindustri.se/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PayTrans : WebService
    {
        [WebMethod]
        public bool InsertPayTrans(string accessToken, int diPayTransRefNo, int merchantId, string currency, int amountInOren,
            int vatInOren, string paymentMethod, DateTime purchaseDate, string goodsDescription,
            string comment, string consumerName, string email, string cardType, string transactionId,
            string status, string statusCode, DateTime finishDate)
        {
            if (!ValidateRequest(accessToken))
            {
                return false;
            }
            return MsSqlHandler.InsertPayTransFromDi(diPayTransRefNo,
                merchantId,
                currency,
                amountInOren,
                vatInOren,
                paymentMethod,
                purchaseDate,
                goodsDescription,
                comment,
                consumerName,
                email,
                cardType,
                transactionId,
                status,
                statusCode,
                finishDate);
        }

        /// <summary>
        /// Make sure this logic is used for building accessToken in applications that calls any webservice that using this method
        /// </summary>
        /// <returns>True if accessToken is valid</returns>
        private bool ValidateRequest(string accessToken)
        {
            var secret = ConfigurationManager.AppSettings["InsertPayTransSecret"];
            if (string.IsNullOrEmpty(secret))
            {
                return false;
            }
            
            //The logic for correct accessToken
            var stringToProcess = string.Format("{0}_{1}_{2}_{3}", DIClassLib.Misc.MiscFunctions.GetMd5Hash(secret), DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var token = DIClassLib.Misc.MiscFunctions.GetMd5Hash(stringToProcess);
            return accessToken == token;
        }
    }
}