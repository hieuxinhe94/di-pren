using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using DIClassLib.DbHelpers;
using DIClassLib.OboWebReference;
using System.ServiceModel.Description;
using DIClassLib.DbHandlers;
using System.Xml;


namespace DIClassLib.OneByOne
{
    public static class Obo
    {

        /// <summary>
        /// Set security credentials for One By One's Xml webservice client authentication
        /// </summary>
        /// <returns></returns>
        public static DocumentFactoryService SetUpOneByOneDocumentFactory()
        {
            DocumentFactoryService df = null;
            try
            {
                df = new DocumentFactoryService();
                df.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["OBOUserName"], ConfigurationManager.AppSettings["OBOPassword"]);
            }
            catch (Exception ex)
            {
                new Logger("SetUpOneByOneDocumentFactory() - failed", ex.ToString());
                df = null;
            }

            return df;
        }


        /// <summary>
        /// Set security credentials for One By One's Xml webservice client authentication. 
        /// </summary>
        /// <returns></returns>
        public static DocumentFactoryService SetUpOneByOneDocumentFactory(string username, string passwd)
        {
            DocumentFactoryService df = null;
            
            try
            {
                if (string.IsNullOrEmpty(username))
                    username = ConfigurationManager.AppSettings["OBOUserName"];
                
                if (string.IsNullOrEmpty(passwd))
                    passwd = ConfigurationManager.AppSettings["OBOPassword"];

                df = new DocumentFactoryService();
                df.Credentials = new NetworkCredential(username, passwd);

            }
            catch (Exception ex)
            {
                new Logger("SetUpOneByOneDocumentFactory() - failed", ex.ToString());
                df = null;
            }

            return df;
        }


        public static bool OboSubscribe(long cusno, string unitId, string personId, string linkId)
        {
            string methodName = "OboSubscribe()";
            DocumentFactoryService factory = null;
            
            try
            {
                factory = SetUpOneByOneDocumentFactory(ConfigurationManager.AppSettings["OBOUserNameSubs"], ConfigurationManager.AppSettings["OBOPasswordSubs"]);
                if (factory == null)
                {
                    new Logger(methodName + " failed", "factory == null");
                    return false;
                }

                string docName = ConfigurationManager.AppSettings["OBOSubscribe"];
                RequestParameter[] parms;

                parms = GetSubscribeParameters(cusno, unitId);
                factory.getDocument(docName, parms);

                parms = GetSubscribeParameters(cusno, linkId);
                factory.getDocument(docName, parms);

                SubscriptionController.UpdateCustomerExtraInfo(cusno, unitId, personId, linkId);

                return true;
            }
            catch (Exception ex)
            {
                new Logger(methodName + " failed", ex.ToString());
                return false;
            }

            //return factory.getDocument(documentName, parameters);
        }


        public static bool OboUnsubscribe(long cusno, string unitId, string personId, string linkId)
        {
            string methodName = "OboUnsubscribe()";
            DocumentFactoryService factory = null;

            try
            {
                factory = SetUpOneByOneDocumentFactory(ConfigurationManager.AppSettings["OBOUserNameSubs"], ConfigurationManager.AppSettings["OBOPasswordSubs"]);
                if (factory == null)
                {
                    new Logger(methodName + " failed", "factory == null");
                    return false;
                }

                string docName = ConfigurationManager.AppSettings["OBOUnsubscribe"];
                RequestParameter[] parms;

                parms = GetSubscribeParameters(cusno, unitId);
                factory.getDocument(docName, parms);

                parms = GetSubscribeParameters(cusno, linkId);
                factory.getDocument(docName, parms);

                SubscriptionController.UpdateCustomerExtraInfo(cusno, "", "", "");

                return true;
            }
            catch (Exception ex)
            {
                new Logger(methodName + " failed", ex.ToString());
                return false;
            }
        }


        private static RequestParameter[] GetSubscribeParameters(long cusno, string xtra)
        {
            RequestParameter[] prms = new RequestParameter[2];

            for (int i = 0; i < prms.Length; i++)
                prms[i] = new RequestParameter();

            prms[0].name = "parId";
            prms[0].value = xtra;

            prms[1].name = "customerId";
            prms[1].value = cusno.ToString();

            return prms;
        }


