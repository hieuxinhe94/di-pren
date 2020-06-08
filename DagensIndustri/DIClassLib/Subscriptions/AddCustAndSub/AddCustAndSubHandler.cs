using System.Net;
using System.Web.Script.Services;
using System.Web.Services.Discovery;
using System.Windows.Forms;

using DIClassLib.BonnierDigital;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;

namespace DIClassLib.Subscriptions.AddCustAndSub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public class AddCustAndSubHandler
    {
        public AddCustAndSubHandler() { }

        public AddCustAndSubReturnObject TryAddCustAndSub(Subscription sub, int? customerRefNo, bool subscriberAddressIsRequired = true, List<long> servicePlusIdToExcludeFromCheck = null)
        {
            return TryAddCustAndSub(
                    sub.CampId,
                    sub.TargetGroup,
                    subscriberAddressIsRequired,
                    sub.PayMethod,
                    sub.SubsStartDate,
                    sub.IsTrialPeriod,
                    sub.Subscriber.ServicePlusUserToken,
                    sub.Subscriber.ServicePlusUserId,
                    sub.Subscriber,
                    sub.SubscriptionPayer,
                    customerRefNo,
                    servicePlusIdToExcludeFromCheck,
                    sub.IsTrialFreePeriod);
        }
        
        public AddCustAndSubReturnObject TryAddCustAndSub(string cirixCampaignId, string cirixTargetGroup, bool subscriberAddressIsRequired, PaymentMethod.TypeOfPaymentMethod paymentMethod,
                                                            DateTime wantedStartDate, bool isTrialPeriod, string servicePlusToken, string servicePlusUserId, Person subscriber, Person payer = null,
                                                            int? cardPayCustRefno = null, List<long> servicePlusIdToExcludeFromCheck = null, bool isTrialFreePeriod = false)
        {
            var subscriberObject = GetSubscriberObjFromPersonObj(subscriber);
            var payerObject = payer != null ? GetPayerObjFromPersonObj(payer) : null;
            return TryAddCustAndSub(cirixCampaignId, cirixTargetGroup, subscriberAddressIsRequired, paymentMethod, wantedStartDate, isTrialPeriod, servicePlusToken, servicePlusUserId, subscriberObject, payerObject, cardPayCustRefno, servicePlusIdToExcludeFromCheck, isTrialFreePeriod);
        }


        public AddCustAndSubReturnObject TryAddCustAndSub(string cirixCampaignId, string cirixTargetGroup, bool subscriberAddressIsRequired, PaymentMethod.TypeOfPaymentMethod paymentMethod, 
                                                          DateTime wantedStartDate, bool isTrialPeriod, string servicePlusToken, string servicePlusUserId, Subscriber subscriber, Payer payer = null,
                                                          int? cardPayCustRefno = null, List<long> servicePlusIdToExcludeFromCheck = null, bool isTrialFreePeriod = false)
        {
            var ret = new AddCustAndSubReturnObject(cirixCampaignId, cirixTargetGroup, subscriberAddressIsRequired, paymentMethod, wantedStartDate, servicePlusUserId, subscriber, payer, cardPayCustRefno);            

            var sub = new Subscription(cirixCampaignId, 0, paymentMethod, wantedStartDate.Date, isTrialPeriod, isTrialFreePeriod);
            
            sub.TargetGroup = cirixTargetGroup;
            ValidateSubBasic(sub, ret);
            if (ret.HasMessages)
                return ret;

            //if address is required address fields are also validated
            subscriber.AddressIsRequired = subscriberAddressIsRequired;
            subscriber.Validate(ret);
            
            if (payer != null)
                payer.Validate(ret);

            //form data validation failed, no need to continue 
            if (ret.HasMessages)
                return ret;

            sub.Subscriber = GetPersonObjFromSubscriberObj(subscriber, servicePlusToken, servicePlusUserId);
            sub.SubscriptionPayer = GetPersonObjFromPayerObj(payer);

            //try use cusno from S+
            List<long> extCusnos = RequestHandler.TryGetCirixCusnosFromBonDig(sub.Subscriber.ServicePlusUserToken);

            if (servicePlusIdToExcludeFromCheck != null && servicePlusIdToExcludeFromCheck.Any())
            {
                extCusnos = extCusnos.Where(extCusno => servicePlusIdToExcludeFromCheck.All(extCusnoToExclude => extCusnoToExclude != extCusno)).ToList();
            }
            
            if (extCusnos.Count == 1)
                sub.Subscriber.Cusno = extCusnos[0];
            
            if (extCusnos.Count > 1)
            {
                ret.Messages.Add(Message.MessMultipleCirixCusnosInServicePlus(extCusnos));
                ret.EmailMessage = MiscFunctions.SendStaffMailFailedSaveSubs(sub, ret);
                return ret;
            }

            //Sets the PersonFoundBySubscriber (and PersonFoundByPayer if exists), either if sub.Subscriber.Cusno > 0 or trying to find customer
            var helper = new ExistingCustHelper(sub, subscriberAddressIsRequired);

            //If PersonFoundBySubscriber is set
            helper.ValidateFoundSubscriber(ret); 
            if (ret.HasMessages)
                return ret;

            helper.ValidateFoundPayer(ret);
            if (ret.HasMessages)
                return ret;

            //Call to CreateNewCustomer will eventually be called in this step SetSubscriber() -> TryAddNewCust() -> TryAddCustomer2()
            helper.SetSubscriber(ret);
            if (ret.HasMessages)
                return ret;

            //Call to CreateNewCustomer will eventually be called in this step SetPayer() -> TryAddNewCust() -> TryAddCustomer2()
            helper.SetPayer(ret);
            if (ret.HasMessages)
                return ret;

            bool subRenewed = helper.TryCreateRenewal(ret);
            if (ret.HasMessages)
                return ret;

            if (!subRenewed)
            {
                helper.AddNewSubs(ret);
                if (ret.HasMessages)
                    return ret;
            }

            ret.CirixPayerCusno = (sub.SubscriptionPayer != null) ? sub.SubscriptionPayer.Cusno : 0;
            ret.CirixSubscriberCusno = sub.Subscriber.Cusno;
            ret.CirixSubsno = sub.SubsNo;
            ret.SavedEntitlementInServicePlus = false;


            //todo: no value in servicePlusUserId - try get servicePlusUserId by email?

            if (!string.IsNullOrEmpty(servicePlusUserId))
            {
                string bdProdId = Settings.GetBonDigProductId(sub.PaperCode, sub.ProductNo);
                ImportOutput impOut = BonDigHandler.CreateImport(bdProdId, servicePlusUserId, ret.CirixSubscriberCusno, ret.CirixSubsno);
                if (impOut == null)
                {
                    ret.Messages.Add(Message.MessFailedToCreateImportInServicePlus(sub.Subscriber.Cusno, sub.SubsNo));
                    ret.EmailMessage = MiscFunctions.SendStaffMailFailedSaveSubs(sub, ret);
                }
                else
                    ret.SavedEntitlementInServicePlus = true;
            }

            return ret;
        }
        
        //TODO: Better name on method
        public AddCustAndSubReturnObject AddFreeTrySub(string cirixCampaignId, string cirixTargetGroup, DateTime wantedStartDate, string servicePlusUserId, Subscriber subscriber, long cusno = 0)
        {
            var ret = new AddCustAndSubReturnObject(cirixCampaignId, cirixTargetGroup, false, PaymentMethod.TypeOfPaymentMethod.Invoice, wantedStartDate, servicePlusUserId, subscriber);

            var sub = new Subscription(cirixCampaignId, 0, PaymentMethod.TypeOfPaymentMethod.Invoice, wantedStartDate.Date, true)
            {
                TargetGroup = cirixTargetGroup
            };

            ValidateSubBasic(sub, ret);
            if (ret.HasMessages)
                return ret;

            //if address is required address fields are also validated
            subscriber.AddressIsRequired = false;
            subscriber.Validate(ret);
            if (ret.HasMessages)
                return ret;

            //Transfer provided subscriber to the Subscription person object
            sub.Subscriber = GetPersonObjFromSubscriberObj(subscriber, string.Empty, servicePlusUserId);
            
            //We already know cusno of current user that we want to add a new subscription to
            if (cusno > 0)
            {
                sub.Subscriber.Cusno = cusno;
            }
            sub.SubscriptionPayer = null;

            //Try use cusno from S+
            if (sub.Subscriber.Cusno < 1 && !string.IsNullOrEmpty(sub.Subscriber.ServicePlusUserToken))
            {
                var extCusnos = RequestHandler.TryGetCirixCusnosFromBonDig(sub.Subscriber.ServicePlusUserToken);
                if (extCusnos.Count == 1)
                    sub.Subscriber.Cusno = extCusnos[0];

                if (extCusnos.Count > 1)
                {
                    ret.Messages.Add(Message.MessMultipleCirixCusnosInServicePlus(extCusnos));
                    ret.EmailMessage = MiscFunctions.SendStaffMailFailedSaveSubs(sub, ret);
                    return ret;
                }
            }

            var helper = new ExistingCustHelper(sub, false);

            helper.ValidateFoundSubscriber(ret);
            if (ret.HasMessages)
                return ret;

            helper.ValidateFoundPayer(ret);
            if (ret.HasMessages)
                return ret;

            //Will add new customer if not exist
            helper.SetSubscriber(ret);
            if (ret.HasMessages)
                return ret;

            var subRenewed = helper.TryCreateRenewal(ret);
            if (ret.HasMessages)
                return ret;

            if (!subRenewed)
            {
                //Will add subscription to the customer
                helper.AddNewSubs(ret);
                if (ret.HasMessages)
                    return ret;
            }

            ret.CirixPayerCusno = (sub.SubscriptionPayer != null) ? sub.SubscriptionPayer.Cusno : 0;
            ret.CirixSubscriberCusno = sub.Subscriber.Cusno;
            ret.CirixSubsno = sub.SubsNo;
            ret.SavedEntitlementInServicePlus = false;


            //todo: no value in servicePlusUserId - try get servicePlusUserId by email?

            if (!string.IsNullOrEmpty(servicePlusUserId))
            {
                string bdProdId = Settings.GetBonDigProductId(sub.PaperCode, sub.ProductNo);
                ImportOutput impOut = BonDigHandler.CreateImport(bdProdId, servicePlusUserId, ret.CirixSubscriberCusno, ret.CirixSubsno);
                if (impOut == null)
                {
                    ret.Messages.Add(Message.MessFailedToCreateImportInServicePlus(sub.Subscriber.Cusno, sub.SubsNo));
                    ret.EmailMessage = MiscFunctions.SendStaffMailFailedSaveSubs(sub, ret);
                }
                else
                    ret.SavedEntitlementInServicePlus = true;
            }

            return ret;
        }     

        private void ValidateSubBasic(Subscription sub, AddCustAndSubReturnObject ret)
        {
            if (string.IsNullOrEmpty(sub.CampId))
                ret.Messages.Add(Message.MessValidateSubMissingCampId());
            else
            {
                if (sub.CampNo <= 0)
                    ret.Messages.Add(Message.MessValidateSubCampIdNotValid(sub));
            }

            //todo validate targetgroup
        }

        private Person GetPersonObjFromSubscriberObj(Subscriber subscriber, string servicePlusToken = "", string servicePlusUserId = "")
        {
            if (subscriber == null)
                return null;

            return new Person(true, false, subscriber.FirstName, subscriber.LastName, subscriber.CareOf, subscriber.Company, subscriber.StreetName, subscriber.HouseNo,
                subscriber.StairCase, subscriber.Stairs, subscriber.ApartmentNo, subscriber.ZipCode, subscriber.City, subscriber.MobilePhone, subscriber.Email, string.Empty,
                string.Empty, string.Empty, string.Empty, servicePlusToken, servicePlusUserId);
        }

        private Person GetPersonObjFromPayerObj(Payer payer)
        {
            if (payer == null)
                return null;

            return new Person(false, false, string.Empty, string.Empty, payer.CareOf, payer.Company, payer.StreetName, payer.HouseNo,
                payer.StairCase, payer.Stairs, payer.ApartmentNo, payer.ZipCode, payer.City, "", "", "", payer.CompanyNo, payer.Attention, payer.PhoneDayTime);
        }

        private Subscriber GetSubscriberObjFromPersonObj(Person person)
        {
            return new Subscriber(person.FirstName, person.LastName, person.MobilePhone, person.Email, person.Company, person.CareOf, person.StreetName, person.HouseNo,
                person.StairCase, person.Stairs, person.ApartmentNo, person.ZipCode, person.City);
        }

        private Payer GetPayerObjFromPersonObj(Person person)
        {
            //Using person.MobilePhone as PhoneDayTime
            return new Payer(person.PhoneDayTime, person.Company, person.CareOf, person.Attention, person.CompanyNo, person.StreetName, person.HouseNo,
                person.StairCase, person.Stairs, person.ApartmentNo, person.ZipCode, person.City);
        }

    }

}
