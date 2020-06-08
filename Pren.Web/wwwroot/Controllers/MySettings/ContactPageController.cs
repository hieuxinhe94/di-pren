using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Di.Common.Utils.Context;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Mail;
using Pren.Web.Business.Messaging;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.CustomForms.MySettings;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.MySettings
{
    // ReSharper disable Mvc.ViewNotResolved    
    public class ContactPageController : MySettingsControllerBase<ContactPage>
    {
        private readonly IMailHandler _mailHandler;
        private readonly ISiteSettings _siteSettings;
        
        public ContactPageController(
            ISessionData sessionData, 
            IMailHandler mailHandler, 
            ISiteSettings siteSettings) 
            : base(sessionData)
        {
            _mailHandler = mailHandler;
            _siteSettings = siteSettings;
        }

        [AuthorizeCheck]
        public ActionResult Index(ContactPage currentPage)
        {
            var model = new ContactPageViewModel(currentPage);                
            var subscriber = GetSubscriberFromSession();

            PopulateModel(model, subscriber);

            model.HideForm = !ShowForm(currentPage);
            
            ClearInvalidSession(subscriber);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(ContactPage currentPage, ContactPageFormModel postedForm)
        {
            var fileName = postedForm.Attachment != null ? Path.GetFileName(postedForm.Attachment.FileName) : null;
            var attachment = postedForm.Attachment != null ? postedForm.Attachment.InputStream : null;
            
            _mailHandler.SendMail(
                postedForm.Email,
                _siteSettings.MailPrenAddress,
                "Kontakta oss: " + postedForm.Category,
                GetMailBody(postedForm),
                true,
                fileName,
                attachment
                );

            TempData["Message"] = new Message(currentPage.ThankYouBody != null ? currentPage.ThankYouBody.ToHtmlString() : string.Empty);

            return RedirectToAction("index");
        }

        private string GetMailBody(ContactPageFormModel postedForm)
        {
            var body = new StringBuilder();

            body.Append("<strong>Namn</strong>: " + postedForm.Name + "<br />");
            body.Append("<strong>Telefonnummer</strong>: " + postedForm.Phone + "<br />");
            body.Append("<strong>E-post</strong>: " + postedForm.Email + "<br />");
            body.Append("<strong>Kundnummer</strong>: " + postedForm.CustomerNumber + "<br />");
            body.Append("<strong>Kategori</strong>: " + postedForm.Category + "<br />");
            body.Append("<strong>Ärende</strong>: " + postedForm.Message + "<br /><br />");

            body.Append("<hr><strong>UserAgent</strong>: " + HttpContextUtils.GetUserAgent() + "<br />");
            body.Append("<strong>Webbläsare</strong>: " + HttpContextUtils.GetUserBrowser() + "<br />");
            body.Append("<strong>Webbläsarversion</strong>: " + HttpContextUtils.GetUserBrowserVersion() + "<br />");
            body.Append("<strong>Ip</strong>: " + HttpContextUtils.GetUserIp() + "<br />");

            return body.ToString();
        }

        private bool ShowForm(ContactPage currentPage)
        {
            var isWeekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(DateTime.Now.DayOfWeek);

            // Never hide form on weekends
            if (isWeekend) return false;

            var startTime = new TimeSpan(currentPage.FormShowFrom, 0, 0);
            var endTime = new TimeSpan(currentPage.FormShowTo, 0, 0);
            var now = DateTime.Now.TimeOfDay;

            return currentPage.ShowForm || (now > startTime) && (now < endTime);
        }

        private void PopulateModel(ContactPageViewModel model, Subscriber subscriber)
        {
            // If user is logged in, populate fields
            if (subscriber != null && subscriber.SelectedSubscription != null && subscriber.SelectedSubscription.KayakCustomer != null)
            {
                var user = subscriber.SelectedSubscription.KayakCustomer;
                model.Name = user.FullName;
                model.Phone = !string.IsNullOrEmpty(user.PhoneOffice) ? user.PhoneOffice : user.PhoneWork;
                model.Email = user.Email;
                model.CustomerNumber = user.CustomerNumber;
            }
        }

    }
}