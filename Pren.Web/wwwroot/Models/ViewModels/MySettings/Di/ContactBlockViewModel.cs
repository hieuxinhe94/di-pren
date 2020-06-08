using System;
using Pren.Web.Models.Blocks.Di;

namespace Pren.Web.Models.ViewModels.MySettings.Di
{
    public class ContactBlockViewModel
    {
        public ContactBlockViewModel(ContactBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }

        public ContactBlock CurrentBlock { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool HideForm { get; set; }

        public bool SendEmail { get; set; }
    }
}
