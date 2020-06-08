using System;
using System.Collections.Generic;
using System.Data;
using DIClassLib.CardPayment.Nets;
using DIClassLib.Subscriptions;

namespace Pren.Web.Business.Subscription
{
    public interface ISubscriptionService<TSubscription, out TSubscriptionUser>
    {
        TSubscription InitiateSubscription(
            PaymentMethod.TypeOfPaymentMethod paymethod, 
            string campId, 
            string prenStart,
            bool isTrialPeriod,
            bool isTrialFreePeriod = false);

        String SaveSubscription(
            TSubscription subscription,            
            int? customerRefNo = null,
            PaymentMethod.TypeOfPaymentMethod payMethod = PaymentMethod.TypeOfPaymentMethod.Invoice,
            NetsCardPayReturn ret = null,
            bool createInvoiceAndPayment = false);

        bool ValidateSubscription(TSubscription subscription);

        void HandleDigitalSubscription(TSubscription subscription, bool isDigital);

        HashSet<string> GetAllTargetGroups();

        void InsertExtraInfo(int contentId, long cusNo, string heading, string value);

        TSubscriptionUser GetSubscriptionUser(long cusno);        

        bool IsActiveAutowithdrawalSubscription(long subsno);

        bool SubscriptionIsQuarterlyPayment(long subscriptionNumber, int extno);

        DataSet GetOpenInvoices(long cusNo);

        DataSet GetAutoWithdrawalPaymentHistory(long subsNo);
        void CancelAutoWithdrawalPayment(long subsNo);

        long GetCustomerNumberByEcusno(string eCusno);
        long GetEcusnoByCustomerNumber(long customerNumber);

        List<long> GetCustomerNumberByEmail(string email);

        bool UpdateSubscriberEmail(SubscriptionUser2 subscriber, string email);

        void AddConsent(long customerNumer);

    }
}
