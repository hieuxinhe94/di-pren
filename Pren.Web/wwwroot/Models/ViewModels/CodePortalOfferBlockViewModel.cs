using Pren.Web.Models.Blocks;

namespace Pren.Web.Models.ViewModels
{
    public class CodePortalOfferBlockViewModel
    {
        public CodePortalOfferBlockViewModel(CodePortalOfferBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }

        public CodePortalOfferBlock CurrentBlock { get; set; }
        public string CodeListId { get; set; }
    }
}
