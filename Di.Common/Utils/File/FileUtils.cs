using System.IO;
using System.Linq;

namespace Di.Common.Utils.File
{
    public class FileUtils
    {
        public static bool IsValidFileExtension(string[] acceptedFileExtensions, string filePath)
        {
            return acceptedFileExtensions.Contains(Path.GetExtension(filePath));
        }
    }
}
