using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

using DagensIndustri.Tools.Admin.CustomerInfo;
using DagensIndustri.Tools.Telemarketing;

using DIClassLib.BonnierDigital;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;

namespace DagensIndustri.Handlers.Tools
{
    public class AddSubscriptionsHandler : IHttpHandler
    {
        private const string MethodName = "methodName";
        private const string FirstName = "firstname";
        private const string LastName = "lastname";
        private const string Company = "company";
        private const string MobilePhone = "mobilephone";
        private const string Email = "email";
        private const string Cusno = "cusno";

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod.ToLower() == "post")
            {
                try
                {
                    var methodName = context.Request.Params[MethodName];
                    switch (methodName.ToLower())
                    {
                        case "checkemailatserviceplus":
                            CheckEmailAtServicePlus(context);
                            return;
                        case "findcustomer":
                            FindCustomer(context);
                            return;
                        case "listcustomersubs":
                            ListCustomerSubs(context);
                            return;
                        case "getproducts":
                            GetTelemarketingProducts(context);
                            return;

                    }
                    context.Response.StatusDescription = string.Format("Method '{0}' isn't supported", methodName);
                }
                catch (Exception ex)
                {
                    new Logger("AddSubscriptionsHandler failed", ex.Message);
                }
            }
            context.Response.StatusCode = 500;
        }

        private void GetTelemarketingProducts(HttpContext context)
        {
            var products = TelemarketingHelper.GetProducts();
            var data = new TelemarketingData() {ProductConfig = products};
            ReturnJson(context, data);
        }

        private void FindCustomer(HttpContext context)
        {
            var data = new TelemarketingData();
            var prmCusno = GetCleanRequestParam(context, Cusno);
            long cusnoForSearch;
            var cusnoForSearchProvided = long.TryParse(prmCusno, out cusnoForSearch) && cusnoForSearch > 0;

            //Is cusno is provided (from S+) use only that for search as that is the unique identifier, and email etc can differ!
            var prmFirstName = cusnoForSearchProvided ? string.Empty : GetCleanRequestParam(context, FirstName);
            var prmLastName = cusnoForSearchProvided ? string.Empty : GetCleanRequestParam(context, LastName);
            var prmCompany = string.Empty; //cusnoForSearchProvided ? string.Empty : GetCleanRequestParam(context, Company);
            var prmMobilePhone = cusnoForSearchProvided ? string.Empty : GetCleanRequestParam(context, MobilePhone);
            var prmEmail = cusnoForSearchProvided ? string.Empty : GetCleanRequestParam(context, Email);
            var searchCriteria = new Person(false, false, prmFirstName, prmLastName,
                string.Empty, prmCompany, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, prmMobilePhone, prmEmail, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            if (cusnoForSearchProvided)
            {
                searchCriteria.Cusno = cusnoForSearch;
            }

            var searchResult = SubscriptionController.FindCustomerByPerson(searchCriteria, true);
            data.CirixCustomerSearchResult = new CirixCustomerSearchResult();
            data.CirixCustomerSearchResult.CirixCustomerSearchCount = searchResult.Count;
            if (data.CirixCustomerSearchResult.CirixCustomerSearchCount == 1)
            {
                var foundPerson = searchResult[0];
                var customerInfo = SubscriptionController.GetCustomerInfo(foundPerson.Cusno);
                data.CirixCustomerSearchResult.CirixCustomer = new CirixCustomer() {Cusno = foundPerson.Cusno, FirstName = customerInfo.FirstName, LastName = customerInfo.LastName, Company = customerInfo.Company, MobilePhone = customerInfo.MobilePhone, Email = customerInfo.Email};
            }
            else if (data.CirixCustomerSearchResult.CirixCustomerSearchCount>1)
            {
                data.CirixCustomerSearchResult.CirixCustomers = new List<CirixCustomer>();
                foreach (var person in searchResult)
                {
                    var cusInfo = SubscriptionController.GetCustomerInfo(person.Cusno);
                    data.CirixCustomerSearchResult.CirixCustomers.Add(new CirixCustomer() {Cusno = person.Cusno, FirstName = cusInfo.FirstName, LastName = cusInfo.LastName, Company = cusInfo.Company, MobilePhone = cusInfo.MobilePhone, Email = cusInfo.Email});

                }
            }
            ReturnJson(context, data);
        }

        private void ListCustomerSubs(HttpContext context)
        {
            var data = new TelemarketingData();
            var prmCusno = GetCleanRequestParam(context, Cusno);
            long cusno;
            if (long.TryParse(prmCusno, out cusno))
            {
                data.CustomerSubscriptions = SubscriptionController.GetSubscriptions2(cusno);
            }
            ReturnJson(context, data);
        }
        #endregion

        private void CheckEmailAtServicePlus(HttpContext context)
        {
            var prmEmail = GetCleanRequestParam(context, Email).ToLower();
            string servicePlusUserId;
            var emailExistInServicePlus = BonDigHandler.UserExistInServicePlus(prmEmail, out servicePlusUserId);
            var data = new TelemarketingData() {EmailExistAtServicePlus = emailExistInServicePlus, ServicePlusUserId = servicePlusUserId};
            if (emailExistInServicePlus)
            {
                //Try fetch one unique ExternalSubscriberId (cusno) from user's all entitlements to match later with Cirix
                var list = BonDigHandler.GetUserEntitlementsExternalIds(servicePlusUserId);
                if (list.Any())
                {
                    if (list.Count == 1)
                    {
                        data.ExternalSubscriberId = list[0];
                    }
                    else
                    {
                        data.ExternalSubscriberId = -1; //More than 1 different ExternalSubscriberId is some error in S+, as a user should have same ExternalSubscriberId on all entitlements!
                    }
                }
            }
            ReturnJson(context, data);
        }

        private void ReturnJson(HttpContext context, string returnJsonData)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(returnJsonData);
        }

        //TODO: Upgrade to .Net v4 or use http://json.codeplex.com to serialize IList IEnumerables etc. which this does not handle
        private void ReturnJson(HttpContext context, TelemarketingData data)
        {
            var serializer = new JavaScriptSerializer();
            var jsonResult = new StringBuilder();
            serializer.Serialize(data, jsonResult);
            ReturnJson(context, jsonResult.ToString());
        }

        private string GetCleanRequestParam(HttpContext context, string paramName)
        {
            // Only POST allowed
            var paramValue = context.Request.Form[paramName];
            if (!string.IsNullOrEmpty(paramValue))
            {
                paramValue =  MiscFunctions.REC(paramValue);
               
            }
            return paramValue;
        }
    }
}