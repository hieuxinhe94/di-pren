using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Di.Common.Logging;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.BusinessSubscription;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Messaging;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.MySettings
{
    // ReSharper disable Mvc.ViewNotResolved
    public class BusinessSubscriptionPageController : MySettingsControllerBase<BusinessSubscriptionPage>
    {
        private readonly IServicePlusFacade _servicePlusFacade;
        private readonly IPriceHandler _priceHandler;
        private readonly ILogger _logService;
        private readonly IDetectionHandler _detection;
        private readonly IInviteImporter _inviteImporter;
        private readonly IUrlHelper _urlHelper;
        private readonly IContentRepository _contentRepository;

        public BusinessSubscriptionPageController(
            IServicePlusFacade servicePlusFacade,
            ISessionData sessionData, 
            IPriceHandler priceHandler,
            ILogger logService, IDetectionHandler detection, 
            IInviteImporter inviteImporter, 
            IUrlHelper urlHelper, 
            IContentRepository contentRepository)
            : base(sessionData)
        {
            _servicePlusFacade = servicePlusFacade;
            _priceHandler = priceHandler;
            _logService = logService;
            _detection = detection;
            _inviteImporter = inviteImporter;
            _urlHelper = urlHelper;
            _contentRepository = contentRepository;
        }

        [AuthorizeCheck]      
        public async Task<ActionResult> Index(BusinessSubscriptionPage currentPage, string activeTab, bool reciept = false)
        {
            var model = new BusinessSubscriptionPageViewModel(currentPage);
           
            var subscriber = GetSubscriberFromSession();

            ClearInvalidSession(subscriber);

            if (subscriber == null || subscriber.SelectedSubscription == null || subscriber.SelectedSubscription.Type != SubscriptionType.Business)
            {
                return Redirect(Url.ContentUrl(_contentRepository.Get<StartPage>(ContentReference.StartPage).MySettingsStartPage));
            }

            // If a pending yet we try to fetch it on each page load
            if (subscriber.SelectedSubscription.CustomerNumber == 0 && subscriber.SelectedSubscription.IsPending)
            {
                subscriber = await UpdateSubscriberFromSources(subscriber);               
            }
            
            var servicePlusUser = subscriber.ServicePlusUser;

            if (servicePlusUser == null)
            {
                _logService.Log("BusinessSubscriptionAdminPage - Could not show biz subscription - " + String.Format("subscriber.ServicePlusUser is null"), LogLevel.Error, typeof(BusinessSubscriptionPageController));                
                return View(model);
            }

            var bizSubscriptions = _servicePlusFacade.GetBizSubscriptions(servicePlusUser.Id).ToList();

            if (!bizSubscriptions.Any())
            {
                _logService.Log("BusinessSubscriptionAdminPage - Could not show biz subscription - " + String.Format("servicePlusUser with id: '{0}' does not have a biz subscription", servicePlusUser.Id), LogLevel.Error, typeof(BusinessSubscriptionPageController));
                return ViewWithErrorMessage(model);
            }

            var bizSubscription = bizSubscriptions.First();

            var bizSubscriptionEntitlement = _servicePlusFacade.GetEntitlement(bizSubscription.EntitlementId);

            if (bizSubscriptionEntitlement == null || 
                (bizSubscriptionEntitlement.ValidTo != null &&
                bizSubscriptionEntitlement.ValidTo < DateTime.Now))
            {
                _logService.Log("BusinessSubscriptionAdminPage - Could not show biz subscription - " + String.Format("Entitlement not valid, s+ userid: '{0}', entitlementId: '{1}'", servicePlusUser.Id, bizSubscription.EntitlementId), LogLevel.Error, typeof(BusinessSubscriptionPageController));
                return View("ExpiredSubscription", model);
            }

            var bizSubscriptionDefinition = _servicePlusFacade.GetBizSubscriptionDefinition(bizSubscription.DefinitionId);

            if (bizSubscriptionDefinition == null)
            {
                _logService.Log("BusinessSubscriptionAdminPage - Could not show biz subscription - " + String.Format("bizSubscriptionDefinition is null, s+ userid: '{0}', definitionId: '{1}'", servicePlusUser.Id, bizSubscription.DefinitionId), LogLevel.Error, typeof(BusinessSubscriptionPageController));
                return ViewWithErrorMessage(model);
            }

            model.MasterUser = new BusinessSubscriber
            {
                Activated = true,
                Email = servicePlusUser.Email,
                FirstName = servicePlusUser.FirstName,
                LastName = servicePlusUser.LastName
            };
            model.ShowReciept = reciept;
            model.SubscriptionId = bizSubscription.Id;
            model.CompanyName = bizSubscription.CompanyName;
            model.MinNumberOfAllowedSubscribers = bizSubscriptionDefinition.MinQuantity;
            model.MaxNumberOfAllowedSubscribers = bizSubscriptionDefinition.MaxQuantity;
            model.AccountPricePerMonth = _priceHandler.GetPrice(bizSubscriptionDefinition.ExternalProductCode);
            model.BizSubscriberCustomerNumber = servicePlusUser.KayakBizSubscriptionCustomerNumber;
            model.ActiveTab = activeTab;

            return View(model);
        }

        [HttpPost]
        [AuthorizeUser]
        public ActionResult ImportFile(BusinessSubscriptionPage currentPage, string businessSubscriptionId, HttpPostedFileBase inviteFile)
        {
            if (inviteFile == null)
            {
                return RedirectToAction("Index", new { activeTab = BusinessSubscriberTabs.invites });
            }

            const string errorTemplate = "Misslyckades skicka inbjudan till <strong>{0}</strong>, {1}.<br/><br/>";            
            var reloadLink = "<a href='" + _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + "' class='btn btn-primary'>Ladda om sidan</a>";
            var importTemplate = "Importerat <strong>{0}</strong> av <strong>{1}</strong> e-postadresser<br/><br/><strong>Det kan ta en stund innan importerade adresser visas i listan nedan.</strong><br/><br/>" + reloadLink;
            
            var model = new BusinessSubscriptionPageViewModel(currentPage);
            var message = new StringBuilder();            

            try
            {
                if (!_inviteImporter.FileExtensionIsAccepted(inviteFile.FileName))
                {
                    return ViewWithErrorMessage(model, "Felaktigt filformat. Kontrollera att din fil är i formatet txt eller csv.<br/><br/>" + reloadLink);
                }

                var importRows = _inviteImporter.GetImportRows(inviteFile.InputStream);
                var noOfRowsToImport = importRows.Count;
                var noOfRowsImported = 0;

                foreach (var importRow in importRows)
                {
                    if (!_detection.IsValidEmail(importRow.Email))
                    {
                        message.Append(string.Format(errorTemplate, importRow.Email, "felaktig e-postadress"));
                        continue;
                    }

                    var importSucceded = _servicePlusFacade.InviteBizSubscriberByEmail(businessSubscriptionId, importRow.Email);

                    if (!importSucceded)
                    {
                        message.Append(string.Format(errorTemplate, importRow.Email, "kontrollera att du angivit en e-postadress som inte redan är inbjuden"));
                    }
                    else
                    {
                        noOfRowsImported++;
                    }
                }

                message.Append(string.Format(importTemplate, noOfRowsImported, noOfRowsToImport));
                
                TempData["Message"] = new Message(message.ToString());

                return RedirectToAction("Index", new { activeTab = BusinessSubscriberTabs.invites });
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "BusinessSubscriptionAdminPage - ImportFile", LogLevel.Error, typeof(BusinessSubscriptionPageController));  
                return ViewWithErrorMessage(model, "Ett fel har uppstått vid import. Kontrollera formatet i filen du vill importera.<br/><br/>" + reloadLink);                               
            }            
        }

        private ViewResult ViewWithErrorMessage(BusinessSubscriptionPageViewModel model, string errorMessage = null)
        {
            TempData["Message"] = new Message(errorMessage ?? "Ett fel har uppstått. Var god försök senare.", MessageType.Danger);
              
            return View("Index", model);
        }
    }

    public class BusinessSubscriber
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public bool Activated { get; set; }
    }

    // ReSharper disable InconsistentNaming
    public enum BusinessSubscriberTabs
    {
        subscribers,
        invites
    }
}