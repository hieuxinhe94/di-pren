using System.Collections.Generic;
using System.IO;

namespace Pren.Web.Business.BusinessSubscription
{
    public interface IInviteImporter
    {
        bool FileExtensionIsAccepted(string filePath);

        List<ImportRow> GetImportRows(Stream stream);
    }
}
