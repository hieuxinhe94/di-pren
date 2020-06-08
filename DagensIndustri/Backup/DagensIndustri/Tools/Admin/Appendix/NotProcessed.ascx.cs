using System;
using System.IO;
using System.Data;
using System.Net;
using System.Collections.Generic;
using System.Collections;
using System.Web.UI.WebControls;
using DIClassLib.Misc;
using DIClassLib.DbHelpers;


namespace DagensIndustri.Tools.Admin.Appendix
{
    public partial class NotProcessed : System.Web.UI.UserControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            LoadFiles("*");
        }

        protected void LoadFiles(string searchPattern)
        {
            RepFiles.DataSource = ((AppendixAdmin)base.Page).getFiles(searchPattern);
            RepFiles.DataBind();
        }

        protected void BtnUploadOnClick(object sender, EventArgs e)
        {
            List<string> allowedFileTypes = new List<string> { "gif", "pdf" };

            string fileToUploadName = FileInput.PostedFile.FileName;

            if (fileToUploadName == "")
            {
                LbError.Text = "Du måste ange en fil";
            }
            else if (!allowedFileTypes.Contains(fileToUploadName.Substring(fileToUploadName.LastIndexOf(".") + 1)))
            {
                LbError.Text = "Felaktig filtyp";
            }
            else
            {
                try
                {
                    string pdfArchivePath = MiscFunctions.GetAppsettingsValue("PDFArchivePath");

                    string serverFileName = Path.GetFileName(FileInput.PostedFile.FileName);

                    FileInput.PostedFile.SaveAs(pdfArchivePath + "Bilagor\\NotProcessed\\" + serverFileName);

                    //Refill radiobuttons
                    ((AppendixAdmin)base.Page).FillRadioButtonListWithFiles();
                }
                catch (Exception ex)
                {
                    new Logger("NotProcessed.ascx/BtnUploadOnClick() - failed", ex.ToString());
                    LbError.Text = ex.ToString();
                }
            }
        }

        protected void IbDeleteFileOnClick(object sender, EventArgs e)
        {
            string fileName = ((ImageButton)sender).CommandArgument;

            File.Delete(MiscFunctions.GetAppsettingsValue("PDFArchivePath") + "Bilagor\\notprocessed\\" + fileName);

            //Refill radiobuttons
            ((AppendixAdmin)base.Page).FillRadioButtonListWithFiles();
        }

        protected string GetLink(string fileName)
        {
            if (fileName.ToLower().EndsWith(".pdf"))
                return "<a target=\"_blank\" href=\"\\tools\\operations\\stream\\DownloadPDF.aspx?appendix=true&issueid=notprocessed\\" + fileName.Replace(".pdf", "") + "\">" + fileName + "</a>";
            else
                return "<a target=\"_blank\" href=\"\\tools\\operations\\stream\\showimage.aspx?what=appendix&imgname=bilagor\\notprocessed\\" + fileName.Replace(".gif", "") + "\">" + fileName + "</a>";
        }
    }
}