        public static void OboUnsubscribe(long cusno)
        {
            List<string> xtra = SubscriptionController.GetCustomerXtraFields(cusno);
            if (!string.IsNullOrEmpty(xtra[0] + xtra[1] + xtra[2]))
                OboUnsubscribe(cusno, xtra[0], xtra[1], xtra[2]);
        }

        public static Person GetPerson(string personalNumber) 
        {
            try
            {
                var factoryService = SetUpOneByOneDocumentFactory(ConfigurationManager.AppSettings["OBOPersonSearchUserName"], ConfigurationManager.AppSettings["OBOPersonSearchPassword"]); 
                RequestParameter[] parameters = { new RequestParameter() { name = "personalNumber", value = personalNumber } };

                var parData = factoryService.getDocument("PersonPhone.xml", parameters);
                var parXml = new XmlDocument();
                parXml.LoadXml(parData);

                var documentElement = parXml.DocumentElement;

                if (documentElement != null)
                    return new Person(documentElement);

                return null;
            }
            catch (Exception ex)
            {
                new Logger("Obo.GetPerson() for personalNumber '" + personalNumber + "' failed", ex.ToString());
                return null;
            }
        }

        public static Company GetCompany(string companyNumber)
        {
            try
            {
                var factoryService = SetUpOneByOneDocumentFactory(ConfigurationManager.AppSettings["OBOPersonSearchUserName"], ConfigurationManager.AppSettings["OBOPersonSearchPassword"]);
                RequestParameter[] parameters = { new RequestParameter() { name = "companyNumber", value = companyNumber } };

                var parData = factoryService.getDocument("S-W-4.xml", parameters);
                var parXml = new XmlDocument();
                parXml.LoadXml(parData);

                var hitNode = parXml.DocumentElement.SelectSingleNode("//Hit[Status = 'Aktiv' and TypeCode = 'TY10']");

                if (hitNode == null)
                {
                    hitNode = parXml.DocumentElement.SelectSingleNode("//Hit[Status = 'Aktiv']");
                }

                if (hitNode != null)
                    return new Company(hitNode);

                return null;
            }
            catch (Exception ex)
            {
                new Logger("Obo.GetCompany() for companyNumber '" + companyNumber + "' failed", ex.ToString());
                return null;
            }
        }
    }

    public class Person 
    {
        public Person(XmlElement documentElement) 
        {
            var namesNode = documentElement.SelectSingleNode("Names");
            var addressNode = documentElement.SelectSingleNode("RegistrationAddress");
            var phoneNodes = documentElement.SelectNodes("Phone"); //Can be multiple phone nodes

            if (namesNode != null)
            {
                FirstNames = namesNode.SelectSingleNode("FirstNames").InnerText;
                GivenNames = namesNode.SelectSingleNode("GivenNames").InnerText;
                LastNames = namesNode.SelectSingleNode("LastNames").InnerText;
            }
            if (addressNode != null)
            {
                StreetAddressRaw = addressNode.SelectSingleNode("StreetAddress").InnerText;
                ZipCode = addressNode.SelectSingleNode("ZipCode").InnerText;
                City = addressNode.SelectSingleNode("City").InnerText;
            }
            if (phoneNodes != null)
            { 
                foreach(XmlNode phoneNode in phoneNodes)
                {
                    //We are only interested in mobile phone number
                    var typeNode = phoneNode.SelectSingleNode("Type");
                    if (typeNode != null && typeNode.InnerText == "MOB")
                    {
                        PhoneMobile = phoneNode.SelectSingleNode("AreaCode").InnerText +
                                        phoneNode.SelectSingleNode("LeasedLineNumber").InnerText;
                    }
                }
            }
        }

        public string FirstNames { get; set; }
        public string GivenNames { get; set; }
        public string LastNames { get; set; }
        public string StreetAddressRaw { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string PhoneMobile { get; set; }
    }

    public class Company
    {
        public Company(XmlNode hitNode)
        {
            Name = hitNode.SelectSingleNode("Name").InnerText;
            StreetAddressRaw = hitNode.SelectSingleNode("Address").InnerText;
            ZipCode = hitNode.SelectSingleNode("Zipcode").InnerText;
            City = hitNode.SelectSingleNode("City").InnerText;
            CompanyNumber = hitNode.SelectSingleNode("CompanyNumber").InnerText;            
        }

        public string Name { get; set; }
        public string StreetAddressRaw { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string CompanyNumber { get; set; }
    }
}
