using System;
using EPiServer;
using EPiServer.PlugIn;
using System.Web.UI.WebControls;
using System.Collections.Generic;
//using EPiServer.Utility.Jobs.Apsis;
//using EPiServer.Utility.DbHandlers;

namespace DagensIndustri.Tools.Admin.Apsis
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Apsis admin", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Apsis admin", UrlFromUi = "/Tools/Admin/Apsis/ApsisAdmin.aspx", SortIndex = 1040)]
    public partial class ApsisAdmin : System.Web.UI.Page
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
                HandleUCVisibility(true, false);

            #region test
            //test area
            //Utility.Jobs.Apsis.MyJob.Execute();

            //EPiServer.Utility.Jobs.Apsis.MailSenderDbHandler db = new MailSenderDbHandler();

            //List<Customer> custs = db.MssqlGetNewSendSuccessCusts();
            //string sx = "";

            //List<Customer> custs = db.MssqlGetBouncedCusts();

            //db.ApsisGetBounces();
            //db.MssqlAddBouncesFromApsis();
            //db.CirixGetNewCusts();

            //string s = db.CirixGetEmailFromCustomerTable(3453550);
            //db.CirixUpdateEmailInCustomerTable(3453550, "studsar2@di.se");
            //s = db.CirixGetEmailFromCustomerTable(3453550);

            //db.CirixEmailIsUnique("cissigthln4@gmail.com");
            //List<string> freeSubsIds = db.CirixGetFreeSubsCampIds();
            //List<string> compSubsIds = db.CirixGetCompanySubsCampIds();

            //Customer c = CirixDbHandler.CirixGetCustomer(3453581);
            //Customer c = db.CirixGetCustomer(3453558);
            //Customer c2 = db.CirixGetCustomer(3453556);

            //Customer c = CirixDbHandler.CirixGetCustomer(3453566);

            //string s = db.CirixGetUpdatedStatus(3453558);
            //string s2 = db.CirixGetUpdatedStatus(3453556);

            //bool bo = CirixDbHandler.CirixEmailIsUnique("cissitstr@gmail.com");


            //System.Data.DataSet ds1 = CirixDbHandler.GetGetCustomer(3453566);
            //string s = CirixDbHandler.GetWWWPassword(3453581);
            //System.Data.DataSet ds2 = CirixDbHandler.GetGetCustomer2(3453566);

            //List<Customer> custsNotBounced = db.MssqlGetSuccessfullySentCusts();
            //db.MssqlSetIsSendSuccess(custsNotBounced, true);

            //Customer c1 = CirixDbHandler.CirixGetCustomer(3453594);
            //CirixDbHandler.CirixUpdateEmail(3453594, "knas2");     //bounce3@di.se
            //Customer c2 = CirixDbHandler.CirixGetCustomer(3453594);
            #endregion
        }

        protected void ButtonDispUC_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (b.ID == "ButtonDispBounces")
                HandleUCVisibility(true, false);
            else if (b.ID == "ButtonDispRules")
                HandleUCVisibility(false, true);
        }

        private void HandleUCVisibility(bool dispBounces, bool dispRules)
        {
            Bounces1.Visible = dispBounces;
            Rules1.Visible = dispRules;
        }


    }
}