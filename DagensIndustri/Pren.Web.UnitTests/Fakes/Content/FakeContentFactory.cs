using EPiServer.Core;

namespace Pren.Web.UnitTests.Fakes.Content
{
    public class FakeContentFactory
    {
        public static TContent GetFakePageContent<TContent>(int pageId = 1) where TContent : IContent, new()
        {
            var fakeContent = new TContent();
            fakeContent.Property["PageLink"] = new PropertyPageReference(pageId);

            return fakeContent;
        }

        public static TContent GetFakeBlockContent<TContent>(int pageId = 1) where TContent : BlockData, new()
        {
            var fakeContent = new TContent();
            fakeContent.Property["PageLink"] = new PropertyPageReference(pageId);

            return fakeContent;
        }
    }
}
