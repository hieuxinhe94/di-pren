using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Di.Common.Logging;
using DIClassLib.CardPayment;
using DIClassLib.CardPayment.Nets;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.AddCustAndSub;
using EPiServer.Framework.Cache;
using Pren.Web.Business.Cache;
using Pren.Web.Business.Campaign;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Mail;
using Pren.Web.Business.ServicePlus.Logic;

namespace Pren.Web.Business.Subscription
{
    class SubscriptionService : ISubscriptionService<DIClassLib.Subscriptions.Subscription, DIClassLib.Subscriptions.SubscriptionUser2>
    {
        private readonly ILogger _logService;
        private readonly IObjectCache _objectCache;
        private readonly IMailHandler _mailHandler;
        private readonly ISiteConfiguration _siteConfiguration;
        private readonly IServicePlusFacade _servicePlusFacade;

        public SubscriptionService(ILogger logService, IObjectCache objectCache, IMailHandler mailHandler, ISiteConfiguration siteConfiguration, IServicePlusFacade servicePlusFacade)
        {
            _logService = logService;
            _objectCache = objectCache;
            _mailHandler = mailHandler;
            _siteConfiguration = siteConfiguration;
            _servicePlusFacade = servicePlusFacade;
        }

        public DIClassLib.Subscriptions.Subscription InitiateSubscription(
            PaymentMethod.TypeOfPaymentMethod paymethod, 
            string campId, 
            string prenStart, 
            bool isTrialPeriod,
            bool isTrialFreePeriod = false)
        {
            //Set startdate on sub
            DateTime dateParser;
            var startDate = DateTime.TryParse(prenStart, out dateParser) ? dateParser : DateTime.Now;

            //Create subscription
            var subscription = new DIClassLib.Subscriptions.Subscription(campId, 0, paymethod, startDate, isTrialPeriod, isTrialFreePeriod);

            return subscription;
        }

        public string SaveSubscription(
            DIClassLib.Subscriptions.Subscription subscription,            
            int? customerRefNo = null, 
            PaymentMethod.TypeOfPaymentMethod payMethod = PaymentMethod.TypeOfPaymentMethod.Invoice,
            NetsCardPayReturn ret = null,
            bool createInvoiceAndPayment = false)
        {
            var err = string.Empty;

            try
            {
                if (subscription == null || string.IsNullOrEmpty(subscription.CampId))
                {
                    _logService.Log("SubscriptionService - SaveSubscription error", LogLevel.Error, typeof(SubscriptionService));
                    return CampaignConstants.GeneralErrorMessage + "<!-- Error: subscription null -->";
                }

                var servicePlusIdsToExcludeFromCheck = new List<long>();

                try
                {
                    var user = _servicePlusFacade.GetUserByToken(subscription.Subscriber.ServicePlusUserToken);
                    if (user != null && user.KayakBizSubscriptionCustomerNumber > 0)
                    {
                        servicePlusIdsToExcludeFromCheck.Add(user.KayakBizSubscriptionCustomerNumber);
                    }
                }
                catch (Exception)
                {
                    
                }

                var addCustAndSubHandler = new AddCustAndSubHandler();
                var addCustSubRet = addCustAndSubHandler.TryAddCustAndSub(subscription, null, subscription.Subscriber.ZipCode != "10000", servicePlusIdsToExcludeFromCheck);
                if (subscription.Subscriber != null)
                {
                    subscription.Subscriber.Cusno = addCustSubRet.CirixSubscriberCusno;
                    subscription.SubsNo = addCustSubRet.CirixSubsno;
                }

                // Credit card payment
                if (addCustSubRet.CirixSubsno > 0 && customerRefNo != null)
                {
                    MsSqlHandler.AppendToPayTransComment((int)customerRefNo, "cusno: " + addCustSubRet.CirixSubscriberCusno);

                    if (payMethod == PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal)
                    {
                        SubscriptionController.CreateNewInvoice(subscription);
                        MsSqlHandler.InsertAwd2(ret, addCustSubRet.CirixSubscriberCusno, addCustSubRet.CirixSubsno,
                            subscription.CampNo, subscription.SubsEndDate);
                    }
                    else if(createInvoiceAndPayment)
                    {
                        CreateInvoiceAndPayment(subscription, addCustSubRet);                           
                    }
                }

                // Something went wrong!
                if (addCustSubRet.HasMessages)
                {
                    var returnMessages = new StringBuilder();
                    foreach (var msg in addCustSubRet.Messages)
                    {
                        returnMessages.Append(msg.MessageCustomer);
                        if (subscription.Subscriber != null)
                            _logService.Log(string.Format("{0} {1} ({2}): {3}", subscription.Subscriber.FirstName, subscription.Subscriber.LastName, subscription.Subscriber.Email, msg.MessageSweStaff), LogLevel.Info, typeof(SubscriptionService));
                        else
                            _logService.Log(msg.MessageSweStaff, LogLevel.Info, typeof(SubscriptionService));
                    }

                    return returnMessages.ToString();
                }
            }
            catch (Exception ex)
            {
                err = CampaignConstants.GeneralErrorMessage + "<!-- Error: " + ex.Message + "-->";
                _logService.Log(ex, "Pren.di.se error CampaignPaperPrototyp(2): ", LogLevel.Error, typeof(SubscriptionService));
            }

            if (!string.IsNullOrEmpty(err))
                return err;

            if (subscription.Subscriber == null)
                _logService.Log("SaveSubscription() failed - subscriber=null", LogLevel.Info, typeof(SubscriptionService));

            return string.Empty;

        }

