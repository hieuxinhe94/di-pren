using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Xml;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.DiPlus;
using EPiServer.Web.Hosting;


namespace DIClassLib.Misc
{
    public static class MiscFunctions
    {
        
        #region DISE

        //DISE
        //returnerar datum i svenskt format
        public static string GetSwedishDateFormat(DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// If possible, cast object to datetime and return a ShortDateString
        /// </summary>
        /// <param name="dateTime">Input parameter</param>
        /// <returns>A ShortDateString representation of DateTime object, otherwise an empty string</returns>
        public static string ToShortDateString(object dateTime)
        {
            if (dateTime != null && dateTime is DateTime)
                return GetSwedishDateFormat((DateTime)dateTime);

            return string.Empty;
        }

        #endregion

        #region DI Online

        public static string textalkGetLink(string issueDate, string placard, string link)
        {
            if (string.IsNullOrEmpty(link))
                link = ConfigurationManager.AppSettings["LinkToPaperUrl"];

#if (DEBUG)
            string ip = "195.198.185.100";
#else 
                string ip = HttpContext.Current.Request.UserHostAddress;
#endif

            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            byte[] byteArr = Encoding.ASCII.GetBytes(ip + ":" + currentTime + ":");
            string xxx = Convert.ToBase64String(byteArr);
            string chk = getMd5Hash(xxx + "DIiopi93rer435grd435mnad");
            byteArr = Encoding.ASCII.GetBytes(chk);
            chk = Convert.ToBase64String(byteArr);
            return link + "?a=" + HttpContext.Current.Server.UrlEncode(xxx) + "&b=" + HttpContext.Current.Server.UrlEncode(chk) + "&p=" + issueDate + "&placard=" + placard;
        }

        public static string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            int i = 0;
            for (i = 0; i <= data.Length - 1; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        //public static string GetReceiptText(string transactionType, double priceIncVat, double vat, string transactionId, bool isAutoGiro)
        public static string GetReceiptText(double priceIncVat, double vat, string transactionId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Kvitto för betalning med kontokort över internet" + "<br>" +
                      "Dagens industri AB" + "<br>" +
                      "113 90 Stockholm" + "<br>" +
                      "Orgnr: 556221-8494" + "<br>" +
                      "www.di.se/tidningen" + "<br><br>");

            if (priceIncVat > -1)
                sb.Append("Pris inkl moms: " + priceIncVat.ToString() + " SEK" + "<br>");

            if (vat > -1)
                sb.Append("Moms: " + vat.ToString() + " SEK" + "<br>");

            sb.Append("Transaktionsdatum: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "<br>");

            if (!string.IsNullOrEmpty(transactionId))
                sb.Append("Transaktionsnummer: " + transactionId + "<br>");

            //sb.Append("Transaktionstyp: " + transactionType + "<br><br>");  //"Kortbetalning" "Fakturabetalning"
            sb.Append("Transaktionstyp: Kortbetalning<br><br>");  //"Kortbetalning" "Fakturabetalning"
            sb.Append("Spara detta kvitto");


            //if (isAutoGiro)
            //{
            //    sb.Append("<br><br>");
            //    sb.Append("<a href='" + ConfigurationManager.AppSettings["pathAutoGiroDoc"] + "' target='_blank'>Länk till medgivandeblankett för autogiro</a>");
            //}

            return sb.ToString();
        }

        public static string GetNewSubsThankYouText(bool isWeekend)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<IMG alt='' src='/Global/Prenumeration/09_Tackbild.jpg'><IMG alt=tack src='/Global/Prenumeration/09pren_Tack.gif'>");

            if (!isWeekend)
                sb.Append("<P>Du kommer att få en bekräftelse på din beställning både via e-post " +
                          "och post med ditt användarnamn och lösenord till DI på nätet. ");

            sb.Append("Vi kommer att höra av oss snart för att kontrollera att allt fungerar som det ska. Kontakta oss gärna " +
                      "om du har några frågor angående din nya prenumeration.<BR><BR><STRONG>Kundtjänst</STRONG><BR>" +
                      "<STRONG>Telefon:</STRONG> 08-573 651 00 <BR><STRONG>E-post:</STRONG> " +
                      "<A href='mailto:pren@di.se'>pren@di.se</A> <BR><STRONG>Öppettider:</STRONG> " +
                      "Vardagar 06:00-18:00, lördag 07:00-12:00</P>");

            return sb.ToString();
        }

        public static string GetDefaultErrMess(string details)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Ett tekniskt fel har uppstått.<br>");
            sb.Append("Var god kontaka kundtjänst på tel 08-573 651 00 för att reda ut problemet.");

            if (!string.IsNullOrEmpty(details))
                sb.Append("<br><br>Detaljerat felmeddelande: " + details);

            return sb.ToString();
        }


        #endregion

        #region RC4 Encrypt/Decrypt
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::: :::
        //::: This script performs 'RC4' Stream Encryption :::
        //::: (Based on what is widely thought to be RSA's RC4 :::
        //::: algorithm. It produces output streams that are identical :::
        //::: to the commercial products) :::
        //::: :::
        //::: This script is Copyright © 1999 by Mike Shaffer :::
        //::: ALL RIGHTS RESERVED WORLDWIDE :::
        //::: :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        private static int[] sbox = new int[256];
        private static int[] key = new int[256];

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::: This routine called by EnDeCrypt function. Initializes the :::
        //::: sbox and the key array) :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static void RC4Initialize(string strPwd)
        {
            int tempSwap = 0;
            int a = 0;
            int b = 0;
            int intLength = 0;

            intLength = strPwd.Length;
            for (a = 0; a <= 255; a++)
            {
                key[a] = (int)Encoding.ASCII.GetBytes(strPwd.Substring((a % intLength), 1))[0];
                sbox[a] = a;
            }

            b = 0;
            for (a = 0; a <= 255; a++)
            {
                b = (b + sbox[a] + key[a]) % 256;
                tempSwap = sbox[a];
                sbox[a] = sbox[b];
                sbox[b] = tempSwap;

            }
        }

