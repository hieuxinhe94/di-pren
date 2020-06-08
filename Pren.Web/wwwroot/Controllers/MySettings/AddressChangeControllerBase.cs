using System;
using System.Collections.Generic;
using System.Text;
using Di.Subscription.Logic.Address.Types;
using Di.Subscription.Logic.IssueDate;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Mail;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.CustomForms.MySettings;
using Pren.Web.Models.Pages;

namespace Pren.Web.Controllers.MySettings
{
    [NoCache]
    public abstract class AddressChangeControllerBase<T> : MySettingsControllerBase<T> where T : SitePageData
    {
        protected const string NotValidFormErrorMessage = "Formuläret är inte korrekt ifyllt. Var god försök igen.";

        private readonly IDetectionHandler _detectionHandler;
        private readonly IMailHandler _mailHandler;
        private readonly ISiteSettings _siteSettings;
        private readonly IIssueDateHandler _issueDateHandler;

        protected AddressChangeControllerBase(
            ISessionData sessionData, 
            IDetectionHandler detectionHandler,
            IMailHandler mailHandler,
            ISiteSettings siteSettings, 
            IIssueDateHandler issueDateHandler) 
            : base(sessionData)
        {
            _detectionHandler = detectionHandler;
            _mailHandler = mailHandler;
            _siteSettings = siteSettings;
            _issueDateHandler = issueDateHandler;
        }

        protected void SetFieldsFromAddressMap(AddressChange address, AddressFormModel addressForm)
        {
            addressForm.StreetAddressForm.Co = GetCareOf(address.Street2);
            addressForm.StreetAddressForm.StreetAddress = address.StreetAddress;
            addressForm.StreetAddressForm.StreetNo = address.StreetNumber;
            addressForm.StreetAddressForm.StairCase = address.StairCase;
            addressForm.StreetAddressForm.Stairs = (string.IsNullOrEmpty(address.Apartment)) ? string.Empty : address.Apartment.Replace("TR", "").Trim();
            addressForm.StreetAddressForm.ApartmentNumber = GetApartmentNo(address.Street2);
            addressForm.StreetAddressForm.Zip = address.Zip;
            addressForm.StreetAddressForm.City = address.City;
            addressForm.StreetAddressForm.FromDate = address.StartDate;
            addressForm.StreetAddressForm.ToDate = DateTime.MinValue;

            if (address.EndDate > DateTime.MinValue && address.EndDate != new DateTime(2078, 12, 31))
            {
                addressForm.StreetAddressForm.ToDate = address.EndDate;
            }
        }

        protected string GetErrMess()
        {
            var err = new StringBuilder();
            err.Append("Ett tekniskt fel uppstod.<br>");
            err.Append("Ring gärna kundtjänst på 08-573 651 00 så hjälper vi till att lösa problemet.");
            return err.ToString();
        }

        protected void SendCustMail(StreetAddressFormModel addressForm, Subscriber subscriber, string changeTypeString, string mailBody)
        {
            if (!_detectionHandler.IsValidEmail(subscriber.ServicePlusUser.Email))
                return;

            var replaceDictionary = new Dictionary<string, string>
            {
                {"[fromDate]", addressForm.FromDate.ToString("yyyy-MM-dd")},
                {"[toDate]", addressForm.ToDate.ToString("yyyy-MM-dd")},
                {"[address]", GetAddressAsHtml(subscriber, addressForm)}
            };

            mailBody = _mailHandler.ReplaceMailPlaceHolders(replaceDictionary, mailBody);

            _mailHandler.SendMail(
                _siteSettings.MailNoReplyAddress,
                subscriber.ServicePlusUser.Email,
                "Bekräftelse " + changeTypeString + " adressändring",
                mailBody,
                true);
        }

        protected DateTime GetClosestIssueDate(IEnumerable<SubscriptionItem> subsActive, DateTime dt)
        {
            var earliestDate = DateTime.MinValue;
            foreach (var subscription in subsActive)
            {
                var nextIssueDate = _issueDateHandler.Retriever.GetNextIssuedate(subscription.PaperCode, subscription.ProductNumber, dt);
                if (nextIssueDate > earliestDate)
                {
                    earliestDate = nextIssueDate;
                }
            }

            return earliestDate;
        }

        private string GetAddressAsHtml(Subscriber subscriber, StreetAddressFormModel addressForm)
        {
            var sb = new StringBuilder();

            //tidigare skrevs rowtext2 ut efter rowtext1 men nu ligger den logiken i CustomerRetriever och ser lite annorlunda ut, bra att veta om vi får support på detta 
            sb.Append(subscriber.SelectedSubscription.KayakCustomer.FullName);

            sb.Append("<br>");

            if (!string.IsNullOrEmpty(addressForm.Co))
                sb.Append("C/O " + addressForm.Co.ToUpper() + "<br>");

            sb.Append(addressForm.StreetAddress.ToUpper());

            if (!string.IsNullOrEmpty(addressForm.StreetNo))
                sb.Append(" " + addressForm.StreetNo);

            if (!string.IsNullOrEmpty(addressForm.StairCase))
                sb.Append(" " + addressForm.StairCase.ToUpper());

            if (!string.IsNullOrEmpty(addressForm.Stairs))
                sb.Append(" " + addressForm.Stairs + "TR");

            if (!string.IsNullOrEmpty(addressForm.ApartmentNumber))
                sb.Append(" LGH" + addressForm.ApartmentNumber);

            sb.Append("<br>");

            sb.Append(addressForm.Zip + " " + addressForm.City.ToUpper());

            return sb.ToString();
        }

        private string GetCareOf(string street2)
        {
            if (string.IsNullOrEmpty(street2))
                return string.Empty;

            var co = street2;
            var lghIndex = street2.IndexOf("LGH", StringComparison.Ordinal);
            if (lghIndex > -1)
                co = street2.Substring(0, lghIndex).Trim();

            return co;
        }

        private string GetApartmentNo(string street2)
        {
            if (string.IsNullOrEmpty(street2))
                return string.Empty;

            var appNo = "";
            var lghIndex = street2.IndexOf("LGH", StringComparison.Ordinal);
            if (lghIndex > -1)
                appNo = street2.Substring(lghIndex + 3).Trim();

            return appNo;
        }
    }
}
