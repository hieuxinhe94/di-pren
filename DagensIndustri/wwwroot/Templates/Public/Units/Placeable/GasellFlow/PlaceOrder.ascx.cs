using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Units.Placeable.GasellFlow
{
    public partial class PlaceOrder : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
 	        base.OnLoad(e);

            if (Request.QueryString["responseCode"] == null)
            {
                int numOfParticipants = Convert.ToInt32(Request.QueryString["nop"]);
                int gasellID = Convert.ToInt32(Request.QueryString["gasellid"]);
                string paymentMethod = Request.QueryString["pm"];

                if (gasellID <= 0)
                    return;

                PageData pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(gasellID));

                int totalPrice = (numOfParticipants - (numOfParticipants / 4)) * Convert.ToInt32(pd["Price"]);

                //int i;
                //for (i = 1; i <= numOfParticipants / 4; i++)
                //{
                //    if((numOfParticipants / 4) > 0)
                //        totalPrice = totalPrice - Convert.ToInt32(pd["Price"]);
                //}         

                if (paymentMethod == "3")
                    totalPrice = 0;

                string ticket = String.Empty;

                if (numOfParticipants > 1)
                    ticket = Translate("/gasell/flow/text/tickets");
                else
                    ticket = Translate("/gasell/flow/text/ticket");


                DesciptionLiteral.Text = numOfParticipants.ToString() + " " + Translate("/gasell/flow/text/quantity") + " " + ticket + " " + Translate("/gasell/flow/text/to") + " " + pd.PageName;

                PriceLiteral.Text = Translate("/gasell/flow/text/totalprice") + " " + "<strong>" + totalPrice.ToString() + " " + Translate("/gasell/flow/text/sek") + "</strong>" + " " + Translate("/gasell/flow/text/withouttaxes");
            }
        }
    }
}