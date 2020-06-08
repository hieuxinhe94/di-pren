using System;
using System.Collections.Generic;
using System.Linq;
using Bn.CompanyService.ContentApi;
using Bn.CompanyService.ContentApi.Models;
using EPiServer;
using EPiServer.Core;
using Pren.Web.Business.Configuration;
using Pren.Web.Models.Pages;
using Pren.Web.Business.EpiCompanyServiceContentApi.Models.Pages;
using Pren.Web.Business.EpiCompanyServiceContentApi.Models.Blocks;

namespace Pren.Web.Business.EpiSelfServiceContentApi
{
    public class EpiCompanyServiceContentService : ICompanyServiceContentService
    {
        private readonly IContentRepository _contentRepository;
        private readonly ISiteSettings _siteSettings;

        public EpiCompanyServiceContentService(
            IContentRepository contentRepository,
            ISiteSettings siteSettings)
        {
            _contentRepository = contentRepository;
            _siteSettings = siteSettings;
        }

        public IEnumerable<Message> GetMessages(string brand)
        {
            var companyServiceHomePage = GetCompanyServiceHomePage(brand);

            var messageBlocks =
                companyServiceHomePage.MessageContentArea?.Items.Select(block => block.GetContent())
                    .OfType<MessageBlock>()
                    .ToList() ?? new List<MessageBlock>();

            var messages = new List<Message>();

            foreach (var messageBlock in messageBlocks)
            {
                var now = DateTime.Now;

                if (now > messageBlock.VisibleFrom && now < messageBlock.VisibleTo)
                {
                    var id = (messageBlock as IContent).ContentLink.ID.ToString();
                    messages.Add(new Message(id, messageBlock.MessageType ?? "", messageBlock.Title ?? "", messageBlock.Text?.ToString() ?? ""));
                }
            }

            return messages;
        }

        private CompanyServiceHomePage GetCompanyServiceHomePage(string brand)
        {
            //brand not supported yet... Hardcoded to Di
            StartPage startPage;

            try
            {
                startPage = _contentRepository.Get<StartPage>(ContentReference.StartPage);
            }
            catch (Exception)
            {
                // Fallback to hardcoded value for startpage. this is because when using the stage address ContentReference.StartPage is null for some reason.
                startPage = _contentRepository.Get<StartPage>(new ContentReference(5));
            }

            var companyServiceStartPage = _contentRepository.Get<CompanyServiceHomePage>(startPage.DiCompanyServiceStartPage);

            return companyServiceStartPage;
        }
    }
}