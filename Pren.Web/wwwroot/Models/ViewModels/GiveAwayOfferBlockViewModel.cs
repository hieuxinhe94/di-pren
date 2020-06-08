using Pren.Web.Models.Blocks;

namespace Pren.Web.Models.ViewModels
{
    public class GiveAwayOfferBlockViewModel
    {
        public GiveAwayOfferBlockViewModel(GiveAwayOfferBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }

        public GiveAwayOfferBlock CurrentBlock { get; set; }
        public string CodeListId { get; set; }
    }
}
