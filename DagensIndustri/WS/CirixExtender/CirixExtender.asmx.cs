using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.DbHandlers;

namespace WS.CirixExtender
{
    /// <summary>
    /// Summary description for CirixExtender
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CirixExtender : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="campId">XXXXX (string)</param>
        /// <param name="campNo">12345 (long)</param>
        /// <param name="paymentMethodId">1=Invoice, 2=InvoiceOtherPayer, 3=DirectDebit, 4=CreditCard, 5=CreditCardAutowithdrawal, 6=DirectDebitOtherPayer</param>
        /// <param name="wantedStartDate">YYYY-MM-DD</param>
        /// <returns></returns>
        public static AddCustsToCirixReturn ExtInsertSubsWithSubscriber(string campId, long campNo, int paymentMethodId, DateTime wantedStartDate, string targetGroup,
                                                                        string SubscriberFirstName, string SubscriberLastName, string SubscriberCareOf, string SubscriberCompany, string SubscriberStreetName, string SubscriberHouseNo, string SubscriberStairCase, string SubscriberStairs, string SubscriberApartmentNo, string SubscriberZipCode, string SubscriberCity, string SubscriberPhoneMobile, string SubscriberEmail, string SubscriberSocialSecurityNo, string SubscriberCompanyNo, string SubscriberAttention, string SubscriberPhoneDayTime)
        {
            AddCustsToCirixReturn retObj = new AddCustsToCirixReturn();

            Person subscriber = new Person(true, false, SubscriberFirstName, SubscriberLastName, SubscriberCareOf, SubscriberCompany, SubscriberStreetName, SubscriberHouseNo, SubscriberStairCase, SubscriberStairs, SubscriberApartmentNo, SubscriberZipCode, SubscriberCity, SubscriberPhoneMobile, SubscriberEmail, SubscriberSocialSecurityNo, SubscriberCompanyNo, SubscriberAttention, SubscriberPhoneDayTime);

            ValidatePerson(subscriber, retObj);
            if (retObj.Errors.Count > 0)
                return retObj;


            Subscription sub = new Subscription(campId, campNo, GetPayMethodByInt(paymentMethodId), wantedStartDate);
            sub.TargetGroup = targetGroup;
            sub.Subscriber = subscriber;

            ValidateSub(sub, retObj);
            if (retObj.Errors.Count > 0)
                return retObj;


            string err = CirixDbHandler.TryInsertSubscription2(sub, null);
            if (string.IsNullOrEmpty(err))
            {
                retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.SaveToCirixFailed);
                retObj.ErrorDetails = err + "<br><br><br><br>" + sub.ToString();
                return retObj;
            }

            retObj.Subsno = sub.SubsNo;
            retObj.SubscriberCusno = sub.Subscriber.Cusno;

