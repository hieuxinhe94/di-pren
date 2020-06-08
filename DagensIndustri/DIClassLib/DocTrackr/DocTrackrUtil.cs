

namespace DIClassLib.DocTrackr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using DIClassLib.Misc;
    using DIClassLib.DbHandlers;
    using System.Web;
    using DIClassLib.DbHelpers;

    public class DocTrackrUtil
    {
        public DocTrackrUtil(){}


        /// <summary>
        /// Will stream a trackable document if possible. Fallback is path to org file. 
        /// If file cannot be found for provided 'dateIssue' nothing is returned.
        /// </summary>
        public void StreamFile(DateTime dateIssue, string servicePlusUserId, string ipNumber, string siteProvidedDownload)
        {
            var path = TryGetPathToDocForDownload(dateIssue, servicePlusUserId, ipNumber, siteProvidedDownload);
            if (string.IsNullOrEmpty(path))
                return;

            byte[] fileByteArray = ReadAllBytes(path);
            if (fileByteArray == null)
                return;

            var fileName = Path.GetFileName(path);  //33-YYYYMMDD.pdf

            try
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.AddHeader("Content-Type", "application/pdf");
                HttpContext.Current.Response.AddHeader("Content-Length", fileByteArray.Length.ToString());
                HttpContext.Current.Response.OutputStream.Write(fileByteArray, 0, fileByteArray.Length);
            }
            catch (Exception ex)
            {
                new Logger("StreamFile failed for dateIssue: " + dateIssue.ToShortDateString() + ", servicePlusUserId: " + servicePlusUserId + ", ipNumber: " + ipNumber + ", siteProvidedDownload: " + siteProvidedDownload, ex.ToString());
            }
            finally
            {
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }


        /// <summary>
        /// Will return path to trackable document if possible. Fallback is path to org file. 
        /// If file cannot be found for provided 'dateIssue' an empty string is returned. 
        /// Stores info in DocTrackr database, table customerDownload.
        /// </summary>
        private string TryGetPathToDocForDownload(DateTime dateIssue, string servicePlusUserId, string ipNumber, string siteProvidedDownload)
        {
            var yyyymmdd                = dateIssue.ToString("yyyyMMdd");
            var pathToOrgFile           = MiscFunctions.GetAppsettingsValue("PDFArchivePath") + yyyymmdd + ".pdf";
            //var pathToTrackableFileDir  = MiscFunctions.GetAppsettingsValue("DocTrackrTrackableDiFiles") + yyyymmdd + "\\";

            //org file must exist for dateIssue
            if (!File.Exists(pathToOrgFile))
            {
                new Logger("TryGetPathToDocForDownload() - orgfile does not exist. PathToOrgFile: " + pathToOrgFile, "");
                return string.Empty;
            }

            //var docName = MsSqlHandler.DocTrackr_TryGetTrackableDocName(dateIssue, servicePlusUserId, ipNumber, siteProvidedDownload);
            //var docPath = pathToTrackableFileDir + docName;

            //return path to org file if: file not found in DB / something wrong with file on disk
            //if (string.IsNullOrEmpty(docName) || !FileExistsAndHasContent(docPath))
            return pathToOrgFile;

            //return docPath;
        }

        //private bool FileExistsAndHasContent(string path)
        //{
        //    if (!File.Exists(path))
        //    {
        //        new Logger("FileExistsAndHasContent() - file does not exist for path: " + path, "");
        //        return false;
        //    }

        //    var fi = new FileInfo(path);
        //    if (fi.Length < 100)
        //    {
        //        new Logger("FileExistsAndHasContent() - fileSize < 100 for path: " + path, "");
        //        return false;
        //    }

        //    return true;
        //}

        private byte[] ReadAllBytes(string filePath)
        {
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                new Logger("ReadAllBytes() failed for filePath: " + filePath, ex.ToString());
                return null;
            }
        }

    }
}