        public static string RC4EnDeCrypt(string plaintxt, string psw)
        {
            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            //::: This routine does all the work. Call it both to ENcrypt :::
            //::: and to DEcrypt your data. :::
            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            int temp = 0;
            int a = 0;
            int i = 0;
            int j = 0;
            int k = 0;
            string cipher = "";

            RC4Initialize(psw);

            for (a = 0; a <= plaintxt.Length - 1; a++)
            {
                i = (i + 1) % 256;
                j = (j + sbox[i]) % 256;
                temp = sbox[i];
                sbox[i] = sbox[j];
                sbox[j] = temp;

                k = sbox[(sbox[i] + sbox[j]) % 256];

                //cipherby = (int)Encoding.ASCII.GetBytes(plaintxt.Substring(a, 1))[0] ^ k;
                //cipher += Convert.ToChar(cipherby);

                int cipherby = Microsoft.VisualBasic.Strings.Asc(plaintxt.Substring(a, 1)) ^ k;
                cipher += Microsoft.VisualBasic.Strings.Chr(cipherby);
            }

            return cipher;
        }


        #endregion

        #region Various

        public static Control GetPostBackControl(Page page)
        {
            Control control = null;

            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        public static bool ApsisMailerIsInTestMode
        {
            get
            {
                bool bo = false;
                bool.TryParse(ConfigurationManager.AppSettings["apsisInTestMode"].ToLower(), out bo);
                return bo;
            }
        }


        public static Int32 TryParse(string parseThis)
        {
            Int32 tmpInt;

            if (Int32.TryParse(parseThis, out tmpInt))
                return tmpInt;

            return -1;
        }

        public static void SendMail(string mailFrom, string mailTo, string subject, string body, bool isHtml)
        {
            if (!IsValidEmail(mailFrom) || !IsValidEmail(mailTo))
                return;
            
            string result = "OK";

            try
            {
                MailMessage message = new MailMessage(mailFrom, mailTo)
                {
                    IsBodyHtml = isHtml,
                    Subject = subject,
                    Body = body
                };
                SmtpClient smtp = new SmtpClient();
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                //throw;
            }
            finally
            {
                string ip = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "not available";

                //logg email
                SqlParameter[] sqlParameters = new SqlParameter[] {
                    new SqlParameter("@ToAddress", mailTo),
                    new SqlParameter("@FromAddress", mailFrom),
                    new SqlParameter("@Subject", subject),
                    new SqlParameter("@Body", body),
                    new SqlParameter("@IsHtml", isHtml),
                    new SqlParameter("@IP", ip), 
                    new SqlParameter("@Result", result)};

                SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "LogEmail", sqlParameters);
            }
        }

        public static void SendMailWithVppFile(MailMessage message, string vppFilePath)
        {
            vppFilePath = HttpUtility.UrlDecode(vppFilePath);
            UnifiedFile uf = System.Web.Hosting.HostingEnvironment.VirtualPathProvider.GetFile(vppFilePath) as UnifiedFile;
            Attachment attachment = new Attachment(uf.LocalPath);
            attachment.Name = uf.Name; //+ uf.Extension;
            message.Attachments.Add(attachment);

            SendMail(message);
        }

