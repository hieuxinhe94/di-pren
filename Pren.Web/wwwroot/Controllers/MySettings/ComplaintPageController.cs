using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Di.Common.Logging;
using Di.Subscription.Logic.PublicationDays;
using Di.Subscription.Logic.Reclaim;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Mail;
using Pren.Web.Business.Messaging;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.MySettings
{
    // ReSharper disable Mvc.ViewNotResolved    
    public class ComplaintPageController : MySettingsControllerBase<ComplaintPage>
    {
        #region Fields
        
        private readonly IReclaimHandler _reclaimHandler;
        private readonly ILogger _logService;
        private readonly IPublicationDaysHandler _publicationDaysHandler;
        private readonly ISiteSettings _siteSettings;
        private readonly IMailHandler _mailHandler;

        #endregion

        #region Constr

        public ComplaintPageController(
            IReclaimHandler reclaimHandler,
            IPublicationDaysHandler publicationDaysHandler,
            ISessionData sessionData,
            ILogger logService,
            ISiteSettings siteSettings, 
            IMailHandler mailHandler) : base(sessionData)
        {
            _reclaimHandler = reclaimHandler;
            _publicationDaysHandler = publicationDaysHandler;
            _logService = logService;
            _siteSettings = siteSettings;
            _mailHandler = mailHandler;
        }

        #endregion

        #region Actions

        [AuthorizeUser(ValidateSubscriptionId = true)] 
        public ActionResult Index(ComplaintPage currentPage, string sid)
        {
            var model = new ComplaintPageViewModel(currentPage) { SubscriptionNumber = sid };

            if (!currentPage.ActivateComplaintsKayak) return View(model);

            var subscriber = GetSubscriberFromSession();
            var subscription = GetSubscriptionItem(subscriber, sid);

            var reclaimTypeFilterForUiDictionary = GetReclaimTypeFilterForUiDictionary();
            model.ReclaimItems = GetReclaimItems(reclaimTypeFilterForUiDictionary);
            
            model.DaysToReclaim = GetAvailableDaysToReclaim(subscription, 5);

            return View(model);
        }

        [HttpPost]
        [AuthorizeUser(ValidateSubscriptionId = true)] 
        [ValidateAntiForgeryToken]
        public ActionResult Send(ComplaintPage currentPage, string sid, DateTime fromdate, int numberofdays, string reason, DateTime[] daystoreclaim)
        {
            var subscriber = GetSubscriberFromSession();
            var subscription = GetSubscriptionItem(subscriber, sid); 

            try
            {
                if (currentPage.ActivateComplaintsKayak)
                {
                    CreateDeliveryReclaims(subscriber.SelectedSubscription.CustomerNumber, subscription.SubscriptionNumber, subscription.GenerationNumber, subscription.PaperCode, reason, daystoreclaim);
                }
                else
                {
                    _mailHandler.SendMail(_siteSettings.MailNoReplyAddress, _siteSettings.MailPrenAddress, "Reklamation", GetComplaintsMailText(numberofdays, subscriber.SelectedSubscription.CustomerNumber, subscriber.ServicePlusUser.Email, fromdate, reason), true);
                }

                TempData["Message"] = new Message("Din reklamation har skickats.");
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "ComplaintPageController.CreateDeliveryReclaim failed", LogLevel.Error, typeof(ComplaintPageController));
                TempData["Message"] = new Message("Just nu kan vi inte ta emot reklamationer. Vi beklagar detta.", MessageType.Danger);
            }

            return RedirectToAction("Index", new { sid });
        }

        #endregion

        #region Private methods

        private void CreateDeliveryReclaims(long customerNumber, long subscriptionNumber, int extNo, string paperCode, string reasonId, IEnumerable<DateTime> daysToReclaim)
        {
            foreach (var date in daysToReclaim)
            {
                try
                {
                    _reclaimHandler.CreateDeliveryReclaim(customerNumber, subscriptionNumber, extNo, paperCode, reasonId, date);
                }
                catch (Exception exception)
                {
                    _logService.Log(exception, string.Format("ComplaintPageController.CreateDeliveryReclaims Failed Params: customerNumber: {0},  subscriptionNumber: {1}, extNo: {2}, paperCode: {3}, reasonId: {4}, date: {5}", customerNumber, subscriptionNumber, extNo, paperCode, reasonId, date), LogLevel.Error, typeof(ComplaintPageController));
                }                
            }
        }

        private string GetComplaintsMailText(int numDays, long cusNo, string email, DateTime fromdate, string reason)
        {
            var sb = new StringBuilder();
            sb.Append("Reklamation via \"mina sidor\".<br><br>");
            sb.Append("Kundnummer: " + cusNo + "<br>");
            sb.Append("E-post: " + email + "<br>");
            sb.Append("Från datum: " + fromdate.ToString("yyyy-MM-dd") + "<br>");
            sb.Append("Antal dagar: " + numDays + "<br>");
            sb.Append("Orsak: " + reason);
            return sb.ToString();
        }

        private IEnumerable<DateTime> GetAvailableDaysToReclaim(SubscriptionItem subscription, int numberOfDaysBackFromToday)
        {
            var availableDates = new List<DateTime>();            
            var endDate = DateTime.Now;
            // You get publication days between two dates and for the startdate we go back 30 days to be sure we get some days.
            // If the startdate of the subscription is later that our calculated startdate we use that instead so user cannot reclaim days before the subscription was started
            var startDate = endDate.AddDays(-30);
            if (subscription.StartDate > startDate)
            {
                startDate = subscription.StartDate;
            }
            var issueDates = _publicationDaysHandler.GetPublicationDays(subscription.ProductNumber, startDate, endDate);
            availableDates.AddRange(issueDates.OrderByDescending(d => d.IssueDate).Take(numberOfDaysBackFromToday).Select(d => d.IssueDate));
            return availableDates.OrderBy(d => d.Date);
        }

        private IEnumerable<ReclaimItem> GetReclaimItems(IReadOnlyDictionary<int, string> filteredItemsDictionary)
        {
            try
            {
                var reclaimTypes = _reclaimHandler.GetReclaimTypes();

                return reclaimTypes
                    .Where(reclaimType => filteredItemsDictionary.ContainsKey(reclaimType.Id))
                    .Select(reclaimType => new ReclaimItem(filteredItemsDictionary[reclaimType.Id], reclaimType.Id.ToString()));
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "ComplaintPageController.GetReclaimItem Failed", LogLevel.Error, typeof(ComplaintPageController));
                TempData["Message"] = new Message("Just nu kan vi inte ta emot reklamationer. Vi beklagar detta.", MessageType.Danger);
                return null;
            }
        }

        /// <summary>
        /// Gets a dictionary containing the id of all reclaimItems that should be visible in the UI, with related texts.
        /// </summary>
        /// <returns>A dictionary with reclaim item keys and text.</returns>
        public Dictionary<int, string> GetReclaimTypeFilterForUiDictionary()
        {
            return new Dictionary<int, string>
            {
                {890, "Utebliven tidning"},
                {901, "Sent levererad tidning"},
                {896, "Fel tidning levererad"},
                {914, "Bilaga saknas"},
                {911, "Blöt tidning"},
                {912, "Trasig tidning"}
            };
        }

        #endregion
    }


}