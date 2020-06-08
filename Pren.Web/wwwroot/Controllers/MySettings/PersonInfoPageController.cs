using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Di.Common.Logging;
using Di.Subscription.Logic.Customer;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Messaging;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.CustomForms.MySettings;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.MySettings
{
    public class PersonInfoPageController : MySettingsControllerBase<PersonInfoPage>
    {
        private readonly ILogger _logService;
        private readonly ISiteSettings _siteSettings;
        private readonly IDetectionHandler _detectionHandler;
        private readonly IServicePlusFacade _servicePlusFacade;

        private readonly ICustomerHandler _customerHandler;

        public PersonInfoPageController(
            ILogger logService,
            IDetectionHandler detectionHandler,
            ISiteSettings siteSettings,
            ISessionData sessionData, 
            ICustomerHandler customerHandler, 
            IServicePlusFacade servicePlusFacade) : base(sessionData)
        {
            _logService = logService;
            _siteSettings = siteSettings;
            _detectionHandler = detectionHandler;
            _customerHandler = customerHandler;
            _servicePlusFacade = servicePlusFacade;
        }

        [AuthorizeUser]
        public ActionResult Index(PersonInfoPage currentPage)
        {            
            var model = new PersonInfoViewModel(currentPage);

            var subscriber = GetSubscriberFromSession();

            model.PersonInfoForm.Email = subscriber.SelectedSubscription.KayakCustomer.Email;
            model.PersonInfoForm.Phone = subscriber.SelectedSubscription.KayakCustomer.PhoneOffice;
            model.ServicePlusEmail = subscriber.ServicePlusUser.Email;
            model.ServicePlusForgotPasswordUrl = _siteSettings.ServicePlusForgotPasswordUrl;
            model.ServicePlusChangePasswordUrl = _siteSettings.ServicePlusChangePasswordUrl;

            // ReSharper disable once Mvc.ViewNotResolved
            return View(model);
        }

        [AuthorizeUser]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostForm(PersonInfoPage currentPage, PersonInfoFormModel postedForm)
        {
            var model = new PersonInfoViewModel(currentPage) {PersonInfoForm = postedForm};

            var error = new StringBuilder();
            var email = postedForm.Email.ToLower();
            var phone = postedForm.Phone;

            var subscriber = GetSubscriberFromSession();

            if (!ModelState.IsValid)
            {
                model.Message = new Message("Ett tekniskt fel uppstod. Kontrollera att fälten är korrekt formaterade och försök igen.",MessageType.Danger);
                _logService.Log("PersonInfoPageController PostForm - Model not valid - Cusno: " + subscriber.SelectedSubscription.CustomerNumber + ", Email:" + email + ", Mobnr:" + phone, LogLevel.Error, typeof(PersonInfoPageController));

                // ReSharper disable Mvc.ViewNotResolved
                return View("Index", model);
            }

            if (!string.IsNullOrEmpty(phone))
            {
                phone = FormatPhoneNumber(phone, _siteSettings.PhoneMaxNoOfDigits, true);
            }

            var isValidEmail = _detectionHandler.IsValidEmail(email);

            if (!isValidEmail)
            {
                error.Append("E-posten är inte giltig");
            }
            
            //email required (cannot be empty)
            if (isValidEmail && email != subscriber.SelectedSubscription.KayakCustomer.Email.ToLower())
            {
                if (!UpdateSubscriberEmail(subscriber, email))
                {
                    error.Append("E-postadressen kunde inte sparas. ");
                }
                else
                {
                    try
                    {
                        _servicePlusFacade.UpdateUser(subscriber.ServicePlusToken, string.Empty, string.Empty, string.Empty, email);
                    }
                    catch (Exception exception)
                    {
                        _logService.Log(exception, "Failed to update S+ email " + email, LogLevel.Error, typeof(PersonInfoPageController));
                    }
                }                
            }

            if (_detectionHandler.IsValidSwePhoneNum(phone) && (phone != subscriber.SelectedSubscription.KayakCustomer.PhoneOffice))
            {
                if (!UpdatePhoneMobile(subscriber, phone))
                {
                    error.Append("Mobilnumret kunde inte sparas.");
                }                    
            }

            if (error.ToString().Length > 0)
            {
                TempData["Message"] = new Message("Ett tekniskt fel uppstod. " + error, MessageType.Danger);

                return View("Index", model);
            }

            TempData["Message"] = new Message("Dina personuppgifter har sparats");

            // Update subscriber from sources to get new updated values
            await UpdateSubscriberFromSources(subscriber);

            return RedirectToAction("Index");
        }

        private bool UpdateSubscriberEmail(Subscriber subscriber, string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                    email = email.ToLower();

                return _customerHandler.UpdateCustomerEmail(subscriber.SelectedSubscription.CustomerNumber, email);
            }
            catch (Exception ex)
            {
                _logService.Log(ex, "UpdateEmail() failed", LogLevel.Error, typeof(PersonInfoPageController));
            }

            return false;
        }

        private bool UpdatePhoneMobile(Subscriber subscriber, string phoneMob)
        {
            try
            {
                return _customerHandler.UpdateCustomerPhone(subscriber.SelectedSubscription.CustomerNumber, phoneMob);
            }
            catch (Exception ex)
            {
                _logService.Log(ex, "UpdatePhoneMobile() failed", LogLevel.Error, typeof(PersonInfoPageController));
            }
            return false;
        }

        /// <summary>
        ///  Format phone number to match Kayak requirements. 
        ///  It has to start with a country code, have a max no of digits and only contain digits and a '+'.
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="maxNoOfDigits"></param>
        /// <param name="doMobileCheck"></param>
        /// <returns></returns>
        public static string FormatPhoneNumber(string phoneNo, int maxNoOfDigits, bool doMobileCheck)
        {
            var num = string.Empty;
            if (!string.IsNullOrEmpty(phoneNo))
            {
                num = phoneNo.Trim();
                num = num.Replace("-", "");
                num = num.Replace("/", "");
                num = num.Replace(@"\", "");
                num = num.Replace(" ", "");

                if (!num.StartsWith("+"))
                {
                    if (num.StartsWith("00"))
                    {
                        num = string.Format("+{0}", num.Substring(2));
                    }
                    else if (num.StartsWith("0"))
                    {
                        num = num.Substring(1);
                        num = string.Format("+46{0}", num);
                    }
                }

                if (doMobileCheck && num.StartsWith("+46"))
                {
                    string phoneNoCountryCode = num.Substring(3);

                    //Check if mobile number without country code, starts with 1 or 7 (start digits of a mobile number)
                    if (!phoneNoCountryCode.StartsWith("1") && !phoneNoCountryCode.StartsWith("7"))
                        return null;
                }

                //Check if phone number only has digits (excluding + in country code) and it doesn't exceed max no of digits
                Int64 parsedPhoneNo;
                if (!Int64.TryParse(num.Substring(1), out parsedPhoneNo) || num.Length > maxNoOfDigits)
                    num = null;
            }

            return num;
        }
    }
}