        public bool ValidateSubscription(DIClassLib.Subscriptions.Subscription subscription)
        {
            return subscription.CampNo > 0;
        }

        public void HandleDigitalSubscription(DIClassLib.Subscriptions.Subscription subscription, bool isDigital)
        {
            if (isDigital &&
                subscription != null &&
                subscription.Subscriber != null)
            {
                if (string.IsNullOrEmpty(subscription.Subscriber.ZipCode))
                {
                    subscription.Subscriber.ZipCode = "10000";
                }
                if (string.IsNullOrEmpty(subscription.Subscriber.FirstName))
                {
                    subscription.Subscriber.FirstName = "Förnamn";
                }
                if (string.IsNullOrEmpty(subscription.Subscriber.LastName))
                {
                    subscription.Subscriber.LastName = "Efternamn";
                }
            }
        }

        public HashSet<string> GetAllTargetGroups()
        {
            const string allTargetGroupsCacheKey = "alltargetgroups";

            var targetGroups = (HashSet<string>)_objectCache.GetFromCache(allTargetGroupsCacheKey);
            if (targetGroups != null)
            {
                return targetGroups;
            }

            targetGroups = SubscriptionController.GetTargetGroups(Settings.PaperCode_DI);
            targetGroups.UnionWith(SubscriptionController.GetTargetGroups(Settings.PaperCode_DISE));
            targetGroups.UnionWith(SubscriptionController.GetTargetGroups(Settings.PaperCode_IPAD));

            _objectCache.AddToCache(allTargetGroupsCacheKey, targetGroups, new CacheEvictionPolicy(new TimeSpan(0, 1, 0, 0), CacheTimeoutType.Absolute));

            return targetGroups;
        }

        public void InsertExtraInfo(int contentId, long cusNo, string heading, string value)
        {
            MsSqlHandler.MdbInsertExtraInfo(contentId, cusNo, heading, value);
        }


        public SubscriptionUser2 GetSubscriptionUser(long cusno)
        {
            return new SubscriptionUser2(cusno);
        }

        public bool IsActiveAutowithdrawalSubscription(long subsno)
        {
            return MsSqlHandler.IsActiveAwdSubs(subsno);
        }

        public bool SubscriptionIsQuarterlyPayment(long subscriptionNumber, int extno)
        {
            var ds = SubscriptionController.GetInvArgItems(subscriptionNumber, extno);
            if (!DbHelpMethods.DataSetHasRows(ds)) return false;

            return ds.Tables[0].Rows.Count > 1;
        }

        public DataSet GetOpenInvoices(long cusNo)
        {
            var invoices = SubscriptionController.GetOpenInvoices(cusNo);

            return DbHelpMethods.DataSetHasRows(invoices) ? invoices : null;
        }

        public DataSet GetAutoWithdrawalPaymentHistory(long subsNo)
        {
            var history = MsSqlHandler.GetAwdPayHistoryBySubsno(subsNo);

            return DbHelpMethods.DataSetHasRows(history) ? history : null;
        }

        public void CancelAutoWithdrawalPayment(long subsNo)
        {
            MsSqlHandler.CancelAwdSubscription(subsNo);
        }

        public long GetCustomerNumberByEcusno(string eCusno)
        {
            long eCusnoLong;

            return long.TryParse(eCusno, out eCusnoLong) ? SubscriptionController.GetCustomerByEcusno(eCusnoLong) : 0;
        }

