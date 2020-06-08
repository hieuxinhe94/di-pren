using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow
{
    public partial class PlaceOrder : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
 	        base.OnLoad(e);

            if (Request.QueryString["responseCode"] == null)
            {
                int numOfParticipants = Convert.ToInt32(Request.QueryString["nop"]);
                int eventId = Convert.ToInt32(Request.QueryString["eventId"]);
                string paymentMethod = Request.QueryString["pm"];

                if (!string.IsNullOrEmpty(paymentMethod))
                {
                    PageData pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(eventId));
                    ImageConfirmTicket.ImageUrl = pd["ImageConfirmTicket"].ToString();

                    int totalPrice = (numOfParticipants - (numOfParticipants / 4)) * Convert.ToInt32(pd["Price"]);

                    string ticket = String.Empty;
                    if (numOfParticipants > 1)
                        ticket = Translate("/signup/flow/text/tickets");
                    else
                        ticket = Translate("/signup/flow/text/ticket");

                    DesciptionLiteral.Text = numOfParticipants.ToString() + " " + Translate("/signup/flow/text/quantity") + " " + ticket + " " + Translate("/signup/flow/text/to") + " " + pd.PageName;
                    PriceLiteral.Text = Translate("/signup/flow/text/totalprice") + " " + "<strong>" + totalPrice.ToString() + " " + Translate("/signup/flow/text/sek") + "</strong>";  //+" " + Translate("/signup/flow/text/withouttaxes");
                }
            }
        }
    }
}