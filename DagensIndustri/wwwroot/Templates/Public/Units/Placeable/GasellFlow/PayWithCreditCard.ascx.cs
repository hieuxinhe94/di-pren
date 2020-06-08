using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DIClassLib.CardPayment;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable.GasellFlow
{
    public partial class PayWithCreditCard : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        //public void RedirectToAuriga()
        //{
        //    string gasellFlowPageURL = "";

        //    int numOfParticipants = Convert.ToInt32(Request.QueryString["nop"]);
        //    int gasellID = Convert.ToInt32(Request.QueryString["gasellid"]);
        //    string paymentMethod = Request.QueryString["pm"];


        //    if (EPiFunctions.SettingsPageSetting(CurrentPage, "GasellFlowPage") != null)
        //    {
        //        PageData GasellFlowPage = new PageData();

        //        GasellFlowPage = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(CurrentPage, "GasellFlowPage") as PageReference);

        //        gasellFlowPageURL = GasellFlowPage.LinkURL;
        //    }

        //    PageData pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(gasellID));

        //    int totalPrice = numOfParticipants * Convert.ToInt32(pd["Price"]) * 100;

        //    int priceExl = ((totalPrice / 100) * 75) * 100;

        //    int invoiceVAT = totalPrice - priceExl;

        //    string returnURL = gasellFlowPageURL + "&gasellid=" + gasellID.ToString() + "&pm=" + paymentMethod + "&nop=" + numOfParticipants.ToString() + "&as=1";
        //    string cancelURL = EPiServer.DataFactory.Instance.GetPage(PageReference.StartPage).LinkURL;

        //    returnURL = EPiFunctions.GetFriendlyAbsoluteUrl(returnURL);
        //    cancelURL = EPiFunctions.GetFriendlyAbsoluteUrl(cancelURL);

        //    AurigaPayment.DoAurigaPayment("SEK", totalPrice, priceExl, invoiceVAT, false, "", returnURL, cancelURL, pd.PageName, "", "", "");
        //}
    }
}