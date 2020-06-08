using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;


namespace DIClassLib.Misc
{
    public class FtpHelper
    {

        public FtpHelper(string appKeyHost, string appKeyUser, string appKeyPassword)
        {
            FtpHost = MiscFunctions.GetAppsettingsValue(appKeyHost);
            FtpUser = MiscFunctions.GetAppsettingsValue(appKeyUser);
            FtpPassword = MiscFunctions.GetAppsettingsValue(appKeyPassword);
        }

        /// <summary>
        /// Uploads file to Ftp
        /// </summary>
        /// <param name="uploadFileFromPath">Full path to file to upload</param>
        /// <param name="uploadFileToPath">Target path (exclude ftp host)</param>
        public void UploadFileToFtp(string uploadFileFromPath, string uploadFileToPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(FtpHost + uploadFileToPath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(FtpUser, FtpPassword);
                request.Timeout = 360000;
                //request.UsePassive = true; 
                //request.UseBinary = true; 
                //request.KeepAlive = false;

                //Loading the file
                FileStream stream = File.OpenRead(uploadFileFromPath);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                //Uploading the file
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete file from ftp
        /// </summary>
        /// <param name="deleteFileFromPath">Path to delete (exclude ftp host)</param>
        public void DeleteFileFromFtp(string deleteFileFromPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(FtpHost + deleteFileFromPath);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(FtpUser, FtpPassword);
                request.Timeout = 360000;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Rename file on ftp
        /// </summary>
        /// <param name="renameFilePath">Target path (exclude ftp host)</param>
        /// <param name="newFileName">New file name (do not include path)</param>
        public void RenameFileOnFtp(string renameFilePath, string newFileName)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(FtpHost + renameFilePath);
                request.Method = WebRequestMethods.Ftp.Rename;
                request.RenameTo = newFileName;
                request.Credentials = new NetworkCredential(FtpUser, FtpPassword);
                request.Timeout = 360000;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string FtpHost { get; set; }

        private string FtpUser { get; set; }

        private string FtpPassword { get; set; }
    }
}
