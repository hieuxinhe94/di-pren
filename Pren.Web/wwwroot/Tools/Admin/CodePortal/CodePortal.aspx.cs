using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using CsvHelper;
using Di.Common.Logging;
using Di.Common.Utils.Context;
using EPiServer;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using Pren.Web.Business.CodePortal;
using Pren.Web.Business.DataAccess.CodePortal;
using Pren.Web.Business.DataAccess.CodePortal.Entities;

namespace Pren.Web.Tools.Admin.CodePortal
{
    [GuiPlugIn(
        Area = PlugInArea.AdminMenu, 
        Description = "Verktyg för att hantera koder i kodportalen", 
        RequiredAccess = EPiServer.Security.AccessLevel.Administer, 
        DisplayName = "Kodportaladmin",
        UrlFromUi = "/Tools/Admin/CodePortal/CodePortal.aspx", 
        SortIndex = 2060)]
    public partial class CodePortal : System.Web.UI.Page
    {
        private ICodePortalService _codePortalService;
        private ILogger _logger;

        protected override void OnLoad(EventArgs e)
        {
            _codePortalService = ServiceLocator.Current.GetInstance<ICodePortalService>();
            _logger = ServiceLocator.Current.GetInstance<ILogger>();

            base.OnLoad(e);            

            DataBindControls();
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
           
            // Use EPiServer UI master page
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        protected void btnAddList_Click(object sender, EventArgs e)
        {
            ActiveTab.Value = Tabs.newlist.ToString();

            if(!InputFileIsValid(fuCodeList, lblAddListFeedback))
                return;

            var listName = tbListName.Text;
            var resourceId = tbResourceId.Text;
            var validFrom = DateTime.Parse(tbValidFrom.Text);
            var validTo = DateTime.Parse(tbValidTo.Text);

            try
            {
                var newCodes = new List<CodeEntity>();

                using (var inputStreamReader = new StreamReader(fuCodeList.PostedFile.InputStream, Encoding.Default))
                {
                    var csvReader = new CsvReader(inputStreamReader);
                    csvReader.Configuration.Encoding = Encoding.Default;
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    newCodes = csvReader.GetRecords<CodeEntity>().ToList();
                }

                var newCodeList = new CodeListEntity
                {
                    Name = listName,
                    ResourceId = resourceId,
                    ValidFrom = validFrom,
                    ValidTo = validTo
                };

                var codeListId = _codePortalService.AddCodeList(newCodeList, newCodes);

                var addedListInfo = GetAddedListInfo(codeListId);
                lblAddListFeedback.Text = addedListInfo;
                lblAddListFeedback.Visible = true;
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "Error when adding new list for code portal", LogLevel.Error, typeof(CodePortal));
                lblAddListFeedback.Text = "FEL: " + exception;
                lblAddListFeedback.Visible = true;
            }
        }

