using System;
using System.Linq;
using System.Web.Mvc;
using Di.Common.Logging;
using EPiServer.Web.Mvc;
using Pren.Web.Business.DataAccess;
using Pren.Web.Business.Utils.Replace;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers
{
    public class CampaignBlockController : BlockController<CampaignBlock>
    {
        private readonly IDataAccess _dataAccess;

        public CampaignBlockController(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public override ActionResult Index(CampaignBlock currentBlock)
        {
            var model = new CampaignBlockViewModel(currentBlock);

            try
            {
                ViewData.GetEditHints<CampaignBlockViewModel, CampaignBlock>().AddFullRefreshFor(block => block.UspProduct);

                if (currentBlock.UspProduct > 0)
                {
                    var uspTexts = _dataAccess.UspHandler.GetUspTexts(currentBlock.UspProduct).ToList();
                    if (uspTexts.Any())
                    {
                        model.UspTexts = uspTexts.Select(uspText => ReplaceUtil.ReplacePlaceholderWithImage(uspText.Text)).ToList();
                    }
                }
            }
            catch (Exception exception)
            {     
                _dataAccess.Logger.Log(exception, "GetUspTexts failed", LogLevel.Error, typeof(CampaignBlockController));
            }

            return PartialView(model);
        }
    }
}
