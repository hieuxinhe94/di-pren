using System;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Blocks.Di;
using Pren.Web.Models.ViewModels.MySettings.Di;

namespace Pren.Web.Controllers.Blocks.Di
{
    // ReSharper disable Mvc.PartialViewNotResolved
    public class ContactBlockController : BlockController<ContactBlock>
    {
        private readonly ISubscriberFacade _subscriberFacade;

        public ContactBlockController(ISubscriberFacade subscriberFacade)
        {
            _subscriberFacade = subscriberFacade;
        }

        public override ActionResult Index(ContactBlock currentBlock)
        {
            var model = new ContactBlockViewModel(currentBlock);

            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            model.IsLoggedIn = subscriber != null;
            model.HideForm = !ShowForm(currentBlock);
            model.SendEmail = currentBlock.SendMailOnSpecificTimes && ShouldSendEmail(currentBlock);
            
            return PartialView(model);
        }

        private bool ShowForm(ContactBlock currentBlock)
        {
            var isWeekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(DateTime.Now.DayOfWeek);

            // Never hide form on weekends
            if (isWeekend) return false;

            var startTime = new TimeSpan(currentBlock.FormShowFrom, 0, 0);
            var endTime = new TimeSpan(currentBlock.FormShowTo, 0, 0);
            var now = DateTime.Now.TimeOfDay;

            return currentBlock.ShowForm || (now > startTime) && (now < endTime);
        }

        private bool ShouldSendEmail(ContactBlock currentBlock)
        {
            var isWeekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(DateTime.Now.DayOfWeek);

            if (isWeekend)
            {
                var startTime = new TimeSpan(currentBlock.WeekendChatOpenFrom, 0, 0);
                var endTime = new TimeSpan(currentBlock.WeekendChatOpenTo, 0, 0);
                var now = DateTime.Now.TimeOfDay;
                if ((now > startTime) && (now < endTime))
                {
                    return false;
                }
                return true;
            }
            else
            {
                var startTime = new TimeSpan(currentBlock.VardagChatOpenFrom, 0, 0);
                var endTime = new TimeSpan(currentBlock.VardagChatOpenTo, 0, 0);
                var now = DateTime.Now.TimeOfDay;
                if((now > startTime) && (now < endTime))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