        protected void BtnImportToExistingList_Click(object sender, EventArgs e)
        {
            ActiveTab.Value = Tabs.managelists.ToString();

            if (!InputFileIsValid(FuCodeListImport, lblImportListFeedback))
                return;

            try
            {
                var codeListId = int.Parse(DdCodeLists.SelectedValue);
                var newCodes = new List<CodeEntity>();

                using (var inputStreamReader = new StreamReader(FuCodeListImport.PostedFile.InputStream, Encoding.Default))
                {
                    var csvReader = new CsvReader(inputStreamReader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    newCodes = csvReader.GetRecords<CodeEntity>().ToList();
                }

                var nrOfImportedCodes = _codePortalService.AddCodesToCodeList(codeListId, newCodes);

                var updatedListInfo = GetUpdatedListInfo(codeListId, nrOfImportedCodes, newCodes.Count);
                lblImportListFeedback.Text = updatedListInfo;
                lblImportListFeedback.Visible = true;
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "Error when importing codes to list for code portal", LogLevel.Error, typeof(CodePortal));
                lblImportListFeedback.Text = "FEL: " + exception;
                lblImportListFeedback.Visible = true;
            }
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var codeListId = int.Parse(DdCodeLists.SelectedValue);
                var limiter = TxtCsvLimiter.Text;

                var csv = new StringBuilder();

                // Add Csv headings
                csv.Append(
                    "Code" + limiter + 
                    "UsedTime" + limiter +
                    "UsedById" + limiter +
                    "UsedByEmail" + 
                    Environment.NewLine);

                var codes = _codePortalService.GetCodes(codeListId).ToList();

                // Add codes to Csv
                codes.ForEach(code => csv.Append(
                    code.Code + limiter +
                    code.UsedTime + limiter +
                    code.UsedById + limiter +
                    code.UsedByEmail + 
                    Environment.NewLine));

                HttpContextUtils.Stream(
                    "utf-8", 
                    Encoding.GetEncoding("windows-1250"), 
                    "application/ms-excel", 
                    DdCodeLists.SelectedItem.Text + ".csv",
                    csv.ToString());   
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "Error when exporting codes for list " + DdCodeLists.SelectedItem.Text, LogLevel.Error, typeof(CodePortal));
                lblImportListFeedback.Text = "FEL: " + exception;
                lblImportListFeedback.Visible = true;
            }
        }

        private string GetUpdatedListInfo(int listId, int importedCodes, int codesInCsv)
        {
            var info = new StringBuilder("Uppdaterad lista ");

            try
            {
                var list = _codePortalService.GetCodeList(listId);
                var codes = _codePortalService.GetCodes(listId).ToList();

                info.Append("<br />");
                info.AppendLine("Namn: " + list.Name);
                info.Append("<br />");
                info.AppendLine("Service+ resurs: " + list.ResourceId);
                info.Append("<br />");
                info.AppendLine("Giltig från: " + list.ValidFrom.ToString("yyy-MM-dd"));
                info.Append("<br />");
                info.AppendLine("Giltig till: " + list.ValidTo.ToString("yyy-MM-dd"));
                info.Append("<br />");
                info.AppendLine("Antal nya importerade koder: <strong>" + importedCodes + "</strong>/" + codesInCsv);
                info.Append("<br />");
                info.AppendLine("Totalt antal koder: " + codes.Count());        
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "GetUpdatedListInfo failed", LogLevel.Error, typeof(CodePortal));
            }

            return info.ToString();
        }

        private string GetAddedListInfo(int listId)
        {
            var info = new StringBuilder("Tillagd lista ");

            try
            {
                var list = _codePortalService.GetCodeList(listId);
                var codes = _codePortalService.GetCodes(listId).ToList();

                info.Append("<br />");
                info.AppendLine("Namn: " + list.Name);
                info.Append("<br />");
                info.AppendLine("Service+ resurs: " + list.ResourceId);
                info.Append("<br />");
                info.AppendLine("Giltig från: " + list.ValidFrom.ToString("yyy-MM-dd"));
                info.Append("<br />");
                info.AppendLine("Giltig till: " + list.ValidTo.ToString("yyy-MM-dd"));
                info.Append("<br />");
                info.AppendLine("Antal koder: " + codes.Count());                
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "GetAddedListInfo failed", LogLevel.Error, typeof(CodePortal));
            }

            return info.ToString();
        }

        private void DataBindControls()
        {
            if (IsPostBack) return;

            DdCodeLists.DataSource = _codePortalService.GetAllCodeLists();
            DdCodeLists.DataBind();

            DdCodeLists.Items.Insert(0, new ListItem("Välj lista", "0"));
        }

        private bool InputFileIsValid(FileUpload fileControl, Label feedBackLabel)
        {
            if (!fileControl.HasFile)
            {
                feedBackLabel.Text = "FEL: Ingen fil vald";
                feedBackLabel.Visible = true;
                return false;
            }

            if (!fileControl.PostedFile.FileName.ToLower().EndsWith(".csv"))
            {
                feedBackLabel.Text = "FEL: Felaktigt filformat, filen måste vara av formatet .csv";
                feedBackLabel.Visible = true;
                return false;
            }

            return true;
        }
    }

    public enum Tabs
    {
        // ReSharper disable InconsistentNaming
        managelists,
        newlist
    }
}