using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Di.Common.Logging;
using EPiServer.Web.Mvc;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.DataAccess;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Utils.Replace;
using Pren.Web.Models.CustomForms;
using Pren.Web.Models.Pages;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers
{
    [NoCache]
    public class BusinessSubscriptionActivationPageController : PageController<BusinessSubscriptionActivationPage>
    {
        private readonly IServicePlusFacade _servicePlusFacade;
        private readonly ILogger _logService;
        private readonly IDataAccess _dataAccess;

        public BusinessSubscriptionActivationPageController(
            IServicePlusFacade servicePlusFacade,
            ILogger logService, 
            IDataAccess dataAccess)
        {
            _servicePlusFacade = servicePlusFacade;
            _logService = logService;
            _dataAccess = dataAccess;
        }

        public ActionResult Index(BusinessSubscriptionActivationPage currentPage, string bizSubscriptionId, string registrationCode)
        {
            var model = new BusinessSubscriptionActivationPageViewModel(currentPage);

            try
            {
                ViewData.GetEditHints<BusinessSubscriptionActivationPageViewModel, BusinessSubscriptionActivationPage>().AddFullRefreshFor(page => page.UspProduct);

                if (currentPage.UspProduct > 0)
                {
                    var uspTexts = _dataAccess.UspHandler.GetUspTexts(currentPage.UspProduct).ToList();
                    if (uspTexts.Any())
                    {
                        model.UspTexts = uspTexts.Select(uspText => ReplaceUtil.ReplacePlaceholderWithImage(uspText.Text)).ToList();
                    }
                }
            }
            catch (Exception exception)
            {
                // TODO Add logging when implemented /TKM
                //_dataAccess.Logger.Log(exception, "GetUspTexts failed", LogLevel.Error,  null);
            }

            // Queryparameters are not valid - log and return, the view will display generic error message
            if (string.IsNullOrEmpty(bizSubscriptionId) || string.IsNullOrEmpty(registrationCode))
            {
                model.DisplayErrorMessage = true;
                _logService.Log("BusinessSubscriptionActivationPage - Activation failed - " + String.Format("Queryparameters are not valid bizSubscriptionId: '{0}', registrationCode: '{1}'", bizSubscriptionId, registrationCode), LogLevel.Error, typeof(BusinessSubscriptionActivationPageController));
                return View(model);
            }

            try
            {
                // Try to find the invited subsciber with the url parameters
                var invitedBizSubscriber = _servicePlusFacade.GetBizSubscriberByInviteCode(bizSubscriptionId, registrationCode);

                // Could not find invited subscriber - log and return, the view will display generic error message
                if (invitedBizSubscriber == null || string.IsNullOrEmpty(invitedBizSubscriber.Email))
                {
                    model.DisplayErrorMessage = true;
                    model.InviteExpired = true;
                    _logService.Log("BusinessSubscriptionActivationPage - Activation failed - " + String.Format("Could not find subscriber bizSubscriptionId: '{0}', registrationCode: '{1}'", bizSubscriptionId, registrationCode), LogLevel.Error, typeof(BusinessSubscriptionActivationPageController));
                    return View(model);
                }

                var user = _servicePlusFacade.GetUserByEmail(invitedBizSubscriber.Email);

                // User already exists in S+ - log and return, the view will display generic error message
                if (user != null)
                {
                    model.DisplayErrorMessage = true;
                    _logService.Log("BusinessSubscriptionActivationPage - Activation failed - " + String.Format("S+ account already exists bizSubscriptionId: '{0}', registrationCode: '{1}', email: {2}", bizSubscriptionId, registrationCode, user.Email), LogLevel.Error, typeof(BusinessSubscriptionActivationPageController));
                    return View(model);
                }

                model.ActivationForm.Email = invitedBizSubscriber.Email;
                model.ActivationForm.BizSubscriptionId = bizSubscriptionId;
                model.InvitingCompanyName = GetInvitingCompanyName(bizSubscriptionId);

                return View(model);
            }
            catch (Exception exception)
            {
                model.DisplayErrorMessage = true;
                _logService.Log(exception, String.Format("BusinessSubscriptionActivationPage - Activation failed bizSubscriptionId: '{0}', registrationCode: '{1}'", bizSubscriptionId, registrationCode), LogLevel.Error, typeof(BusinessSubscriptionActivationPageController));
                return View(model);
            }
        }

        public ActionResult PostForm(BusinessSubscriptionActivationPage currentPage, BusinessSubscriptionActivationFormModel postedForm)
        {
            var model = new BusinessSubscriptionActivationPageViewModel(currentPage) {ActivationForm = postedForm};

            if (!IsValidActivationForm(postedForm))
            {
                model.DisplayErrorMessage = true;
                _logService.Log("BusinessSubscriptionActivationPage - Activation failed - " + String.Format("Posted form is not valid. Formvalues: '{0}'", postedForm != null ? postedForm.ToLogString() : "postedForm is null"), LogLevel.Error, typeof(BusinessSubscriptionActivationPageController));
                return View("Index", model);
            }

            try
            {
                // User does not exist in S+ - Create it
                // Generate pw for user
                var password = System.Web.Security.Membership.GeneratePassword(6, 0);

                // Create the user
                var user = _servicePlusFacade.CreateUser(
                    postedForm.Email,
                    postedForm.Phone,
                    postedForm.FirstName,
                    postedForm.LastName,
                    password,
                    true);

                // User could not be created - log and return, the view will display generic error message
                if (user == null)
                {                    
                    _logService.Log("BusinessSubscriptionActivationPage - Activation failed - " + String.Format("Could not create user. Formvalues: '{0}'", postedForm.ToLogString()), LogLevel.Error, typeof(BusinessSubscriptionActivationPageController));
                    model.DisplayErrorMessage = true;
                    return View("Index", model);
                }

                var successfullyActivated = _servicePlusFacade.ActivateBizSubscriber(postedForm.BizSubscriptionId, user.Email);

                // User could not be activated - log and return, the view will display generic error message
                if (!successfullyActivated)
                {                    
                    _logService.Log("BusinessSubscriptionActivationPage - Activation failed - " + String.Format("Could not activate user. Formvalues: '{0}'", postedForm.ToLogString()), LogLevel.Error, typeof(BusinessSubscriptionActivationPageController));
                    model.DisplayErrorMessage = true;
                    return View("Index", model);
                }

                return View("ThankYou", model);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, String.Format("BusinessSubscriptionActivationPage - Activation failed Could not activate user, Formvalues: '{0}'", postedForm.ToLogString()), LogLevel.Error, typeof(BusinessSubscriptionActivationPageController));
                model.DisplayErrorMessage = true;
                return View("Index", model);
            }
        }

        private string GetInvitingCompanyName(string bizSubscriptionId)
        {
            var businessSubcription = _servicePlusFacade.GetBizSubscription(bizSubscriptionId);
            if (businessSubcription != null && !string.IsNullOrEmpty(businessSubcription.CompanyName))
            {
                return businessSubcription.CompanyName;
            }

            return string.Empty;
        }

        private bool IsValidActivationForm(BusinessSubscriptionActivationFormModel form)
        {
            if (form == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(form.Email))
            {
                return false;
            }
            if (string.IsNullOrEmpty(form.Phone))
            {
                return false;
            }
            if (string.IsNullOrEmpty(form.FirstName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(form.LastName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(form.BizSubscriptionId))
            {
                return false;
            }

            return true;
        }
    }
}
