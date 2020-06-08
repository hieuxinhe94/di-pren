using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Di.Common.Logging;
using Di.Subscription.Logic.HolidayStop;
using Di.Subscription.Logic.HolidayStop.Types;
using Di.Subscription.Logic.IssueDate;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Detection;
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
    public class SubscriptionSleepPageController : MySettingsControllerBase<SubscriptionSleepPage>
    {

        #region Fields

        private readonly ILogger _logService;
        private readonly ISiteSettings _siteSettings;
        private readonly IMailHandler _mailHandler;
        private readonly IDetectionHandler _detectionHandler;
        private readonly IHolidayStopHandler _holidayStopHandler;
        private readonly IIssueDateHandler _issueDateHandler;

        #endregion

        #region Constr

        public SubscriptionSleepPageController(
            ILogger logService,
            ISiteSettings siteSettings,
            ISessionData sessionData, 
            IMailHandler mailHandler,
            IDetectionHandler detectionHandler,
            IHolidayStopHandler holidayStopHandler,
            IIssueDateHandler issueDateHandler) : base(sessionData)
        {
            _logService = logService;
            _issueDateHandler = issueDateHandler;
            _siteSettings = siteSettings;
            _mailHandler = mailHandler;
            _detectionHandler = detectionHandler;
            _holidayStopHandler = holidayStopHandler;
        }

        #endregion

        #region Actions

        [AuthorizeUser(ValidateSubscriptionId = true)]
        public ActionResult Index(SubscriptionSleepPage currentPage, string sid)
        {
            var subscriber = GetSubscriberFromSession();
            var subscriptionItem = GetSubscriptionItem(subscriber, sid);

            var model = new SubscriptionSleepPageViewModel(currentPage)
            {
                SubscriptionSleepForm =
                {
                    DateMinAddrChange = _issueDateHandler.Retriever.GetNextIssuedate(subscriptionItem.PaperCode, subscriptionItem.ProductNumber, DateTime.Now),
                    DateMaxAddrChange = subscriptionItem.EndDate
                },
                SubscriptionNumber = subscriptionItem.SubscriptionNumber,
                FutureSubsSleeps = GetFutureSubsSleeps(subscriptionItem),
                DateNotSet = new DateTime(1800, 1, 1)
            };

            return View(model);
        }                       

        [HttpPost]
        [AuthorizeUser(ValidateSubscriptionId = true)]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult Save(SubscriptionSleepPage currentPage, SubscriptionSleepFormModel postedForm, string sid)
        {
            var subscriber = GetSubscriberFromSession();
            var subscriptionItem = GetSubscriptionItem(subscriber, sid);

            SetUpDates(subscriptionItem, postedForm);

            var message = ValidateInput(subscriptionItem, postedForm);

            message = string.IsNullOrEmpty(message) ?
                CreateSubscriptionSleep(currentPage, subscriber.SelectedSubscription, subscriptionItem, postedForm) : 
                message;            

            TempData["Message"] = string.IsNullOrEmpty(message) ? new Message("Ditt uppehåll har sparats.") : new Message(message, MessageType.Danger);
            return RedirectToAction("Index", new { sid });
        }

        [AuthorizeUser(ValidateSubscriptionId = true)]
        public ActionResult Delete(string sid, DateTime startDate, DateTime endDate)
        {
            var subscriber = GetSubscriberFromSession();
            var subscriptionItem = GetSubscriptionItem(subscriber, sid);

            var message = DeleteSubscriptionSleep(subscriptionItem, startDate, endDate);

            TempData["Message"] = string.IsNullOrEmpty(message) ? new Message("Ditt uppehåll har tagits bort.") : new Message(message, MessageType.Danger);

            return RedirectToAction("Index", new { sid });
        }

        [HttpPost]
        [AuthorizeUser(ValidateSubscriptionId = true)]
        public ActionResult Edit(string sid, SubscriptionSleepFormModel postedForm)
        {
            var subscriber = GetSubscriberFromSession();
            var subscriptionItem = GetSubscriptionItem(subscriber, sid);

            SetUpDates(subscriptionItem, postedForm, postedForm.Ongoing);

            var message = ValidateInput(subscriptionItem, postedForm);

            message = string.IsNullOrEmpty(message) ?
                ChangeSubscriptionSleep(subscriptionItem, postedForm) :
                message;

            TempData["Message"] = string.IsNullOrEmpty(message) ? new Message("Ditt uppehåll har uppdaterats.") : new Message(message, MessageType.Danger);

            return RedirectToAction("Index", new { sid });
        }

        #endregion

        private void SetUpDates(SubscriptionItem subscriptionItem, SubscriptionSleepFormModel postedForm, bool ongoing = false)
        {
            // If sleep is ongoing, fromdate is disabled so use original fromdate
            postedForm.FromDate = ongoing ? postedForm.StartDateOrg : postedForm.FromDate;

            // Do not change fromdate if ongoing sleep
            if (!ongoing && postedForm.FromDate > postedForm.DateMinAddrChange)
            {
                postedForm.FromDate = _issueDateHandler.Retriever.GetNextIssuedate(subscriptionItem.PaperCode, subscriptionItem.ProductNumber, postedForm.FromDate);
            }

            if (postedForm.ToDate != DateTime.MinValue)
            {
                var tmpEnd = _issueDateHandler.Retriever.GetNextIssuedate(subscriptionItem.PaperCode, subscriptionItem.ProductNumber, postedForm.ToDate);
                if (tmpEnd.Date > postedForm.ToDate.Date)
                {
                    postedForm.ToDate = tmpEnd.AddDays(-1);
                }
            }

            if (postedForm.ToDate == DateTime.MinValue)
                postedForm.ToDate = new DateTime(1800, 1, 1); // Default min in Kayakkk
        }

        private string ValidateInput(SubscriptionItem subscriptionItem, SubscriptionSleepFormModel postedForm)
        {
            // Do not check startdate if editing an ongoing sleep
            if (string.IsNullOrEmpty(postedForm.SubscriptionSleepId) && postedForm.FromDate < postedForm.DateMinAddrChange)
            {
                return "Tidigaste möjliga fråndatum är " + postedForm.DateMinAddrChange.ToString("yyyy-MM-dd") + ".<br>Var god välj ett senare datum.";
            }

            if (postedForm.ToDate != DateTime.MinValue)
            {

                if (postedForm.FromDate >= postedForm.ToDate)
                {
                    return "Fråndatum måste vara ett tidigare datum än tilldatum.<br>Var god försök igen.";
                }

                //max 8 weeks subsSleep
                var difference = postedForm.ToDate - postedForm.FromDate;
                if (difference.Days > (8 * 7))
                {
                    return "Uppehållet får max vara 8 veckor långt.<br>Var god försök igen.";
                }
            }

            if (!IsDateSpanValid(GetFutureSubsSleeps(subscriptionItem), postedForm))
            {
                return "Datumintervallet kolliderar med tidigare sparat uppehåll.<br>Var god försök igen.";
            }

            return string.Empty;
        }

        private bool IsDateSpanValid(IEnumerable<HolidayStopItem> futureSubsSleeps, SubscriptionSleepFormModel postedForm)
        {
            foreach (var futureSubSleep in futureSubsSleeps.Where(futureSubSleep =>
                string.IsNullOrEmpty(postedForm.SubscriptionSleepId) || futureSubSleep.Id != postedForm.SubscriptionSleepId))
            {
                //dateStart in old interval
                if (postedForm.FromDate >= futureSubSleep.StartDate && postedForm.FromDate <= futureSubSleep.EndDate)
                    return false;

                //dateEnd in old interval
                if (postedForm.ToDate >= futureSubSleep.StartDate && postedForm.ToDate <= futureSubSleep.EndDate)
                    return false;

                //overlapping entire old interval
                if (postedForm.FromDate < futureSubSleep.StartDate && postedForm.ToDate > futureSubSleep.EndDate)
                    return false;
            }

            return true;
        }

        private string ChangeSubscriptionSleep(SubscriptionItem subscriptionItem, SubscriptionSleepFormModel postedForm)
        {
            try
            {
                _holidayStopHandler.ChangeHolidayStop(
                    subscriptionItem.SubscriptionNumber,
                    subscriptionItem.GenerationNumber,
                    postedForm.StartDateOrg,
                    postedForm.FromDate,
                    postedForm.EndDateOrg,
                    postedForm.ToDate);
            }
            catch (Exception exception)
            {
                _logService.Log(exception,
                    string.Format(
                        "ChangeHolidayStop - exception, UrlSubsno:{0}, DateStart:{1}, DateEnd:{2}", subscriptionItem.SubscriptionNumber, postedForm.FromDate, postedForm.ToDate), LogLevel.Error, typeof(SubscriptionSleepPageController));

                return "Ett tekniskt fel uppstod.<br>Vänligen försök igen."; 
            }

            return string.Empty;
        }

        private string DeleteSubscriptionSleep(SubscriptionItem subscriptionItem, DateTime startDate, DateTime endDate)
        {
            try
            {
                _holidayStopHandler.DeleteHolidayStop(
                    subscriptionItem.SubscriptionNumber,
                    subscriptionItem.GenerationNumber,
                    startDate);                   
            }
            catch (Exception exception)
            {
                _logService.Log(exception,
                    string.Format(
                        "DeleteHolidayStop - exception, UrlSubsno:{0}, DateStart:{1}, DateEnd:{2}", subscriptionItem.SubscriptionNumber, startDate, endDate), LogLevel.Error, typeof(SubscriptionSleepPageController));

                return "Ett tekniskt fel uppstod.<br>Vänligen försök igen."; 
            }

            return string.Empty;
        }

        private string CreateSubscriptionSleep(SubscriptionSleepPage currentPage, UserSubscription subscription, SubscriptionItem subscriptionItem, SubscriptionSleepFormModel postedForm)
        {
            const string errorMessage = "Ett tekniskt fel uppstod.<br>Vänligen försök igen.";
          
            try
            {
                var result = _holidayStopHandler.CreateHolidayStop(
                    postedForm.FromDate,
                    postedForm.ToDate,
                    subscriptionItem.SubscriptionNumber
                    );

                if (result == "OK")
                {
                    _logService.Log("Subscription sleep created for customer " + subscription.CustomerNumber, LogLevel.Info, typeof(SubscriptionSleepPageController));
                    if (currentPage.MailConfirmText != null)
                    {
                        SendCustMail(subscription.KayakCustomer.Email, postedForm.FromDate, postedForm.ToDate, currentPage.MailConfirmText.ToHtmlString());
                    }
                }
                else
                {
                    _logService.Log(string.Format(
                            "CreateHolidayStop - failed. Cusno:{0}, UrlSubsno:{1}, DateStart:{2}, DateEnd:{3}, allowWebPaper:{4}, creditType:{5} - ",
                            subscription.CustomerNumber, subscriptionItem.SubscriptionNumber, postedForm.FromDate, postedForm.ToDate, "Y", "03") + result, LogLevel.Error, typeof(SubscriptionSleepPageController));

                    return errorMessage;
                }
            }
            catch (Exception exception)
            {
                _logService.Log(exception,
                    string.Format(
                        "CreateHolidayStop - exception. Cusno:{0}, UrlSubsno:{1}, DateStart:{2}, DateEnd:{3}, allowWebPaper:{4}, creditType:{5}",
                        subscription.CustomerNumber, subscriptionItem.SubscriptionNumber, postedForm.FromDate, postedForm.ToDate, "Y", "03"), LogLevel.Error, typeof(SubscriptionSleepPageController));

                return errorMessage;
            }

            return string.Empty;
        }

        private List<HolidayStopItem> GetFutureSubsSleeps(SubscriptionItem subscriptionItem)
        {
            var sleepingSubscriptions = _holidayStopHandler.GetHolidayStops(subscriptionItem.SubscriptionNumber, subscriptionItem.GenerationNumber)
                .Where(t => t.EndDate >= DateTime.Now)
                .OrderBy(t => t.StartDate);
            return sleepingSubscriptions.ToList();
        }

        private void SendCustMail(string emailAddress, DateTime startDate, DateTime endDate, string mailBody)
        {
            if (!_detectionHandler.IsValidEmail(emailAddress))
                return;

            var replaceDictionary = new Dictionary<string, string>
            {
                {"[fromDate]", startDate.ToString("yyyy-MM-dd")},
                {"[toDate]", endDate.ToString("yyyy-MM-dd")}
            };

            mailBody = _mailHandler.ReplaceMailPlaceHolders(replaceDictionary, mailBody);

            _mailHandler.SendMail(
                _siteSettings.MailNoReplyAddress,
                emailAddress, 
                "Bekräftelse uppehåll", 
                mailBody, 
                true);
        }
    }
}