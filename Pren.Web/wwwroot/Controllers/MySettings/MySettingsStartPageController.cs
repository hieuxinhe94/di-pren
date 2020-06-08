using System.Web.Mvc;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Messaging;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.MySettings 
{
    // ReSharper disable Mvc.ViewNotResolved  
    public class MySettingsStartPageController : MySettingsControllerBase<MySettingsStartPage>
    {
        public MySettingsStartPageController(       
            ISessionData sessionData): 
            base(sessionData)
        {
        }

        #region Actions

             
        [AuthorizeCheck]
        public ActionResult Index(MySettingsStartPage currentPage)
        {
            var subscriber = GetSubscriberFromSession();

            var message = string.Empty;

            var connectPageUrl = GetConnectPage().LinkURL;

            switch (GetConnectStatus(subscriber))
            {
                case ConnectStatus.InvalidCode:
                    message = "Du har angivit en felaktig kod. Vänligen kontakta kundservice.";
                    break;
                case ConnectStatus.ConnectExistingPrenWithServicePlus:
                    // User logged in by code only
                    message = "För att kunna administrera din prenumeration behöver du koppla ditt konto – gör det redan idag på <a href='" + connectPageUrl + "'>di.se/pren/koppla</a>";
                    break;
                case ConnectStatus.ConnectExistingServicePlusWithPren:
                    // Logged in S+, but no entitlement with cusno
                    message = "För att kunna administrera din prenumeration samt få tillgång till hela nyhetsutbudet behöver du koppla ditt konto. Gör det redan idag på <a href='" + connectPageUrl + "'>di.se/pren/koppla</a>";
                    break;
                case ConnectStatus.ConnectExistingServicePlusWithExistingPren:
                    // Logged in S+ and by code in url
                    message = "För att kunna administrera din prenumeration behöver du koppla ditt konto – gör det redan idag på <a href='" + connectPageUrl + "'>di.se/pren/koppla</a>";
                    break;
                case ConnectStatus.UnableToConnectPrenWithServicePlus:
                    message = "Ditt Di-konto är kopplat till ett annat kundnummer. Vänligen kontakta kundservice.";
                    break;
            }

            TempData["Message"] = !string.IsNullOrEmpty(message) ? new Message(message) : null;

            ClearInvalidSession(subscriber);

            return View(new MySettingsPageViewModel(currentPage));
        }

        #endregion
    }
}