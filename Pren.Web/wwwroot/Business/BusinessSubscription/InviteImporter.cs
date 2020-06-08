using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using Di.Common.Utils.File;

namespace Pren.Web.Business.BusinessSubscription
{
    public class InviteImporter : IInviteImporter
    {
        public string[] AcceptedFileExtensions = {".txt", ".csv"}; 

        public bool FileExtensionIsAccepted(string filePath)
        {
            return FileUtils.IsValidFileExtension(AcceptedFileExtensions, filePath);
        }

        public List<ImportRow> GetImportRows(Stream stream)
        {
            using (var inputStreamReader = new StreamReader(stream, Encoding.Default))
            {
                var csvReader = new CsvReader(inputStreamReader);
                csvReader.Configuration.WillThrowOnMissingField = false;
                csvReader.Configuration.TrimFields = true;
                csvReader.Configuration.HasHeaderRecord = false;

                return csvReader.GetRecords<ImportRow>().ToList();
            }
        }
    }

    public class ImportRow
    {
        public string Email { get; set; }
    }
}