        public long GetEcusnoByCustomerNumber(long customerNumber)
        {
            return SubscriptionController.GetEcusnoByCustomer(customerNumber);
        }

        public List<long> GetCustomerNumberByEmail(string email)
        {
            return SubscriptionController.GetCusnosByEmail(email);
        }

        public bool UpdateSubscriberEmail(SubscriptionUser2 subscriber, string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                    email = email.ToLower();

                subscriber.CompanyId = "1234567890";

                new CustomerPropertyHandler(subscriber.Cusno, email, null, null, null, null);
                SubscriptionController.UpdateCustomerInformation(subscriber.Cusno, email, subscriber.HPhone, subscriber.WPhone, subscriber.OPhone, subscriber.SalesDen, subscriber.OfferdenDir, subscriber.OfferdenSal, subscriber.OfferdenEmail, subscriber.DenySmsMark, subscriber.AccnoBank, subscriber.AccnoAcc, subscriber.Notes, subscriber.Ecusno, subscriber.OtherCusno, subscriber.UserName, subscriber.Expday, subscriber.DiscPercent, subscriber.Terms, subscriber.SocialSecNo, subscriber.Category, subscriber.MasterCusno, subscriber.CompanyId);

                subscriber.Email = email;
                return true;
            }
            catch (Exception ex)
            {
                _logService.Log(ex, "UpdateEmail() failed", LogLevel.Error, typeof(SubscriptionService));
            }

            return false;
        }

        public void AddConsent(long customerNumer)
        {
            var propertyHandler = new CustomerPropertyHandler(customerNumer);
            if (propertyHandler.AllCustomerProperties == null) return;
            var newProps = new List<CustomerProp>() { new CustomerProp("94", "01") }; //Samtycke, 01 = ja, 02 = nej                    
            propertyHandler.InsertCusProps(propertyHandler.AllCustomerProperties, newProps);
        }

        private void CreateInvoiceAndPayment(DIClassLib.Subscriptions.Subscription subscription, AddCustAndSubReturnObject status)
        {
            try
            {
                var invno = SubscriptionController.GetNextInvno();
                var refno = SubscriptionController.BuildRefno2(invno, "00", subscription.PaperCode);
                var invoiceNumber = SubscriptionController.CreateImmediateInvoice(subscription, 0, 1, invno, refno);

                if (invoiceNumber < 1)
                {
                    SendMailToStaff(
                        "CreateImmediateInvoice failed",
                        "Ett problem uppstod vid skapandet av ImmediateInvoice. Fakturanummer är 0.<br><strong>Kunduppgifter</strong><br>" + subscription.ToString());
                    _logService.Log("CreateInvoiceAndPayment - CreateImmediateInvoice failed.", LogLevel.Error, typeof(SubscriptionService));
                    return;
                }

                var vatPct = SubscriptionController.GetProductVat(subscription.PaperCode, subscription.ProductNo);
                var pc = new PriceCalculator(null, subscription.TotalPriceExVat, vatPct, 1);
                var priceIncludingVat = pc.PriceIncVat != null ? Convert.ToInt64(pc.PriceIncVat) : 0;

                var payRet = SubscriptionController.InsertElectronicPayment(subscription.Subscriber.Cusno, invoiceNumber, refno, priceIncludingVat);

                if (payRet == "OK")
                {
                    _logService.Log("CreateInvoiceAndPayment success<br>" + subscription, LogLevel.Info, typeof(SubscriptionService));
                    return;
                }
                
                SendMailToStaff(
                    "CreatePaymentOnInvoice failed",
                    "Ett problem uppstod vid skapandet av CreatePaymentOnInvoice.<br><strong>Kunduppgifter</strong><br>" + subscription.ToString());
                _logService.Log("CreateInvoiceAndPayment - CreateImmediateInvoice failed.", LogLevel.Error, typeof(SubscriptionService));
            }
            catch (Exception exception)
            {
                SendMailToStaff(
                    "CreateInvoiceAndPayment failed",
                    "Ett problem uppstod vid skapandet av CreateInvoiceAndPayment.<br><strong>Kunduppgifter</strong><br>" + subscription.ToString());
                _logService.Log(exception, "CreateInvoiceAndPayment - failed.", LogLevel.Error, typeof(SubscriptionService));                    
                throw;
            }
        }

        private void SendMailToStaff(string subject, string body)
        {
            var mailAddress = _siteConfiguration.GetSetting("mailPrenFelDiSe");
            _mailHandler.SendMail(mailAddress, mailAddress, subject, body, true);
        }
    }
}
