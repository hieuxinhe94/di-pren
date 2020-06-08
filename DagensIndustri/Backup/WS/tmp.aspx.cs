using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DagensIndustri.Tools.Classes.Misc;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using WS.BonnierDigital;
using WS.Di;
using DIClassLib.EPiJobs.EpiDataForExternalUse;
//using DIClassLib.EPiJobs.EpiDataForExternalUse;


namespace WS
{
    public partial class tmp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool displayTestGui = false;
                bool.TryParse(MiscFunctions.GetAppsettingsValue("DisplayTestGui").ToLower(), out displayTestGui);

                if (displayTestGui)
                {
                    TextBoxDateMin.Text = DateTime.Now.AddDays(-10).ToString();
                    TextBoxDateMax.Text = DateTime.Now.ToString();

                    TextBoxDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    TextBoxIpNumber.Text = Request.ServerVariables["REMOTE_ADDR"];

                    PlaceholderTestGui.Visible = displayTestGui;
                }
            }
        }


        protected void ButtonDownloadFile_Click(object sender, EventArgs e)
        {
            var dl = new WS.FileDownload.FileDownload();
            //File.WriteAllBytes("", dl.DownloadFile())

            DateTime dt;
            DateTime.TryParse(TextBoxDate.Text, out dt);
            
            dl.DownloadFile(dt, TextBoxServicePlusUserId.Text, TextBoxIpNumber.Text, TextBoxSiteProvidedDownload.Text);

            //var doc = DIClassLib.DocTrackr.DocTrackrUtil.TryGetPathToDocForDownload(new DateTime(2014, 2, 19), "apa11", "123.123.123.111", "ws.dagensindustri.se");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                BonnierDigital.BonnierDigital bd = new BonnierDigital.BonnierDigital();
                long cusno = int.Parse(TextBox1.Text);
                PlusCustomer pc = bd.GetPlusCustomer(cusno);
                Response.Write("cusno " + pc.CustomerId.ToString() + 
                               "<br>IsActivePlusCust " + pc.IsActivePlusSubscriber.ToString() +
                               "<br>DateSubsEnd " + pc.DateSubsEnd.ToString());
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                BonnierDigital.BonnierDigital bd = new BonnierDigital.BonnierDigital();
                List<long> cusnos = bd.GetUpdatedCusnosInDateInterval(TextBoxDateMin.Text, TextBoxDateMax.Text);
                //List<long> cusnos = bd.GetUpdatedCusnosInDateInterval(DateTime.Now.AddDays(-10), DateTime.Now);
                int i = 0;
                foreach (long cn in cusnos)
                {
                    Response.Write(cn.ToString() + "<br>");
                    
                    i++;
                    if (i > 10) break;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }


        protected void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                Di.Di di = new Di.Di();;
                string s = di.GetSingleSignOnCodeByCusno(long.Parse(TextBoxCusno.Text));
                LabelCode.Text = s;
                
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }


        //private void EncryptDecrypt()
        //{
        //    DIClassLib.Security.EncryptDecrypt sec = new DIClassLib.Security.EncryptDecrypt();
        //    string key = sec.Encrypt("700131-1234");

        //    se.dagensindustri.ws.WineClub wc = new se.dagensindustri.ws.WineClub();
        //    string result = wc.IsActiveDiGoldMember(key);

        //    Response.Write("result: " + result);
        //}


        //EncryptDecrypt();

        //BusinessCalendarDbHandler.GetCompanysFromMS();
        //BusinessCalendarDbHandler.GetCompanyEventsFromMS();
        //BusinessCalendarDbHandler.GetSubscription(new Guid("d709083c-7e1f-454e-bb89-e0dc674df215"));

        //Response.Write("testararar");


        //string s = Functions.GetAppsettingsValue("bonnierDigitalWsUsesCirixTestWs");

        //WS.BonnierDigital.BonnierDigital bd = new WS.BonnierDigital.BonnierDigital();
        //PlusCustomer pc1 = bd.GetPlusCustomer(3520985);
        //PlusCustomer pc2 = bd.GetPlusCustomer(6471703);
        //PlusCustomer pc3 = bd.GetPlusCustomer(3320883);
        //PlusCustomer pc4 = bd.GetPlusCustomer(3513116);

        //Response.Write("1: " + pc1.CustomerId.ToString() + " " + pc1.IsPlusSubscriber.ToString() + "<br>");
        //Response.Write("2: " + pc2.CustomerId.ToString() + " " + pc2.IsPlusSubscriber.ToString() + "<br>");
        //Response.Write("3: " + pc3.CustomerId.ToString() + " " + pc3.IsPlusSubscriber.ToString() + "<br>");
        //Response.Write("4: " + pc4.CustomerId.ToString() + " " + pc4.IsPlusSubscriber.ToString() + " " + pc4.DateSubsStart.ToString() + "<br>");

        //DIClassLib.Security.EncryptDecrypt sec = new DIClassLib.Security.EncryptDecrypt();

        //WS.WineClub.WineClub wc = new WineClub.WineClub();
        //string s1 = wc.GetDiGoldMemberCusno(sec.Encrypt("1234"));
        //string s2 = wc.GetDiGoldMemberCusno(sec.Encrypt("12345678"));
        //string s3 = wc.GetDiGoldMemberCusno(sec.Encrypt("197607131234"));

        //(string s = sec.Encrypt("197607131234");

    }
}