using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.Partials.MySettings;
using SubscriptionType = Pren.Web.Business.Subscription.SubscriptionType;

namespace Pren.Web.Controllers.MySettings 
{
    // ReSharper disable Mvc.PartialViewNotResolved
    public class MySettingsPageMenuController : Controller
    {

        #region Fields

        private readonly IContentRepository _contentRepository;
        private readonly ISubscriberFacade _subscriberFacade;
        private readonly ISiteSettings _siteSettings;

        private IContent _temporaryAddressChangePage;
        private IContent _subscriptionSleepPage;
        private IContent _reclaimPage;


        #endregion

        #region Constuctors

        public MySettingsPageMenuController(
            IContentRepository contentRepository, ISubscriberFacade subscriberFacade, ISiteSettings siteSettings)
    {
            _contentRepository = contentRepository;
            _subscriberFacade = subscriberFacade;
        _siteSettings = siteSettings;
    }

        #endregion

        #region Actions

        public ActionResult Index(ContentReference startPageReference, ContentReference currentPageReference)
        {
            var model = new MySettingsPageMenu
            {
                CurrentPageReference = currentPageReference,
                ChangePasswordUrl = _siteSettings.ServicePlusChangePasswordUrl
            };

            var subscriber = _subscriberFacade.GetSubscriberFromSession();
            PopulateCustomerFacts(subscriber, model);  

            if (subscriber == null || subscriber.SelectedSubscription == null)
            {                              
                return PartialView(model);
            }

            var startPage = _contentRepository.Get<StartPage>(startPageReference);
            var menuPages = GetChildren(startPage.MySettingsStartPage, false).ToList();

            

            SetSubscriptionMenuPages(menuPages);

            menuPages = GetFilteredMenuPages(menuPages, subscriber.SelectedSubscription).ToList();

            model.MenuPages = menuPages.Where(p => ((PageData)p).VisibleInMenu);

            if (subscriber.SelectedSubscription.SubscriptionItems == null ||
                !subscriber.SelectedSubscription.SubscriptionItems.Any())
            {
                return PartialView(model);
            }

            model.SubscriptionMenuItems = GetSubscriptionMenuItems(subscriber.SelectedSubscription.SubscriptionItems);

            //PopulateCustomerFacts(subscriber, model);

            return PartialView(model);
        }

        #endregion

        #region Private methods

        private IEnumerable<IContent> GetChildren(ContentReference mySettingsStartPage, bool includeStartPage = true)
        {
            var menuPages = _contentRepository.GetChildren<SitePageData>(mySettingsStartPage).ToList();

            if (includeStartPage)
            {
                menuPages.Insert(0, _contentRepository.Get<SitePageData>(mySettingsStartPage));   
            }            

            return FilterForVisitor.Filter(menuPages);
        }

        private IEnumerable<IContent> GetFilteredMenuPages(IEnumerable<IContent> menuPages, UserSubscription subscription)
        {

            // Pages to be listed under each subscription and not belongs to the menu
            var ignoreTypes = new[] { typeof(TemporaryAddressChangePage), typeof(SubscriptionSleepPage), typeof(AutowithdrawalPage), typeof(ComplaintPage) };

            var businessSubscriptionIgnoreTypes = new[] { typeof(PermanentAddressChangePage), typeof(PersonInfoPage), typeof(CodePortalPage) };
            // Remove subscription specific menu pages
            menuPages = menuPages.Where(menuPage => !ignoreTypes.Contains(menuPage.GetOriginalType()));

            // Filter menu pages by type
            switch (subscription.Type)
            {
                case SubscriptionType.Private: 
                    // All pages except business subscription admin page
                    return menuPages.Where(c => c.GetOriginalType() != typeof(BusinessSubscriptionPage)).ToList();
                case SubscriptionType.Business:
                    // Subscription not yet processed down to Kayak - only show business subscription admin page
                    if (subscription.CustomerNumber == 0) 
                    {
                        return menuPages.Where(c => c.GetOriginalType() == typeof (BusinessSubscriptionPage)).ToList();
                    }
                    // All pages except those defined in businessSubscriptionIgnoreTypes
                    return menuPages.Where(c => !businessSubscriptionIgnoreTypes.Contains(c.GetOriginalType())); 
                default:
                    return menuPages;
            }
        } 

        private void SetSubscriptionMenuPages(IList<IContent> pages)
        {
            _temporaryAddressChangePage = pages.FirstOrDefault(page => page is TemporaryAddressChangePage);
            _subscriptionSleepPage = pages.FirstOrDefault(page => page is SubscriptionSleepPage);
            _reclaimPage = pages.FirstOrDefault(page => page is ComplaintPage);
        }

        private List<SubscriptionMenuItem> GetSubscriptionMenuItems(IEnumerable<SubscriptionItem> subscriptionItems)
        {
            var menu = new List<SubscriptionMenuItem>();

            foreach (var subscriptionItem in subscriptionItems)
            {
                var subscriptionMenuItem = new SubscriptionMenuItem { Pages = new List<IContent>(), SubscriptionItem = subscriptionItem};

                if (!subscriptionItem.IsDigitalSubscription)
                {
                    if (_subscriptionSleepPage != null) // && (subscription.Autogiro != "Y") //todo: kj autogirokollen?
                    {
                        subscriptionMenuItem.Pages.Add(_subscriptionSleepPage);
                    }

                    if (_reclaimPage != null && subscriptionItem.StartDate <= DateTime.Now)
                    {
                        subscriptionMenuItem.Pages.Add(_reclaimPage);
                    }

                    if (_temporaryAddressChangePage != null)
                    {
                        subscriptionMenuItem.Pages.Add(_temporaryAddressChangePage);
                    }
                }

                subscriptionMenuItem.StartDate = subscriptionItem.StartDate.ToShortDateString();
                subscriptionMenuItem.EndDate = subscriptionItem.EndDate > DateTime.Now.AddYears(20) ? "tillsvidare" : subscriptionItem.EndDate.ToShortDateString();

                menu.Add(subscriptionMenuItem);
            }

            return menu;
        } 

        private void PopulateCustomerFacts(Subscriber subscriber, MySettingsPageMenu model)
        {
            model.ServicePlusEmail = subscriber.ServicePlusUser != null ? subscriber.ServicePlusUser.Email : string.Empty;
            model.IsPrivateSubscriber = subscriber.PrivateSubscription != null;
            model.IsBusinessAdminSubscriber = subscriber.BusinessSubscription != null;

            if (subscriber.SelectedSubscription == null) return;
            model.CustomerNumber = subscriber.SelectedSubscription.CustomerNumber.ToString(CultureInfo.InvariantCulture);
            model.SelectedSubscriptionType = subscriber.SelectedSubscription.Type;
        }

        #endregion
    }

    public class SubscriptionMenuItem
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<IContent> Pages { get; set; }
        public SubscriptionItem SubscriptionItem { get; set; }
    }
}