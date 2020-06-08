using System.Collections.Generic;
using Pren.Web.Models.Blocks;

namespace Pren.Web.Models.ViewModels
{
    public class CampaignBlockViewModel
    {
        public CampaignBlockViewModel(CampaignBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }

        public CampaignBlock CurrentBlock { get; set; }
        public List<string> UspTexts { get; set; }
    }
}