            return retObj;
        }

        public static AddCustsToCirixReturn ExtInsertSubsWithSubscriberAndPayer(string campId, long campNo, int paymentMethodId, DateTime wantedStartDate, string targetGroup,
                                                                                string SubscriberFirstName, string SubscriberLastName, string SubscriberCareOf, string SubscriberCompany, string SubscriberStreetName, string SubscriberHouseNo, string SubscriberStairCase, string SubscriberStairs, string SubscriberApartmentNo, string SubscriberZipCode, string SubscriberCity, string SubscriberPhoneMobile, string SubscriberEmail, string SubscriberSocialSecurityNo, string SubscriberCompanyNo, string SubscriberAttention, string SubscriberPhoneDayTime,
                                                                                string PayerFirstName, string PayerLastName, string PayerCareOf, string PayerCompany, string PayerStreetName, string PayerHouseNo, string PayerStairCase, string PayerStairs, string PayerApartmentNo, string PayerZipCode, string PayerCity, string PayerPhoneMobile, string PayerEmail, string PayerSocialSecurityNo, string PayerCompanyNo, string PayerAttention, string PayerPhoneDayTime)
        {
            AddCustsToCirixReturn retObj = new AddCustsToCirixReturn();

            Person subscriber = new Person(true, false, SubscriberFirstName, SubscriberLastName, SubscriberCareOf, SubscriberCompany, SubscriberStreetName, SubscriberHouseNo, SubscriberStairCase, SubscriberStairs, SubscriberApartmentNo, SubscriberZipCode, SubscriberCity, SubscriberPhoneMobile, SubscriberEmail, SubscriberSocialSecurityNo, SubscriberCompanyNo, SubscriberAttention, SubscriberPhoneDayTime);
            Person payer = new Person(false, false, PayerFirstName, PayerLastName, PayerCareOf, PayerCompany, PayerStreetName, PayerHouseNo, PayerStairCase, PayerStairs, PayerApartmentNo, PayerZipCode, PayerCity, PayerPhoneMobile, PayerEmail, PayerSocialSecurityNo, PayerCompanyNo, PayerAttention, PayerPhoneDayTime);

            ValidatePerson(subscriber, retObj);
            ValidatePerson(payer, retObj);
            if (retObj.Errors.Count > 0)
                return retObj;


            Subscription sub = new Subscription(campId, campNo, GetPayMethodByInt(paymentMethodId), wantedStartDate);
            sub.TargetGroup = targetGroup;
            sub.Subscriber = subscriber;
            sub.SubscriptionPayer = payer;

            ValidateSub(sub, retObj);
            if (retObj.Errors.Count > 0)
                return retObj;


            string err = CirixDbHandler.TryInsertSubscription2(sub, null);
            if (string.IsNullOrEmpty(err))
            {
                retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.SaveToCirixFailed);
                retObj.ErrorDetails = err + "<br><br><br><br>" + sub.ToString();  //?
                return retObj;
            }

            retObj.Subsno = sub.SubsNo;
            retObj.SubscriberCusno = sub.Subscriber.Cusno;
            retObj.PayerCusno = sub.SubscriptionPayer.Cusno;

            return retObj;
        }

        private static void ValidatePerson(Person p, AddCustsToCirixReturn retObj)
        {
            if (p.IsSubscriber)
            {
                if (string.IsNullOrEmpty(p.FirstName))
                    retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.FirstName);

                if (string.IsNullOrEmpty(p.LastName))
                    retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.LastName);

                if (!MiscFunctions.IsValidSwePhoneNum(p.MobilePhone))
                    retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.PhoneMobile);

                if (!MiscFunctions.IsValidEmail(p.Email))
                    retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.Email);

                if (string.IsNullOrEmpty(p.StreetName))
                    retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.StreetName);

                if (!MiscFunctions.IsValidSweZipCode(p.ZipCode))
                    retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.ZipCode);
            }
            else  //payer
            {
                if (!MiscFunctions.IsValidSwePhoneNum(p.PhoneDayTime))
                    retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.PhoneDayTime);

                if (!MiscFunctions.IsValidSweZipCode(p.ZipCode))
                    retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.ZipCode);
            }
        }

        private static void ValidateSub(Subscription sub, AddCustsToCirixReturn retObj)
        {
            if (sub == null)
            {
                retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.CampNotPopulated);
                return;
            }

            if (string.IsNullOrEmpty(sub.CampId) || sub.CampNo <= 0)
            {
                retObj.CampId = sub.CampId;
                retObj.CampNo = sub.CampNo;
                retObj.Errors.Add((int)AddCustsToCirixReturn.ErrorTypes.CampNotPopulated);
            }

            //todo: need to check if targetGroup exists in cirix
        }


        /// <summary>
        /// payMethodId return values: 1=Invoice, 2=InvoiceOtherPayer, 3=DirectDebit, 4=CreditCard, 5=CreditCardAutowithdrawal, 6=DirectDebitOtherPayer
        /// </summary>
        private static PaymentMethod.TypeOfPaymentMethod GetPayMethodByInt(int payMethodId)
        {
            switch (payMethodId)
            {
                case 1:
                    return PaymentMethod.TypeOfPaymentMethod.Invoice;
                case 2:
                    return PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer;
                case 3:
                    return PaymentMethod.TypeOfPaymentMethod.DirectDebit;
                case 4:
                    return PaymentMethod.TypeOfPaymentMethod.CreditCard;
                case 5:
                    return PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal;
                case 6:
                    return PaymentMethod.TypeOfPaymentMethod.DirectDebitOtherPayer;
                default:
                    return PaymentMethod.TypeOfPaymentMethod.Invoice;
            }
        }
    }
}
