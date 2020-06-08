using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DIClassLib.DbHelpers;
using System.Configuration;
using System.Net;
using System.IO;



namespace DIClassLib.StudentVerification
{
    public class StudentVerifier
    {

        public StudentVerifier() { }


        /// <summary>
        /// Verify that person is student by birth number.
        /// </summary>
        /// <param name="birthNum">YYMMDDXXXX</param>
        /// <returns>1=full time student, 0=not full time student, 99=bad/missing input, -1=local exception</returns>
        public string VerifyByBirthNum(string birthNum)
        {
            if (birthNum.Length != 10 || !DIClassLib.Misc.MiscFunctions.IsNumeric(birthNum))
                return "99";

            string url = ConfigurationManager.AppSettings["StudentVerificationUrl"];
            string type = ConfigurationManager.AppSettings["StudentVerificationType"];

            var request = (HttpWebRequest)WebRequest.Create(url + "?persnr=" + birthNum + "&type=" + type);
            try
            {
                using (WebResponse webResp = request.GetResponse())
                {
                    Stream strm = webResp.GetResponseStream();
                    StreamReader strmRdr = new StreamReader(strm);
                    string fullResponse = strmRdr.ReadToEnd().Trim();

                    strmRdr.Close();
                    strm.Close();
                    webResp.Close();

                    if (string.IsNullOrEmpty(fullResponse))
                        return "99";

                    return fullResponse;
                }
            }
            catch (Exception ex)
            {
                new Logger("VerifyByBirthNum - failed. birthNum: " + birthNum, ex.ToString());
                return "-1";
            }

            #region alternative catch
            //catch (WebException ex)
            //{
            //    using (WebResponse webResp = ex.Response)
            //    {
            //        HttpWebResponse httpResp = (HttpWebResponse)webResp;
            //        string s = "Error code: " + httpResp.StatusCode;
            //        using (var streamReader = new StreamReader(webResp.GetResponseStream()))
            //            new Logger("", streamReader.ReadToEnd());
            //    }
            //}
            #endregion
        }


        /// <summary>
        /// Verify that person is student by birth number.
        /// </summary>
        /// <param name="birthNum">YYMMDDXXXX</param>
        /// <returns>1=full time student, 2=part time student, 0=not student, 99=bad/missing input, -1=local exception</returns>
        //public string VerifyByBirthNum(string birthNum)
        //{
        //    try
        //    {
        //        string url = ConfigurationManager.AppSettings["StudentVerificationUrl"];
        //        string type = ConfigurationManager.AppSettings["StudentVerificationType"];
                
        //        XmlTextReader reader = new XmlTextReader(url + "?persnr=" + birthNum + "&type=" + type);
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(reader);
        //        return doc.SelectSingleNode("root/status").InnerText;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("VerifyByBirthNum() failed for birthNum:" + birthNum, ex.ToString());
        //        return "-1";
        //    }


        //    //todo: ? implement timeout ?
        //    //http://stackoverflow.com/questions/134917/enforcing-a-strict-timeout-policy-on-a-server-side-webrequest
        //    //WebRequest request = WebRequest.Create("http://something.somewhere/url");
        //    //WebResponse response = null;
        //    //request.Timeout = 10000; // 10 second timeout
        //    //try
        //    //{
        //    //    response = request.GetResponse();
        //    //}
        //    //catch (WebException e)
        //    //{
        //    //    if (e.Status == WebExceptionStatus.Timeout)
        //    //    {
        //    //        //something
        //    //    }
        //    //}

        //}
    }
}
