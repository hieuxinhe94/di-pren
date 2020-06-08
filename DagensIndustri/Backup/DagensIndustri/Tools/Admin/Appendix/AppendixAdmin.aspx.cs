using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.IO;
using EPiServer;
using EPiServer.PlugIn;
using System.Data;
using System.Net;
using System.Collections.Generic;
using DIClassLib.DbHelpers;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.Extras;
using DIClassLib.Misc;



namespace DagensIndustri.Tools.Admin.Appendix
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Bilageadmin", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Bilageadmin", UrlFromUi = "/Tools/Admin/Appendix/AppendixAdmin.aspx", SortIndex = 1000)]
    public partial class AppendixAdmin : System.Web.UI.Page
    {

        private void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBindAppendix();
            }
        }

        private void DataBindAppendix()
        {
            SqlDataReader dr = null;

            try
            {
                dr = SqlHelper.ExecuteReader("DISEMiscDB", "getPdfBilagor");
                repeatBilagor.DataSource = dr;
                repeatBilagor.DataBind();
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString());
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
        }

        #region Appendix delete

        protected void LbDeleteAppendixOnClick(object sender, EventArgs e)
        {

            try
            {
                string[] arrCommandArguments = ((LinkButton)sender).CommandArgument.Split('|');

                int appendixId = int.Parse(arrCommandArguments[0]);
                string created = arrCommandArguments[1];

                //delete appendix from db
                SqlHelper.ExecuteNonQuery("DISEMiscDB", "deletePdfBilaga", new SqlParameter[] { new SqlParameter("@id", appendixId) });

                //delete .gif from filearea
                File.Delete(MiscFunctions.GetAppsettingsValue("PDFArchivePath") + "Bilagor\\" + appendixId + ".gif");

                //delete .pdf from filearea
                File.Delete(MiscFunctions.GetAppsettingsValue("PDFArchivePath") + "Bilagor\\" + appendixId + ".pdf");

                //Delete gif image on di.se ftp server
                FtpHelper ftp = new FtpHelper("FtpHost", "FtpUser", "FtpPassword");
                //delete file
                ftp.DeleteFileFromFtp("/bilaga_" + created + ".gif");

                DataBindAppendix();
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString());
            }
        }

        #endregion

        #region Appendix details

        protected void LbShowAppendixDetailsOnClick(object sender, EventArgs e)
        {
            Show("PhAppendixDetails|PhNotProcessed");
            FillRadioButtonListWithFiles();

            int appendixId = int.Parse(((LinkButton)sender).CommandArgument);

            setAppendixDetails(appendixId);
            BtnUpdate.Visible = true;
            BtnAdd.Visible = false;
        }

        private void setAppendixDetails(int ID)
        {
            SqlDataReader dr = default(SqlDataReader);
            try
            {
                dr = SqlHelper.ExecuteReader("DISEMiscDB", "DiseGetBilagor", new SqlParameter("@bilageId", ID));

                if (dr.Read())
                {
                    txtHeadLine.Text = dr["headLine"] as string ?? string.Empty;
                    txtUtgivare.Text = dr["publisher"] as string ?? string.Empty;
                    txtAmne.Text = dr["subject"] as string ?? string.Empty;
                    txtOvrigt.Text = dr["other"] as string ?? string.Empty;
                    txtPublishDate.Text = DateTime.Parse(dr["created"].ToString()).ToString("yyyy-MM-dd");
                    txtId.Text = ID.ToString();

                    //set prop publishdate (used on update if new publishdate)
                    AppendixPublishDate = txtPublishDate.Text;

                    HlPdf.NavigateUrl = "\\tools\\operations\\stream\\DownloadPDF.aspx?appendix=true&issueid=" + ID;
                    HlPdf.Text = ID + ".pdf (nuvarande pdf)";

                    HlGif.NavigateUrl = "\\tools\\operations\\stream\\showimage.aspx?what=appendix&imgname=bilagor\\" + ID;
                    HlGif.Text = ID + ".gif (nuvarande bild)";
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString());
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
        }

        #endregion

        #region Appendix add

        protected void LbShowAppendixAddOnClick(object sender, EventArgs e)
        {
            Show("PhAppendixDetails|PhNotProcessed");
            FillRadioButtonListWithFiles();

            txtHeadLine.Text = string.Empty;
            txtUtgivare.Text = string.Empty;
            txtAmne.Text = string.Empty;
            txtOvrigt.Text = string.Empty;
            HlPdf.NavigateUrl = string.Empty;
            HlPdf.Text = string.Empty;
            HlGif.NavigateUrl = string.Empty;
            HlGif.Text = string.Empty;
            txtPublishDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            BtnUpdate.Visible = false;
            BtnAdd.Visible = true;
        }

        protected void BtnAddOnClick(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 360;

            if (!string.IsNullOrEmpty(RadioButtonListPDF.SelectedValue) && !string.IsNullOrEmpty(RadiobuttonlistGIF.SelectedValue))
            {
                try
                {
                    string pdfArchivePath = MiscFunctions.GetAppsettingsValue("PDFArchivePath");
                    string pathPdf = pdfArchivePath + "Bilagor\\NotProcessed\\" + RadioButtonListPDF.SelectedValue;
                    string pathGif = pdfArchivePath + "Bilagor\\NotProcessed\\" + RadiobuttonlistGIF.SelectedValue;

                    FileInfo filePdf = new FileInfo(pathPdf);
                    FileInfo fileGif = new FileInfo(pathGif);

                    //Get size of pdf
                    int size = (int)System.Math.Round((double)filePdf.Length / 1000000, 0);
                    //Insert appendix to db and get ID
                    int id = InsertAppendix(txtHeadLine.Text, txtUtgivare.Text, txtAmne.Text, txtOvrigt.Text, size, DateTime.Parse(txtPublishDate.Text));

                    string movePdfPath = pdfArchivePath + "Bilagor\\" + id + filePdf.Extension;
                    string moveGifPath = pdfArchivePath + "Bilagor\\" + id + fileGif.Extension;

                    //Move pdf and gif
                    filePdf.MoveTo(movePdfPath);
                    fileGif.MoveTo(moveGifPath);

                    DataBindAppendix();
                    Show("PhAppendixList");

                    //Upload gif image to di.se ftp server
                    FtpHelper ftp = new FtpHelper("FtpHost", "FtpUser", "FtpPassword");
                    ftp.UploadFileToFtp(moveGifPath, "/bilaga_" + txtPublishDate.Text + ".gif");
                }
                catch (Exception ex)
                {
                    new Logger("BtnAddOnClick() - failed", ex.ToString());
                    //TODO: tmp hide error due to error on DI ftp. File gets uploaded though.
                    ShowError(ex.ToString());
                }
            }
            else
            {
                ShowError("Du måste välja en PDF och en GIF bild!");
            }

        }

        private int InsertAppendix(string headLine, string publisher, string subject, string other, int size, DateTime created)
        {
            try
            {
                SqlParameter paramId = default(SqlParameter);
                paramId = new SqlParameter("@id", SqlDbType.Int);
                paramId.Direction = ParameterDirection.Output;

                SqlParameter[] sqlParameters = new SqlParameter[] {
                    new SqlParameter("@HeadLine", headLine),
                    new SqlParameter("@Publisher", publisher),
                    new SqlParameter("@Subject", subject),
                    new SqlParameter("@Other", other),
                    new SqlParameter("@size", size),
                    new SqlParameter("@created", created),
                    paramId
                };

                SqlHelper.ExecuteNonQuery("DISEMiscDB", "insertPDFBilaga", sqlParameters);
                return int.Parse(paramId.Value.ToString());
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString());
                return 0;
            }
        }

        #endregion

        #region Appendix update

        protected void BtnUpdateOnClick(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 360;

            int size = 0;
            int ID = int.Parse(txtId.Text);

            string pdfArchivePath = MiscFunctions.GetAppsettingsValue("PDFArchivePath");

            if (ID > 0)
            {
                if (!string.IsNullOrEmpty(RadioButtonListPDF.SelectedValue))
                {
                    FileInfo filePdf = new FileInfo(pdfArchivePath + "Bilagor\\NotProcessed\\" + RadioButtonListPDF.SelectedValue);
                    size = (int)System.Math.Round((double)filePdf.Length / 1000000, 0);
                    File.Delete(pdfArchivePath + "Bilagor\\" + ID + filePdf.Extension);
                    filePdf.MoveTo(pdfArchivePath + "Bilagor\\" + ID + filePdf.Extension);
                }

                if (!string.IsNullOrEmpty(RadiobuttonlistGIF.SelectedValue))
                {
                    FileInfo fileGif = new FileInfo(pdfArchivePath + "Bilagor\\NotProcessed\\" + RadiobuttonlistGIF.SelectedValue);
                    string gifPath = pdfArchivePath + "Bilagor\\" + ID + fileGif.Extension;
                    File.Delete(gifPath);
                    fileGif.MoveTo(gifPath);

                    //Upload the new gif image to di.se ftp server
                    FtpHelper ftp = new FtpHelper("FtpHost", "FtpUser", "FtpPassword");
                    //delete file
                    ftp.DeleteFileFromFtp("/bilaga_" + AppendixPublishDate + ".gif");
                    //upload file
                    ftp.UploadFileToFtp(gifPath, "/bilaga_" + txtPublishDate.Text + ".gif");
                }

                try
                {
                    SqlParameter[] sqlParameters = new SqlParameter[] {
                        new SqlParameter("@id", txtId.Text),
                        new SqlParameter("@HeadLine", txtHeadLine.Text), 
                        new SqlParameter("@Publisher", txtUtgivare.Text), 
                        new SqlParameter("@Subject", txtAmne.Text),
                        new SqlParameter("@Other", txtOvrigt.Text), 
                        new SqlParameter("@size", size),
                        new SqlParameter("@created",  txtPublishDate.Text)
                    };

                    SqlHelper.ExecuteNonQuery("DISEMiscDB", "updatePdfBilaga", sqlParameters);

                    //If no new file is selected and publish date is changed, update DI-ftp
                    if (string.IsNullOrEmpty(RadiobuttonlistGIF.SelectedValue) && txtPublishDate.Text != AppendixPublishDate)
                    {
                        FtpHelper ftp = new FtpHelper("FtpHost", "FtpUser", "FtpPassword");
                        ftp.RenameFileOnFtp("/bilaga_" + AppendixPublishDate + ".gif", "bilaga_" + txtPublishDate.Text + ".gif");
                    }

                    DataBindAppendix();
                    Show("PhAppendixList");
                }
                catch (Exception ex)
                {
                    ShowError(ex.ToString());
                }
            }
        }

        #endregion

        #region Help stuff

        public void FillRadioButtonListWithFiles()
        {
            //clear items first            
            RadioButtonListPDF.Items.Clear();
            RadiobuttonlistGIF.Items.Clear();

            foreach (string fileName in getFiles("*.pdf"))
                RadioButtonListPDF.Items.Add(new ListItem(fileName));
            //RadioButtonListPDF.Items.Add(new ListItem("<a target=\"_new\" href=\"\\functions\\stream\\DownloadPDF.aspx?appendix=true&issueid=notprocessed\\" + fileName.Replace(".pdf", "") + "\">" + fileName + "</a> ", fileName));

            foreach (string fileName in getFiles("*.gif"))
                RadiobuttonlistGIF.Items.Add(new ListItem(fileName));
            //RadiobuttonlistGIF.Items.Add(new ListItem("<a target=\"_new\" href=\"\\functions\\stream\\showimage.aspx?what=appendix&imgname=bilagor\\notprocessed\\" + fileName.Replace(".gif", "") + "\">" + fileName + "</a> ", fileName));
        }

        public ArrayList getFiles(string searchPattern)
        {
            ArrayList tmpFileArr = new ArrayList();
            DirectoryInfo folder = new DirectoryInfo(MiscFunctions.GetAppsettingsValue("PDFArchivePath") + "Bilagor\\NotProcessed\\");

            foreach (FileInfo file in folder.GetFiles(searchPattern))
                tmpFileArr.Add(file.Name);

            return tmpFileArr;
        }

        private void ShowError(string strMessage)
        {
            LblError.Text = strMessage;
        }

        /// <summary>
        /// Show or hide placeholders
        /// </summary>
        /// <param name="placeHolderIDs">The ID of the placeholders you want to show, separated with |</param>
        protected void Show(string placeHolderIDs)
        {
            foreach (object control in form1.Controls)
            {
                if (control.GetType().Equals(typeof(PlaceHolder)))
                {
                    List<string> placeHolders = new List<string>(placeHolderIDs.Split('|'));

                    PlaceHolder ph = (PlaceHolder)control;
                    ph.Visible = placeHolders.Contains(ph.ID);
                }
            }
        }

        protected void Show(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;

            Show(lb.CommandArgument);
        }

        private string AppendixPublishDate
        {
            get
            {
                return (string)ViewState["AppendixPublishDate"];
            }
            set
            {
                ViewState["AppendixPublishDate"] = value;
            }
        }

        #endregion

    }
}