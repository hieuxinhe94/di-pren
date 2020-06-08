using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DagensIndustri.Tools.Classes.BaseClasses;

using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.Misc;

namespace DagensIndustri
{
    public partial class tmp2 : DiTemplatePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            var me = new ApsisCustomer()
            {
                Email = "persyl@gmail.com",
                ApsisProjectGuid = "663d1987-ce1a-4459-9d25-dce3769e27a7",
                CustomerId = 3660875,
                SubsStartDate = DateTime.Parse("2013-08-02"),
                f_PaperCode = "AGENDA",
                f_ProductNo = "01",
                SubsLenMonsFromCirix = 777
            };
            var ah = new ApsisWsHandler();

            var res = ah.ApsisSendEmail(new Guid().ToString(), me);
            */

            /*
            int campId = 54;
            var camp = CirixDbHandler.GetCampaign(campId);
            if (!DbHelpMethods.DataSetHasRows(camp))
            {
                litOutput.Text = "campId = " + campId + " är tomt.";
            }
            else
            {
                string paperCode = camp.Tables[0].Rows[0]["PAPERCODE"].ToString();
                string productNo = camp.Tables[0].Rows[0]["PRODUCTNO"].ToString();
                string subsLength = camp.Tables[0].Rows[0]["SUBSLENGTH"].ToString();
                
                litOutput.Text = "campId = " + campId + ": PAPERCODE = " + paperCode;
                litOutput.Text = ", PRODUCTNO = " + productNo;
                litOutput.Text = ", SUBSLENGTH = " + subsLength;
                litOutput.Text = ", PRODUCTNAME = " + CirixDbHandler.GetProductName(paperCode, productNo);
            }
             */
        }
    }
}