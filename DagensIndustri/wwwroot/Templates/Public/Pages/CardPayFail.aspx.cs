using System;
using System.Collections.Generic;
using System.Linq;
using DIClassLib.CardPayment.Nets;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Misc;
using System.Text;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class CardPayFail : DiTemplatePage
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Betalning med kort misslyckades.");

            var trans = MiscFunctions.REC(Request.QueryString["transactionId"]);
            if (!string.IsNullOrEmpty(trans))
            {
                var qp = new QueryPayment(trans);
                if (!qp.PaymentOk)
                    sb.Append("<br><br>Detaljerat fel:<br>" + MiscFunctions.GetNetsCardPayStatus("E", qp.ErrorResponseCode));
            }

            UserMessageControl1.ShowMessage(sb.ToString(), false, true);
        }
    }
}