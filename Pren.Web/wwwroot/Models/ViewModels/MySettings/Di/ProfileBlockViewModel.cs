using Pren.Web.Models.Blocks.Di;

namespace Pren.Web.Models.ViewModels.MySettings.Di
{
    public class ProfileBlockViewModel
    {
        public ProfileBlockViewModel(ProfileBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }

        public ProfileBlock CurrentBlock { get; set; }

        public string ChangePasswordUrl { get; set; }
    }
}