        public static void SendMail(MailMessage message)
        {
            string result = "OK";

            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                throw;
            }
            finally
            {
                string ip = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "not available";

                //logg email
                SqlParameter[] sqlParameters = new SqlParameter[] {
                    new SqlParameter("@ToAddress", message.To.ToString()),
                    new SqlParameter("@FromAddress", message.From.ToString()),
                    new SqlParameter("@Subject", message.Subject),
                    new SqlParameter("@Body", message.Body),
                    new SqlParameter("@IsHtml", message.IsBodyHtml),
                    new SqlParameter("@IP", ip), 
                    new SqlParameter("@Result", result)};

                SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "LogEmail", sqlParameters);
            }
        }


        public static void SendStaffMailFailedSaveSubs(string err, Subscription sub, DiPlusSubscription plusSub)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Meddelande skickat från www.dagensindustri.se<br>");
            sb.Append("Följande prenumeration kunde inte sparas från webben.<hr>");
            sb.Append("<b>Fel:</b><br>");
            sb.Append(err);
            sb.Append("<hr><b>Prenumerationsinformation:</b><br><br>");
            sb.Append(sub.ToString());
            
            if (plusSub != null)
            {
                sb.Append("<hr>Kunden ska betala genom <b>autodraging på kontokort</b>. Ta hjälp av tekniker för att lägga upp betalningen.<br>");
                sb.Append(plusSub.ToString());
            }

            SendMail(GetAppsettingsValue("mailPrenFelDiSe"),
                        GetAppsettingsValue("mailPrenFelDiSe"),
                        "Prenumerantion kunde inte sparats i Cirix",
                        sb.ToString(),
                        true);
        }


        public static string SendStaffMailFailedSaveSubs(Subscription sub, Subscriptions.AddCustAndSub.AddCustAndSubReturnObject ret)
        {
            var sb = new StringBuilder();
            //sb.Append("Meddelande skickat från ws.dagensindustri.se<br>");
            sb.Append("Följande prenumeration kunde inte sparas från webben.<hr>");

            if (ret.HasMessages)
            {
                sb.Append("<b>Meddelande till personal:</b>");
            
                sb.Append("<ul>");
                foreach (var mess in ret.Messages)
                    sb.Append("<li>" + mess.MessageSweStaff + "</li>");
            
                sb.Append("</ul>");            
            

                sb.Append("<hr><b>Kunden har fått följande information:</b>");
                
                sb.Append("<ul>");
                foreach (var mess in ret.Messages)
                    sb.Append("<li>" + mess.MessageCustomer + "</li>");

                sb.Append("</ul>");
            }

            sb.Append("<br>");
            sb.Append("<hr><b>Prenumerationsinformation:</b><br><br>");
            sb.Append(sub.ToString());


            SendMail(GetAppsettingsValue("mailPrenFelDiSe"),
                    GetAppsettingsValue("mailPrenFelDiSe"),
                    "Prenumerantion kunde inte sparats i Cirix",
                    sb.ToString(),
                    true);

            return sb.ToString();
        }


        public static string GetAppsettingsValue(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
                return ConfigurationManager.AppSettings[key].ToString();

            new Logger("No appsetting found for key '" + key + "'. No defaultvalue, return string.empty");

            return string.Empty;
        }

        //can probably be composed in one expression but is 
        //easier to understand and work with separated like this
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.ToLower();

            //not valid chars
            if (Regex.IsMatch(email, @"[^a-z0-9_\-\.\@]"))
                return false;

            //valid before @
            if (!Regex.IsMatch(email, @"^([_a-z0-9-])+(\.[_a-z0-9-]+)*@"))
                return false;

            //one @
            if (Regex.Matches(email, @"\@").Count != 1)
                return false;

            //valid after @
            if (!Regex.IsMatch(email, @"\@([a-z0-9]+[\-\.]?)+"))
                return false;

            //find (..) (.-) (-.) (--) after @
            if (Regex.IsMatch(email, @"\@(.)*(\.\.|\.\-|\-\.|\-\-){1}"))
                return false;

            //valid ending (x.xx) (x.xxx) (x.xxxx)
            if (!Regex.IsMatch(email, @"([a-z0-9]{1,}\.([a-z]{2,4}))$"))
                return false;


            return true;
        }

        public static bool IsValidSweZipCode(string zip)
        {
            if (string.IsNullOrEmpty(zip))
                return false;

            if (!IsNumeric(zip))
                return false;

            if (zip.Length != 5)
                return false;

            return true;
        }

        public static bool IsValidDate(string date)
        {
            DateTime dt;
            return DateTime.TryParse(date, out dt);
        }


        public static string GetAttributeDescription(this Enum currentEnum)
        {
            string description = String.Empty;
            DescriptionAttribute da;

            FieldInfo fi = currentEnum.GetType().GetField(currentEnum.ToString());
            da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
            if (da != null)
                description = da.Description;
            else
                description = currentEnum.ToString();

            return description;
        }

        public static bool IsNumeric(string s)
        {
            Int64 i = 0;
            return Int64.TryParse(s, out i);
        }

        public static bool IsGuid(string str)
        {
            try
            {
                Guid g = new Guid(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get userId with provided email address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns>userId = one active subscriber, -1 = no user with matching email and password, -2 = user has more than 1 active subscription, -3 = error</returns>
        public static string GetUserIdByEmail(string emailAddress)
        {
            SqlDataReader DR = null;
            try
            {
                DR = SqlHelper.ExecuteReader("DisePren", "GetUserIdByEmail", new SqlParameter("@mail", emailAddress));
                if (DR.Read())
                {
                    if (DR["result"] != System.DBNull.Value)
                    {
                        if (DR["result"].ToString() == "0")
                            return DR["userid"].ToString();
                        else
                            return DR["result"].ToString();
                    }

                    return "-3";
                }

                return "-3";
            }
            catch (Exception ex)
            {
                new Logger("GetUserIdByEmail() - failed", ex.ToString());
                return "-3";
            }
            finally
            {
                if (DR != null)
                    DR.Close();
            }
        }

        /// <summary>
        /// Format the phonenumber so that proper spaces is added to the phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static string FormatPhoneNumber(string phoneNumber)
        {
            string ret = phoneNumber;
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                string prefix = string.Empty;
                string numberWithoutPrefix = phoneNumber;

                if (phoneNumber.Contains("-"))
                {
                    prefix = phoneNumber.Substring(0, phoneNumber.LastIndexOf("-"));
                    numberWithoutPrefix = phoneNumber.Substring(phoneNumber.LastIndexOf("-") + 1);
                }

                numberWithoutPrefix = numberWithoutPrefix.Replace(" ", "");
                Int64 number = 0;
                if (Int64.TryParse(numberWithoutPrefix, out number))
                {
                    if (numberWithoutPrefix.Length == 5)
                        numberWithoutPrefix = number.ToString("### ##");
                    else if (numberWithoutPrefix.Length == 6)
                        numberWithoutPrefix = number.ToString("## ## ##");
                    else if (numberWithoutPrefix.Length == 7)
                        numberWithoutPrefix = number.ToString("### ## ##");
                    else if (numberWithoutPrefix.Length == 8)
                        numberWithoutPrefix = number.ToString("### ### ##");

                    ret = !string.IsNullOrEmpty(prefix)
                                    ? string.Format("{0}-{1}", prefix, numberWithoutPrefix)
                                    : numberWithoutPrefix;
                }
            }

            return ret;
        }

        /// <summary>
        /// Format zipcode to be of format ### ##.
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public static string FormatZipCode(string zipCode)
        {
            int zipCodeNumeric = TryParse(zipCode.Trim().Replace(" ", ""));
            return zipCodeNumeric != -1 ? zipCodeNumeric.ToString("### ##") : zipCode;
        }

        /// <summary>
        /// Format social security number to match Cirix requirements
        /// </summary>
        /// <param name="socialSecurityNo"></param>
        /// <returns></returns>
        public static string FormatSocialSecurityNo(string socialSecurityNo)
        {
            string socSec = socialSecurityNo;

            if (string.IsNullOrEmpty(socSec))
                return null;

            socSec = socialSecurityNo.Trim().Replace("-", "");
            socSec = socSec.Replace("/", "");
            socSec = socSec.Replace(@"\", "");
            socSec = socSec.Replace(" ", "");

            //if (formattedSocialSecurityNo.Length < 12)
            if (socSec.Length < 8)
                return null;

            string datePart = socSec.Substring(0, 8);
            DateTime parsedDate;
            string[] formats = { "yyyyMMdd" };
            if (!DateTime.TryParseExact(datePart, formats, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedDate))
                return null;

            if (socSec.Length > 8)
            {
                string codePart = socSec.Substring(8);
                int i = 0;

                if (!int.TryParse(codePart, out i))
                    return null;

                if (i.ToString().Length != 4)
                    return null;
            }

            return socSec;
        }

        /// <summary>
        ///  Format phone number to match Cirix requirements. 
        ///  It has to start with a country code, have a max no of digits and only contain digits and a '+'.
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="maxNoOfDigits"></param>
        /// <param name="doMobileCheck"></param>
        /// <returns></returns>
        public static string FormatPhoneNumber(string phoneNo, int maxNoOfDigits, bool doMobileCheck)
        {
            var num = string.Empty;
            if (!string.IsNullOrEmpty(phoneNo))
            {
                num = phoneNo.Trim();
                num = num.Replace("-", "");
                num = num.Replace("/", "");
                num = num.Replace(@"\", "");
                num = num.Replace(" ", "");

                if (!num.StartsWith("+"))
                {
                    if (num.StartsWith("00"))
                    {
                        num = string.Format("+{0}", num.Substring(2));
                    }
                    else if (num.StartsWith("0"))
                    {
                        num = num.Substring(1);
                        num = string.Format("+46{0}", num);
                    }
                }

                if (doMobileCheck && num.StartsWith("+46"))
                {
                    string phoneNoCountryCode = num.Substring(3);

                    //Check if mobile number without country code, starts with 1 or 7 (start digits of a mobile number)
                    if (!phoneNoCountryCode.StartsWith("1") && !phoneNoCountryCode.StartsWith("7"))
                        return null;
                }

                //Check if phone number only has digits (excluding + in country code) and it doesn't exceed max no of digits
                Int64 parsedPhoneNo;
                if (!Int64.TryParse(num.Substring(1), out parsedPhoneNo) || num.Length > maxNoOfDigits)
                    num = null;
            }

            return num;
        }

        public static string AddZeroToLengthOne(string val)
        {
            if (val.Length == 1)
                return "0" + val;

            return val;
        }

        public static string AddZeroToLengthOne(int val)
        {
            if (val < 10)
                return "0" + val.ToString();

            return val.ToString();
        }

        public static string REC(string val, bool toUpper)
        {
            if (string.IsNullOrEmpty(val))
                return string.Empty;

            return toUpper ? REC(val.ToUpper()) : REC(val);
        }

        public static string REC(string val)
        {
            if (string.IsNullOrEmpty(val))
                return string.Empty;

            val = val.Replace("<", "");
            val = val.Replace(">", "");
            val = val.Replace("'", "");
            val = val.Replace(";", "");
            val = val.Trim();

            return val;
        }

        public static string MD5EncodeStr(string s)
        {
            try
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                Encoding encoder = Encoding.GetEncoding("ISO-8859-1");
                byte[] hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(s));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("MD5EncodeStr() failed", ex.ToString());
                return string.Empty;
            }
        }

        public static bool IsLongerThanMaxLength(string s, int maxLen)
        {
            if (s.Length > maxLen)
                return true;

            return false;
        }


        public static string GetNetsCardPayStatus(string status, string statusCode)
        {
            if (status == null)
                return string.Empty;

            if (status == "A")
                return "OK";

            if (status == "P")
                return "Påbörjad";

            if (status == "E")
                return GetNetsErrMess(statusCode);

            return string.Empty;
        }

        private static string GetNetsErrMess(string statusCode)
        {
            foreach (var sp in NetsErros())
            {
                if (sp.S1 == statusCode)
                    return sp.S2;
            }

            return "Okänt fel (statuskod: " + statusCode + ")";
        }

        private static List<StringPair> NetsErros()
        {
            var list = new List<StringPair>();
            list.Add(new StringPair("-1", "Betalningsfakta hittades inte för angivet transaktionsId."));
            list.Add(new StringPair("14", "Invalid card number."));
            list.Add(new StringPair("25", "Transaction not found."));
            list.Add(new StringPair("30", "'KID invalid' or 'Field missing PAN'."));
            list.Add(new StringPair("84", "Original transaction rejected."));
            list.Add(new StringPair("86", "Transaction already reversed."));
            list.Add(new StringPair("96", "Internal failure."));
            list.Add(new StringPair("97", "No transaction"));
            list.Add(new StringPair("99", "Several errors can result in this code. Refer to the Response text for details."));
            list.Add(new StringPair("MZ", "Denied by 3D Secure."));
            list.Add(new StringPair("01", "Refused by Issuer. Contact Issuer."));
            list.Add(new StringPair("02", "Refused by Issuer. Contact Issuer."));
            list.Add(new StringPair("03", "Refused by Issuer because of Invalid merchant."));
            list.Add(new StringPair("04", "Refused by Issuer."));
            list.Add(new StringPair("05", "Refused by Issuer."));
            list.Add(new StringPair("06", "Refused by Issuer."));
            list.Add(new StringPair("07", "Refused by Issuer."));
            list.Add(new StringPair("12", "invalid transaction."));
            list.Add(new StringPair("13", "Refused by Issuer because of invalid amount."));
            list.Add(new StringPair("14", "Refused by Issuer."));
            list.Add(new StringPair("19", "Refused by Issuer. Try again."));
            list.Add(new StringPair("30", "Refused by Issuer because of format error."));
            list.Add(new StringPair("31", "Issuer could not be reached, contact Nets support."));
            list.Add(new StringPair("33", "Expired card."));
            list.Add(new StringPair("36", "Restricted card."));
            list.Add(new StringPair("41", "Refused by Issuer. Contact Issuer."));
            list.Add(new StringPair("43", "Refused by Issuer. Contact Issuer."));
            list.Add(new StringPair("51", "Refused by Issuer. Contact Issuer."));
            list.Add(new StringPair("52", "Refused by Issuer because of no checking account."));
            list.Add(new StringPair("54", "Refused by Issuer because of expired card."));
            list.Add(new StringPair("56", "Refused by Issuer because of no card record."));
            list.Add(new StringPair("57", "Refused by Issuer of transaction not permitted."));
            list.Add(new StringPair("58", "Refused by Issuer of transaction not permitted."));
            list.Add(new StringPair("59", "Refused by Issuer. Contact Issuer."));
            list.Add(new StringPair("61", "Refused by Issuer. Contact Issuer."));
            list.Add(new StringPair("62", "Refused by Issuer. Contact Issuer."));
            list.Add(new StringPair("63", "Refused by Issuer. Contact Issuer."));
            list.Add(new StringPair("65", "Refused by Issuer. Contact Issuer."));
            list.Add(new StringPair("68", "Refused by Issuer because response received to late."));
            list.Add(new StringPair("78", "Refused by Issuer."));
            list.Add(new StringPair("79", "Refused by Issuer."));
            list.Add(new StringPair("80", "Refused by Issuer."));
            list.Add(new StringPair("91", "Refused by Issuer because Issuer is temporarily inoperative."));
            list.Add(new StringPair("92", "Refused by Issuer because Issuer is temporarily inoperative."));
            list.Add(new StringPair("93", "Refused by Issuer. Contact Issuer."));
            list.Add(new StringPair("96", "Refused by Issuer because of system malfunction."));
            list.Add(new StringPair("C9", "Refused by Issuer."));
            list.Add(new StringPair("N0", "Refused by Issuer."));
            list.Add(new StringPair("N7", "Refused by Issuer because of invalid 3 digit code from back of credit card."));
            list.Add(new StringPair("P1", "Refused by Issuer."));
            list.Add(new StringPair("P9", "Refused by Issuer."));
            list.Add(new StringPair("T3", "Refused by Issuer."));
            list.Add(new StringPair("T8", "Refused by Issuer."));
            list.Add(new StringPair("01", "Timeout experienced."));
            list.Add(new StringPair("06", "Technical reversal was attempted, due to timeout, but the reversal failed as well."));
            list.Add(new StringPair("30", "Value missing in required field, often resulting in the response text 'Ntoa: Taglen err'. A rare error that can occur if Visa or MasterCard experiences problems with their 3D Secure system."));

            return list;
        }


        //public static string GetAurigaCardPayStatus(string status, string statusCode)
        //{
        //    if (status == null)
        //        return string.Empty;

        //    if (status == "A")
        //        return "OK";

        //    if (status == "P")
        //        return "Påbörjad";

        //    if (status == "E")
        //    {
        //        if (IsNumeric(statusCode))
        //        {
        //            string err = TryGetItemFromArr(GetAurigaMessArr(status), int.Parse(statusCode));
        //            return (!string.IsNullOrEmpty(err)) ? err : "Okänt fel (statuskod: " + statusCode + ")";
        //        }

        //        if (!IsNumeric(statusCode) && !string.IsNullOrEmpty(statusCode))
        //            return "Fel - statuskoden är inte ett numeriskt tal (statuskod: " + statusCode + ")";

        //        return "Okänt fel (statuskod saknas)";
        //    }

        //    return string.Empty;
        //}

        //private static string TryGetItemFromArr(string[] arr, int id)
        //{
        //    try
        //    {
        //        return arr[id];
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}

        //private static string[] GetAurigaMessArr(string status)
        //{
        //    string[] arr;

        //    if (status == "A")
        //    {
        //        arr = new string[12];
        //        arr[0] = "Betalningen/Orderläggningen är genomförd: Betald.";
        //        arr[1] = "Nekad/Avbruten. Betalningen har nekats eller avbrutits.";
        //        arr[2] = "Pågående. Betalningen väntar på svar från banken.";
        //        arr[3] = "Annullerad. Kortbetalningen har makulerats av Butiken, innan inlösen.";
        //        arr[4] = "Påbörjad. Betalningen är initierad av köparen.";
        //        arr[6] = "Auktoriserad. Kortbetalningen är auktoriserad; väntar på konfirmering och inlösen.";
        //        arr[7] = "Inlösen misslyckad. Det gick inte att lösa in kortbetalningen.";
        //        arr[8] = "Inlösen pågående. Inlösen av kortbetalningen pågår just nu.";
        //        arr[9] = "Bekräftad. Kortbetalningen är konfirmerad och kommer att inlösas.";
        //        arr[11] = "Skickad till bank eller Svea Ekonomi. Gäller endast betalmetod INVOICE,SVEAWPSE,SVEADBSE";
        //    }
        //    else
        //    {
        //        arr = new string[111];
        //        arr[3] = "Ett obligatoriskt fält saknas eller har felaktigt format.";
        //        arr[4] = "Felaktigt Butiksid (Merchant_id)";
        //        arr[6] = "Felaktigt Belopp (Amount eller VAT, varuvärde eller moms)";
        //        arr[7] = "Problem vid Auktorisation";

        //        #region remaining message id's
        //        arr[11] = "Gick ej att kontakta banken.";
        //        arr[12] = "Fel vid validering av mottagen MAC.";
        //        arr[13] = "För stort belopp (Amount, varuvärde)";
        //        arr[14] = "Fel format på datum/tid (Purchase_date, ska vara exakt 12 siffror i formatet ååååmmddttmm).";
        //        arr[15] = "Felaktigt Inköpsdatum (Purchase_date)";
        //        arr[17] = "Otillåtet betalsätt (Payment_method). Betalsätt finns inte konfigurerat för Butiken. Eller betalsätt stöds inte av Version = 2";
        //        arr[18] = "Kortbetalning: Nekad pga fel vid auktorisation eller ingen kontakt med bank";
        //        arr[19] = "Kortbetalning: Köp nekat hos bank (kortets giltighetstid har gått ut), kontakta bank.";
        //        arr[20] = "Kortbetalning: Köp nekat hos bank, kontakta bank.";
        //        arr[21] = "Land för kortutgivande bank tillåts ej";
        //        arr[22] = "Transaktionens riskbedömning överskrider tillåtet värde";
        //        arr[23] = "Kortbetalning: kortet spärrat/inaktiverat hos kortutgivande bank. Tex borttappat, stulet, för många felaktiga PIN-inmatningar, etc.";
        //        arr[24] = "Felaktig Request_type i Orderadministration-anrop.";
        //        arr[25] = "För högt belopp: inte tillräckligt med saldo, kortutgivande bank tillåter inte detta belopp på detta kort.";
        //        arr[26] = "Misstänkt bedrägeri";
        //        arr[27] = "Köpesumman (Amount) måste vara större än noll.";
        //        arr[28] = "Nekad pga för många betalningsförsök";
        //        arr[30] = "Nekad pga time-out, inget svar från bank";
        //        arr[31] = "Köpet avbrutet (av köparen).";
        //        arr[32] = "Fel vid anrop, transaktion redan registrerad och betald. Detta Customer_refno finns redan registrerat på annan transaktion.";
        //        arr[33] = "eKöp: Tekniskt fel vid kommunikation med PlusGirot/Nordea. Var god försök senare.";
        //        arr[34] = "Felaktigt angivet mottagarkonto (To_accnt).";
        //        arr[35] = "Felaktigt angivet avsändarkonto (From_accnt).";
        //        arr[36] = "eKöp: Angivet avsändarkonto (From_accnt) temporärt spärrat.";
        //        arr[39] = "eKöp: Felaktigt angivet certifikat (Cert).";
        //        arr[40] = "eKöp: Kontot ej anslutet till tjänsten.";
        //        arr[41] = "Betalvalsidan/Kortbetalsidan: Butiken ej ansluten till tjänsten";
        //        arr[42] = "Felaktigt värde på parametern Auth_null. Ska vara YES eller NO.";
        //        arr[43] = "Felaktigt värde på parametern Capture_now. Ska vara YES eller NO.";
        //        arr[45] = "Vid Orderadministration: Transaktionens status tillåter inte operationen.";
        //        arr[48] = "Vid Orderadministration: Transaktionen finns inte.";
        //        arr[50] = "Kortbetalning: Butik inte konfigurerad för valuta/korttyp kombination...(Currency/Card_type). Korttypen inte upplagd, felaktig Korttyp eller Kortutgivande bank nekar kortet.";
        //        arr[51] = "Kortbetalning: Ogiltigt Exp_date (ska vara exakt 4 siffror i formatet mmåå).";
        //        arr[52] = "Kortbetalning: Ogiltigt Card_num (kortnummer).";
        //        arr[53] = "Kortbetalning: Felaktigt format på kortets kontrollsiffra Cvx2 (3 eller 4 siffror). Eller felaktig kontrollsiffra (Cvx2) till kortet.";
        //        arr[54] = "Felaktig track2";
        //        arr[55] = "Felaktig MOTOmetod. Ska vara MAIL eller PHONE";
        //        arr[56] = "Köpet har avbrutits av köparen eller nekats av banken";
        //        arr[57] = "Felaktigt Customer_refno";
        //        arr[58] = "Felaktig Version, fel format på versionsparametern (ska vara Version=2).";
        //        arr[59] = "Felaktigt Parametervärde för Card_fee. Giltiga värden är YES/NO";
        //        arr[65] = "Transaktion redan registrerad och väntar på svar från banken.";
        //        arr[67] = "Kunde inte utföra krediteringen.";
        //        arr[69] = "Tekniskt fel Betalväxeln.";
        //        arr[70] = "Vid Orderadministration: Funktionen stöds inte.";
        //        arr[71] = "Fel format eller storlek på SvarsURL:en (Response_URL)";
        //        arr[72] = "Felaktig Valutakod (Currency)";
        //        arr[73] = "Felaktig Språkkod (Language)";
        //        arr[75] = "Felaktig Kommentar (Comment)";
        //        arr[76] = "Felaktig varubeskrivning (Goods_description)";
        //        arr[77] = "Customer_refno matchar inte Transaction_id";
        //        arr[78] = "Kortbetalning: Kunde inte utföra Authorization Reversal (Request_type=AuthRev)";
        //        arr[79] = "Kortbetalning: Kunde inte göra inlösen (capture).";
        //        arr[81] = "Kortbetalning: Misslyckad 3-D Secure-identifiering (för Verified by Visa eller SecureCode). Kortauktorisation genomförs inte.";
        //        arr[82] = "Kortbetalning: 3-D Secure-identifiering nekad pga. timeout (för Verified by Visa eller SecureCode). Inträffar när kortinnehavaren inte identifierar sig inom ca. 5 min efter transaktionens start. Kortauktorisationen genomförs inte.";
        //        arr[90] = "Abonnemang (Transaction_id) har upphört.";
        //        arr[91] = "Abonnemang (Transaction_id) hittades inte.";
        //        arr[92] = "Abonnemang (Transaction_id) inte inlöst, har inte status Betald.";
        //        arr[93] = "Betalvalsida: Fel format eller storlek på AvbrytURL:en (Cancel_URL)";
        //        arr[95] = "Fakturaköp: Felaktigt OCR-nummer, t ex felaktig checksiffra";
        //        arr[96] = "Felaktig längd för fakturaparametrarna";
        //        arr[97] = "Felaktig garanti parameter";
        //        arr[98] = "Felaktigt postnummer";
        //        arr[110] = "Fakturaköp: Ogiltigt personnummer eller organisationsnummer";
        //        #endregion
        //    }

        //    return arr;
        //}


        public static bool DateHasBeenSet(DateTime dt)
        {
            if (dt == null)
                return false;

            if (dt > new DateTime(1970, 1, 1))
                return true;

            return false;
        }

        //todo - extend check of mobile phone number
        public static bool IsValidSwePhoneNum(string num)
        {
            if (string.IsNullOrEmpty(num) || num.Length < 7 || num.Length > 20 || !num.StartsWith("+46"))
                return false;

            return true;
        }

        public static string RemoveWwwFromUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
                return url.ToLower().Replace("www.", "");

            return url;
        }

        /// <summary>
        /// Returns splitted phonenumber where string[0]=country code and string[1]=phonenumber without country code
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static string[] GetSeparatedCountryCodePhoneNumber(string phoneNumber)
        {
            const string swedenCountryCode = "46";
            string[] phoneInfo = new string[2];
            string part1 = phoneNumber.Substring(0, 2);
            string part2 = phoneNumber.Substring(0, 1);

            if (part1 == "00")
            {
                phoneInfo[0] = phoneNumber.Substring(2, 2);
                phoneInfo[1] = string.Format("{0}", phoneNumber.Substring(4));
            }
            else if (part1 == "07")
            {
                phoneInfo[0] = swedenCountryCode;
                phoneInfo[1] = phoneNumber;
            }
            else if (part2 == "+")
            {
                phoneInfo[0] = phoneNumber.Substring(1, 2);
                phoneInfo[1] = string.Format("{0}", phoneNumber.Substring(3));
            }
            else if (part2 == "7")
            {
                phoneInfo[0] = swedenCountryCode;
                phoneInfo[1] = string.Format("0{0}", phoneNumber);
            }
            else
            {
                phoneInfo[0] = phoneNumber.Substring(0, 2);
                phoneInfo[1] = string.Format("{0}", phoneNumber.Substring(2));
            }

            return phoneInfo;
        }

        #endregion

        #region Image

        public static void StreamThumbNailImage(String strFile)
        {
            //Check that file exist before streaming
            if (!File.Exists(strFile))
                return;

            try
            {
                System.Drawing.Image orginalimg = System.Drawing.Image.FromFile(strFile);
                System.Drawing.Image thumb = default(System.Drawing.Image);
                IntPtr inp = new IntPtr();
                // Sätt max bredd och höjd här
                //int intDefaultWidth = 150;
                //int intDefaultHeight = 208;
                int width = 0;
                int height = 0;
                int intNewWidth = 0;

                if (HttpContext.Current.Request.QueryString["w"] != null && int.TryParse(HttpContext.Current.Request.QueryString["w"], out intNewWidth))
                {
                    width = intNewWidth;
                    height = orginalimg.Height * intNewWidth / orginalimg.Width;
                }
                else
                {
                    //width = intDefaultWidth;
                    //height = intDefaultHeight;
                    width = orginalimg.Width;
                    height = orginalimg.Height;
                }

                thumb = orginalimg.GetThumbnailImage(width, height, null, inp);
                HttpContext.Current.Response.ContentType = GetContentType(strFile);
                thumb.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                orginalimg.Dispose();
                thumb.Dispose();
            }
            catch (Exception ex)
            {
                new Logger("StreamThumbNailImage(String strFile) - failed", ex.ToString());
            }
            finally
            {
                HttpContext.Current.Response.End();
            }
        }

        public static void StreamImage(String strFile)
        {
            //Check that file exist before streaming
            if (!File.Exists(strFile))
                return;

            BinaryReader imageStream = null;

            try
            {
                imageStream = new BinaryReader(File.OpenRead(strFile));
                HttpContext.Current.Response.Clear();
                // Stream it
                HttpContext.Current.Response.Expires = 0;
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = GetContentType(strFile);

                HttpContext.Current.Response.AddHeader("Content-Length", imageStream.BaseStream.Length.ToString());
                HttpContext.Current.Response.BinaryWrite(imageStream.ReadBytes((int)imageStream.BaseStream.Length));
            }
            catch (Exception ex)
            {
                new Logger("StreamImage(String strFile) - failed", ex.ToString());
            }
            finally
            {
                //Clean up....
                if ((imageStream != null)) imageStream.Close();
                imageStream = null;
                HttpContext.Current.Response.End();
            }
        }

        public static void StreamPdf(String strFile, String attachmentFileName)
        {
            //Check that file exist before streaming
            if (!File.Exists(strFile))
                return;

            BinaryReader pdfStream = null;

            try
            {
                pdfStream = new BinaryReader(File.OpenRead(strFile));
                // Stream it
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Expires = 0;
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/pdf";
                if (!string.IsNullOrEmpty(attachmentFileName))
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + "di" + attachmentFileName + ".pdf");
                HttpContext.Current.Response.AddHeader("Content-Length", pdfStream.BaseStream.Length.ToString());

                HttpContext.Current.Response.BinaryWrite(pdfStream.ReadBytes((int)pdfStream.BaseStream.Length));
            }
            catch (Exception ex)
            {
                new Logger("StreamPdf(String strFile) - failed", HttpContext.Current.Request.UserHostAddress + " PDF: " + attachmentFileName + " " + ex.ToString());
            }
            finally
            {
                if (pdfStream != null)
                {
                    //Clean up....
                    pdfStream.Close();
                    pdfStream = null;
                    HttpContext.Current.Response.End();
                }
            }
        }


        public static string GetContentType(string strFileName)
        {
            switch (strFileName.Substring(strFileName.LastIndexOf(".")))
            {
                case ".gif":
                    return "image/gif";
                case ".jpg":
                    return "image/jpeg";
                default:
                    return string.Empty;
            }
        }

        #endregion Image

        #region DI Online

        public static string TextalkGetLink(string issueDate, string placard, string link)
        {
            if (string.IsNullOrEmpty(link))
                link = ConfigurationManager.AppSettings["LinkToPaperUrl"];

#if (DEBUG)
            string ip = "195.198.185.100";
#else 
            string ip = HttpContext.Current.Request.UserHostAddress;
#endif

            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            byte[] byteArr = Encoding.ASCII.GetBytes(ip + ":" + currentTime + ":");
            string xxx = Convert.ToBase64String(byteArr);
            string chk = GetMd5Hash(xxx + "DIiopi93rer435grd435mnad");
            byteArr = Encoding.ASCII.GetBytes(chk);
            chk = Convert.ToBase64String(byteArr);
            return link + "?a=" + HttpContext.Current.Server.UrlEncode(xxx) + "&b=" + HttpContext.Current.Server.UrlEncode(chk) + "&p=" + issueDate + "&placard=" + placard;
        }

        public static string GetMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            int i = 0;
            for (i = 0; i <= data.Length - 1; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        

        #endregion DI Online
        
        #region Xml
        /// <summary>
        /// Get innertext from an xml node using the XmlDocument
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static string GetXmlNodeText(XmlDocument xmlDoc, string xpath)
        {
            string innerText = string.Empty;
            if (xmlDoc != null && xmlDoc.SelectSingleNode(xpath) != null)
                innerText = xmlDoc.SelectSingleNode(xpath).InnerText;
            return innerText;
        }

        /// <summary>
        /// Get inner text of an xmlNode.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static string GetXmlNodeText(XmlNode xmlNode, string xpath)
        {
            string innerText = string.Empty;
            if (xmlNode != null && xmlNode.SelectSingleNode(xpath) != null)
                innerText = xmlNode.SelectSingleNode(xpath).InnerText;
            return innerText;
        }

        /// <summary>
        /// Give an attribute text from an xml node using the XmlDocument
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="xpath"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string GetXmlAttributeText(XmlDocument xmlDoc, string xpath, string attribute)
        {
            string attributeText = string.Empty;
            if (xmlDoc != null && xmlDoc.SelectSingleNode(xpath) != null)
                attributeText = xmlDoc.SelectSingleNode(xpath).Attributes[attribute].InnerText;
            return attributeText;
        }

        /// <summary>
        /// Give an attribute text from an xml node using the XmlDocument
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xpath"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string GetXmlAttributeText(XmlNode xmlNode, string attribute)
        {
            string attributeText = string.Empty;
            if (xmlNode != null && xmlNode.Attributes[attribute] != null)
                attributeText = xmlNode.Attributes[attribute].InnerText;
            return attributeText;
        }
        #endregion


    }
}
