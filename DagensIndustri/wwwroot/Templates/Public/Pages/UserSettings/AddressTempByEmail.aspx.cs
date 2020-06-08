using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using DagensIndustri.Classes;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class AddressTempByEmail : DiTemplatePage, IUserSettingsPage
    {
        private const string FileCacheKey = "AddressTempFileContent";

        private List<CustomerFileData> ValidCustomerList { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!BuildValidCustomerList())
            {
                return;
            }

            if (!Page.IsPostBack)
            {
                return;
            }

            var validCustomerInfo = ValidCustomerList.FirstOrDefault(c => c.CustomerId.ToString() == InputCusno.Text);
            if (validCustomerInfo == null)
            {
                ShowError("AddressTempByEmail", string.Format("Customer {0} entered cusno {1} that is not valid in file", InputEmail.Text, InputCusno.Text));
                return;
            }
            
            //Cusno is validated, save email to Cirix, build url and redirect user to http://dagensindustri.se/mysettings/addresstemp?code=##PrenumerantID##&sid=##Prenumerationsnr##
            ProcessCirixCustomer(validCustomerInfo);
        }

        private string ReadCachedFileContent(string filePath)
        {
            var data = HttpRuntime.Cache.Get(FileCacheKey);
            if (data != null)
            {
                return (string)data;
            }
            var fileContent = File.ReadAllText(filePath);
            HttpRuntime.Cache.Insert(FileCacheKey, fileContent, new CacheDependency(filePath));
            return fileContent;
        }

        private bool BuildValidCustomerList()
        {
            var filePath = ConfigurationManager.AppSettings["TemporaryAddressCustomerFilePath"];
            if (string.IsNullOrEmpty(filePath))
            {
                ShowError("AddressTempByEmail failed", "Could not load filepath from appsettings TemporaryAddressCustomerFilePath");
                return false;
            }

            var fileContent = ReadCachedFileContent(filePath);
            if (string.IsNullOrEmpty(fileContent))
            {
                ShowError("AddressTempByEmail failed", "Method BuildValidCustomerList no content in file");
                return false;
            }
            var customerList = fileContent.Split(';');
            ValidCustomerList = new List<CustomerFileData>();
            try
            {
                foreach (var data in customerList)
                {
                    var customerData = data.Split(',');
                    var cust = new CustomerFileData();
                    cust.CustomerId = TryGetIntFromArr(customerData, 0);
                    cust.SubsNo = TryGetIntFromArr(customerData, 1);
                    
                    if (cust.CustomerId > 0)
                    {
                        ValidCustomerList.Add(cust);
                    }
                }
            }
            catch (Exception ex)
            {
                ValidCustomerList.Clear();
                ShowError("AddressTempByEmail failed", string.Format("Parse file error. {0}", ex.Message));
            }

            return ValidCustomerList.Any();
        }

        private int TryGetIntFromArr(string[] arr, int arrPosition)
        {
            try
            {
                return int.Parse(arr[arrPosition].Trim());
            }
            catch
            {
                return 0;
            }
        }

        private void ShowError(string logDescription, string errorLogMessage)
        {
            if (!string.IsNullOrEmpty(logDescription) && !string.IsNullOrEmpty(errorLogMessage))
            {
                new Logger(logDescription, errorLogMessage);
            }
            PlaceHolderForm.Visible = false;
            phError.Visible = true;
            litErrorMessage.Text = Translate("/mysettings/subscription/temporaryaddress/errormessage");
        }

        private void ProcessCirixCustomer(CustomerFileData fileData)
        {
            var customerCode = MsSqlHandler.MdbGetCodeByCusno(fileData.CustomerId);
            if (customerCode == Guid.Empty)
            {
                ShowError("AddressTempByEmail.ProcessCirixCustomer failed", string.Format("MdbGetCodeByCusno() returned empty Guid for cusno: {0}", fileData.CustomerId));
                return;
            }

            var identifiedSubscriber = new SubscriptionUser2(fileData.CustomerId);
            if (string.IsNullOrEmpty(identifiedSubscriber.Email) || InputEmail.Text.ToLower() != identifiedSubscriber.Email.ToLower())
            {
                // Update email in Cirix for customer if it is changed
                identifiedSubscriber.UpdateEmail(InputEmail.Text.ToLower());
            }

            // Update customer table and Apsis
            UpdateApsis(fileData.CustomerId);

            if (!IsValue("MySetAddressTempPage"))
            {
                ShowError("AddressTempByEmail.ProcessCirixCustomer failed", "Page property MySetAddressTempPage is not set");
                return;
            }

            var pd = GetPage(CurrentPage["MySetAddressTempPage"] as PageReference);
            
            var url = new StringBuilder();
            url.Append(EPiFunctions.GetFriendlyAbsoluteUrl(pd));
            url.Append("?code=" + customerCode);
            if (fileData.SubsNo > 0)
                url.Append("&sid=" + fileData.SubsNo);

            DoRedirect(url.ToString());
        }

        private void UpdateApsis(int cusno)
        {
            var dhandler = new MailSenderDbHandler();
            dhandler.UpdateEmailInMailSenderDb(cusno, InputEmail.Text.ToLower(), false, false);
            dhandler.MarkCustomerForApsisUpdateJob(new List<int>() { cusno });
        }

        private void DoRedirect(string url)
        {
            Response.Status = "301 Moved Permanently";
            Response.AddHeader("Location", url);
            Response.End();
        }

        public void ShowMessageFromChildCtrl(string mess, bool isKey, bool isError)
        {
            ShowMessage(mess, isKey, isError);
        }
    }
}