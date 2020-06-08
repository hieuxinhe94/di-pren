using System.Web.Helpers;

namespace Pren.Web.Business.Detection
{
    public class AntiForgeryValidator : IAntiForgeryValidator
    {
        public void Validate()
        {
            AntiForgery.Validate();
        }
    }
}