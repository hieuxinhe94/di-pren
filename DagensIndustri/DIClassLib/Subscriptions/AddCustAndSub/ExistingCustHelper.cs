using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DIClassLib.Subscriptions.AddCustAndSub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DIClassLib.DbHandlers;
    using System.Data;

    public class ExistingCustHelper
    {
        private const int MAX_NUM_DAYS_IN_TRIAL_PERIOD = 90;

        public Person PersonFoundBySubscriber;          //active / most recent used / null
        public Person PersonFoundByPayer;

        private Subscription _sub;
        private DataSet _dsSubscriber;

        private string _oldPayersDefInvoiceMode = "";   //needed on rollback of UpdateCustomerInformation2()


        public ExistingCustHelper(Subscription sub, bool subRequiresSubscriberAddress)
        {
            _sub = sub;

            if (_sub.Subscriber.Cusno > 0)
            {
                PersonFoundBySubscriber = _sub.Subscriber;
            }
            else
            {
                List<Person> allFoundBySubscriber = SubscriptionController.FindCustomerByPerson(_sub.Subscriber, !subRequiresSubscriberAddress);
                PersonFoundBySubscriber = GetMostRecentUsedCust(allFoundBySubscriber);    
            }

            List<Person> allFoundByPayer = SubscriptionController.FindCustomerByPerson(_sub.SubscriptionPayer, false);
            PersonFoundByPayer = GetMostRecentUsedCust(allFoundByPayer);

            if (PersonFoundBySubscriber != null)
                _dsSubscriber = SubscriptionController.GetCustomer(PersonFoundBySubscriber.Cusno);
        }

        private Person GetMostRecentUsedCust(List<Person> pers)
        {
            Person ret = null;

            foreach (Person p in pers)
            {
                if (ret == null)
                {
                    ret = p;
                    continue;
                }

                if (p.SubsHistory.Count > 0)
                {
                    DateTime tmp = ret.SubsHistory.Count > 0 ? ret.SubsHistory[0].SubsRealEndDate : DateTime.MinValue;
                    if (p.SubsHistory[0].SubsRealEndDate > tmp)
                        ret = p;
                }
            }

            return ret;
        }


        public bool CustHasSubThatCannotBeRenewed(bool isSubscriber)
        {
            Person p = isSubscriber ? PersonFoundBySubscriber : PersonFoundByPayer;
            var subs = p.SubsHistory;
            List<Subscription> notRenewableSubs = subs.Where(x => SubsAreOfSameKind(x, _sub, true))
                                                      .Where(x => SubsIsTidsbestamd(x) == false)
                                                      .Where(SubsIsActive)
                                                      .ToList();
            if (notRenewableSubs.Count > 0)
                return true;

            return false;
        }

        private bool SubsAreOfSameKind(Subscription existingSub, Subscription wantedSub, bool exactMatch)
        {
            //As old IPAD-01 have changed name to IPAD-01 during migration to Kayak, we need to do this check
            var ipadCheck01 = existingSub.PaperCode == Settings.PaperCode_DI && existingSub.ProductNo == Settings.ProductNo_IPAD
                              && wantedSub.PaperCode == Settings.PaperCode_IPAD && wantedSub.ProductNo == "01";

            var ipadCheck02 = existingSub.PaperCode == Settings.PaperCode_IPAD && existingSub.ProductNo == "01"
                              && wantedSub.PaperCode == Settings.PaperCode_DI && wantedSub.ProductNo == Settings.ProductNo_IPAD;
            if (ipadCheck01 || ipadCheck02)
            {
                return true;
            }

            //any paper sub is just considered to be a paper sub (productNo does not matter)
            if (!exactMatch && existingSub.PaperCode == Settings.PaperCode_DI && wantedSub.PaperCode == Settings.PaperCode_DI)
                return true;

            return (existingSub.PaperCode == wantedSub.PaperCode && existingSub.ProductNo == wantedSub.ProductNo);
        }

        private bool SubsIsActive(Subscription sub)
        {
            return Settings.SubsStateActiveValues.Contains(sub.SubsState);
        }

        private bool SubsIsTidsbestamd(Subscription sub)
        {
            return (sub.SubsKind == Settings.SubsKind_tidsbestamd);
        }
        

        public void ValidateFoundSubscriber(AddCustAndSubReturnObject ret)
        {
            if (PersonFoundBySubscriber != null)
            {
                if (SubIsTrial(_sub) && !TrialPeriodOk(PersonFoundBySubscriber, _sub.Pricegroup))
                {
                    ret.Messages.Add(_sub.PaperCode == Settings.PaperCode_AGENDA ?
                        Message.MessTrialPeriodDeniedAgenda(PersonFoundBySubscriber.Cusno) :
                        Message.MessTrialPeriodDenied(PersonFoundBySubscriber.Cusno));

                    //paid with credit card - send mail to support
                    if (ret.InData.CardPayCustRefno != null && (long)ret.InData.CardPayCustRefno > 0)
                        ret.EmailMessage = MiscFunctions.SendStaffMailFailedSaveSubs(_sub, ret);

                    MsSqlHandler.InsertDeniedTrialSub(_sub);
                    return;
                }

                if (CustHadDifferentRoleLastSub(true))
                {
                    ret.Messages.Add(Message.MessSubscriberHasHadDifferentRole(PersonFoundBySubscriber.Cusno));
                    ret.EmailMessage = MiscFunctions.SendStaffMailFailedSaveSubs(_sub, ret);
                    return;
                }
                
                if (CustHasSubThatCannotBeRenewed(true))
                {
                    ret.Messages.Add(Message.MessSubscriberHasActiveSubOfSameKind(PersonFoundBySubscriber.Cusno));
                    ret.EmailMessage = MiscFunctions.SendStaffMailFailedSaveSubs(_sub, ret);
                    return;
                }
            }
        }

        public void ValidateFoundPayer(AddCustAndSubReturnObject ret)
        {
            if (PersonFoundByPayer != null && CustHadDifferentRoleLastSub(false))
            {
                ret.Messages.Add(Message.MessPayerHasHadDifferentRole(PersonFoundByPayer.Cusno));
                ret.EmailMessage = MiscFunctions.SendStaffMailFailedSaveSubs(_sub, ret);
            }
        }

        private bool SubIsTrial(Subscription sub)
        {
            var trialPriceGroup = MiscFunctions.GetAppsettingsValue("PriceGroupTrialPeriod");
            var trialPriceGroupFree = Settings.PriceGroupTrialFree;

            return sub.Pricegroup == trialPriceGroup || sub.Pricegroup == trialPriceGroupFree;
        }
        
        private bool SubHasTrialLength(Subscription sub)
        {
            return (SubsLengthDays(sub) <= MAX_NUM_DAYS_IN_TRIAL_PERIOD);
        }

        private int SubsLengthDays(Subscription sub)
        {
            return (sub.SubsLenMons * 30) + sub.SubsLenDays;
        }

        private bool TrialPeriodOk(Person p, string priceGroup)
        {
            List<Subscription> recentTrialPeriods = p.SubsHistory.Where(existingSub => SubsAreOfSameKind(existingSub, _sub, false))
                                                        .Where(existingSub => existingSub.Pricegroup == priceGroup)
                                                        .Where(x => CheckValidForNewTrial(x) || SubsIsActive(x))
                                                        .ToList();
            if (recentTrialPeriods.Count > 0)
                return false;

            return true;
        }


        private bool TrialPeriodOk(Person p)
        {
            List<Subscription> recentTrialPeriods = p.SubsHistory.Where(existingSub => SubsAreOfSameKind(existingSub, _sub, false))
                                                        .Where(SubIsTrial)
                                                        .Where(x => CheckValidForNewTrial(x) || SubsIsActive(x))
                                                        .ToList();
            if (recentTrialPeriods.Count > 0)
                return false;

            return true;
        }

        private bool CustHadDifferentRoleLastSub(bool isSubscriber)
        {
            Person p = isSubscriber ? PersonFoundBySubscriber : PersonFoundByPayer;

            if (p != null)
            {
                //no subs history, unclear if p was subscriber/payer, cannot add subs to p
                if (p.SubsHistory.Count == 0)
                    return true;

                if (p.SubsHistory.Count > 0)
                {
                    long oldCusno = 0;
                    if (isSubscriber)
                        oldCusno = p.SubsHistory[0].Subscriber.Cusno;
                    else
                        if (p.SubsHistory[0].SubscriptionPayer != null)
                            oldCusno = p.SubsHistory[0].SubscriptionPayer.Cusno;

                    if (p.Cusno != oldCusno)
                        return true;
                }
            }

            return false;
        }

        private bool CheckValidForNewTrial(Subscription sub)
        {
            int months = (sub.PaperCode == Settings.PaperCode_AGENDA) ? Settings.MinMonthsSinceLastTrialAgenda : Settings.MinMonthsSinceLastTrial;
            DateTime end = sub.SubsRealEndDate;
            return (end > DateTime.Now.AddMonths(-months).Date && end < DateTime.Now.Date);
        }



        public void SetSubscriber(AddCustAndSubReturnObject ret)
        {
            if (PersonFoundBySubscriber != null)
                _sub.Subscriber.Cusno = PersonFoundBySubscriber.Cusno;
            else
                TryAddNewCust(true, ret);
        }

        public void SetPayer(AddCustAndSubReturnObject ret)
        {
            if (_sub.SubscriptionPayer != null)
            {
                if (PersonFoundByPayer != null)
                    _sub.SubscriptionPayer.Cusno = PersonFoundByPayer.Cusno;
                else
                    TryAddNewCust(false, ret);
            }
        }

        private void TryAddNewCust(bool isSubscriber, AddCustAndSubReturnObject ret)
        {
            long cusno = SubscriptionController.TryAddCustomer2(_sub, isSubscriber);

            if (cusno > 0)
            {
                if (isSubscriber)
                    _sub.Subscriber.Cusno = cusno;
                else
                    _sub.SubscriptionPayer.Cusno = cusno;

                return;
            }

            ret.Messages.Add(Message.MessCustomerCouldNotBeSaved(isSubscriber, cusno));
            ret.EmailMessage = MiscFunctions.SendStaffMailFailedSaveSubs(_sub, ret);
        }

        /// <summary>
        /// create renewals if possible
        /// </summary>
        public bool TryCreateRenewal(AddCustAndSubReturnObject ret)
        {
            if (PersonFoundBySubscriber == null)
                return false;

            Subscription subRenew = TryGetRenewalSub(PersonFoundBySubscriber.SubsHistory, _sub);
            if (subRenew == null)
                return false;

            _sub.PackageId = subRenew.PackageId;
            _sub.SubsNo = subRenew.SubsNo;
            _sub.ExtNo = subRenew.ExtNo + 1;
            _sub.Subscriber.Cusno = subRenew.Subscriber.Cusno;

            long payerCusno = (_sub.SubscriptionPayer == null) ? 0 : _sub.SubscriptionPayer.Cusno;

            if (SubsIsActive(subRenew) && subRenew.SubsRealEndDate > _sub.SubsStartDate)
                _sub.SubsStartDate = SubscriptionController.GetNextIssuedate(_sub.PaperCode, _sub.ProductNo, subRenew.SubsRealEndDate.AddDays(1));

            _sub.SetSubsDateMembers(true);

            string communeCode = null;
            if (!string.IsNullOrEmpty(_sub.Subscriber.ZipCode))
                communeCode = SubscriptionController.GetCommuneCode(_sub.Subscriber.ZipCode);
            if (string.IsNullOrEmpty(communeCode))
                communeCode = "0180";  //fallback


            long priceListNo = SubscriptionController.GetPriceListNo(_sub.PaperCode, _sub.ProductNo, _sub.SubsStartDate, communeCode, _sub.Pricegroup, _sub.CampNo.ToString());

            //Cirix uses new ExtNo and Kayak uses original ExtNo from subscription that is being renewed!
            var iOrigExtNo = (SubscriptionController.ActiveHandler == SubscriptionController.AvailableHandlers.Kayak) ? subRenew.ExtNo : _sub.ExtNo;
            string cirRet = SubscriptionController.CreateRenewal_DI(_sub.Subscriber.Cusno, _sub.SubsNo, iOrigExtNo, priceListNo, _sub.CampNo, _sub.SubsLenMons, _sub.SubsLenDays,
                _sub.SubsStartDate, _sub.SubsEndDate, _sub.SubsKind, _sub.TotalPriceExVat, _sub.TotalPriceExVat, 1, string.Empty, payerCusno,
                _sub.PackageId, _sub.PaperCode, _sub.ProductNo, "", _sub.TargetGroup, string.Empty, string.Empty, string.Empty, "N", _sub.Pricegroup,
                SubscriptionController.ActiveHandler == SubscriptionController.AvailableHandlers.Kayak ? Settings.InvoiceModeKayakCreditCard : Settings.InvoiceMode_KontoKort);
            if (cirRet != "OK")
            {
                ret.Messages.Add(Message.MessCreateRenewalFailed(PersonFoundBySubscriber.Cusno, _sub.SubsNo, cirRet));
                ret.EmailMessage = MiscFunctions.SendStaffMailFailedSaveSubs(_sub, ret);
                new Logger("TryCreateRenewal / CirixDbHandler.CreateRenewal_DI - failed for cusno:" + PersonFoundBySubscriber.Cusno + ", subsno:" + _sub.SubsNo, cirRet);
                return false;
            }

            return true;
        }


        public Subscription TryGetRenewalSub(List<Subscription> subs, Subscription _sub)
        {
            List<Subscription> sameKindOfSubs = subs.Where(sub => SubsAreOfSameKind(sub, _sub, true)).ToList();
            if (sameKindOfSubs.Count == 0)
                return null;

            //might exist sub that prevents renewal
            List<Subscription> notRenewableSubs = sameKindOfSubs.Where(SubsIsActive)
                                                                .Where(x => SubsIsTidsbestamd(x) == false)
                                                                .ToList();
            if (notRenewableSubs.Count > 0)
                return null;

            //get sub suitable for renewal
            List<Subscription> renewableSubs = sameKindOfSubs.Where(x => SubsIsTidsbestamd(x) || SubsIsActive(x) == false).ToList();
            if (renewableSubs.Count == 0)
                return null;

            return renewableSubs.OrderByDescending(x => x.SubsNo).ThenByDescending(x => x.ExtNo).FirstOrDefault();
        }

        


        public void AddNewSubs(AddCustAndSubReturnObject ret)
        {
            TrySetOldPayersDefaultInvMode();
            UpdateCustomerData(false);
            
            long cusnoPay = (_sub.SubscriptionPayer != null) ? _sub.SubscriptionPayer.Cusno : 0;
            string addSubsRet = SubscriptionController.AddNewSubs2(_sub, cusnoPay, null);
            if (!addSubsRet.StartsWith("FAILED"))
            {
                long subsno = 0;
                if (long.TryParse(addSubsRet, out subsno))
                    _sub.SubsNo = subsno;

                new CustomerPropertyHandler(_sub.Subscriber.Cusno, _sub.Subscriber.Email, _sub.Subscriber.MobilePhone, _sub.Subscriber.SocialSecurityNo, _sub.Subscriber.IsGoldMember, _sub.Subscriber.IsPersonalEx);
            }
            else
            {
                UpdateCustomerData(true);
                ret.Messages.Add(Message.MessAddNewSubsFailed(addSubsRet));
                ret.EmailMessage = MiscFunctions.SendStaffMailFailedSaveSubs(_sub, ret);
                new Logger("ExistingCustHelper.AddNewSubs(): " + addSubsRet + ". Subs details:" + _sub);                
            }
        }

        /// <summary>
        /// set _oldPayersDefInvoiceMode - needed on rollback
        /// </summary>
        public void TrySetOldPayersDefaultInvMode()
        {
            long cusno = _sub.SubscriptionPayer != null ? _sub.SubscriptionPayer.Cusno : _sub.Subscriber.Cusno;
            _oldPayersDefInvoiceMode = SubscriptionController.TryGetDefaultCusInvMode(cusno);
            //new Logger("_oldPayersDefInvoiceMode:" + _oldPayersDefInvoiceMode);
        }

        /// <summary>
        /// call PopulatePersonsInSub() before using this method
        /// </summary>
        public void UpdateCustomerData(bool doRollback)
        {
            try
            {
                HandlePayersInvMode(doRollback);

                if (PersonFoundBySubscriber == null || _dsSubscriber == null)
                    return;

                string oldEmail = Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["EMAILADDRESS"]);
                string oldPhone = Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["O_PHONE"]);
                string oldSocSec = Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["SOCIALSECNO"]);

                if (!doRollback)
                {
                    string inEmail = oldEmail;
                    string inPhone = oldPhone;
                    string inSocSec = oldSocSec;

                    string newEmail = !string.IsNullOrEmpty(_sub.Subscriber.Email) ? _sub.Subscriber.Email : "";
                    string newPhone = !string.IsNullOrEmpty(_sub.Subscriber.MobilePhone) ? _sub.Subscriber.MobilePhone : "";
                    string newSocSec = !string.IsNullOrEmpty(_sub.Subscriber.SocialSecurityNo) ? _sub.Subscriber.SocialSecurityNo : "";

                    bool b1 = RefUpdated(newEmail, ref inEmail);
                    bool b2 = RefUpdated(newPhone, ref inPhone);
                    bool b3 = RefUpdated(newSocSec, ref inSocSec);

                    if (b1 || b2 || b3)
                        UpdateSubscriberCustInfo(inEmail, inPhone, inSocSec);
                }
                else
                    UpdateSubscriberCustInfo(oldEmail, oldPhone, oldSocSec);
            }
            catch (Exception ex)
            {
                new Logger("ExistingCustHelper.UpdateCustomerData() failed", ex.ToString());
            }
        }

        private void HandlePayersInvMode(bool doRollback)
        {
            long cusno = (_sub.SubscriptionPayer != null) ? _sub.SubscriptionPayer.Cusno : _sub.Subscriber.Cusno;

            if (!doRollback)
            {
                //defCusInvMode is autogiro (03), add new invMode (do not change to defCusInvMode)
                if (_oldPayersDefInvoiceMode == Settings.DefaultCustomerInvoiceMode)
                    SubscriptionController.AddNewCusInvmode(cusno, _sub.InvoiceMode, false);
                else
                    SubscriptionController.ChangeCusInvMode(cusno, _sub.InvoiceMode, _oldPayersDefInvoiceMode);
            }

            if (doRollback)
                SubscriptionController.ChangeCusInvMode(cusno, _oldPayersDefInvoiceMode, _sub.InvoiceMode);
        }

        private bool RefUpdated(string newVal, ref string refVal)
        {
            if (newVal.Length > 0 && newVal != refVal)
            {
                refVal = newVal;
                return true;
            }
            return false;
        }

        private void UpdateSubscriberCustInfo(string email, string phone, string socSec)
        {
            try
            {
                long masCusno = 0;
                long.TryParse(Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["MASTERCUSNO"]), out masCusno);

                string result = SubscriptionController.UpdateCustomerInformation(PersonFoundBySubscriber.Cusno,
                                                                        email,
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["H_PHONE"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["W_PHONE"]),
                                                                        phone,
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["SALESDEN"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["OFFERDEN_DIR"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["OFFERDEN_SAL"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["OFFERDEN_EMAIL"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["DENYSMSMARK"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["ACCNO_BANK"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["ACCNO_ACC"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["NOTES"]),
                                                                        Convert.ToInt64(_dsSubscriber.Tables[0].Rows[0]["ECUSNO"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["OTHER_CUSNO"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["WWWUSERID"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["EXPDAY"]),
                                                                        Convert.ToDouble(_dsSubscriber.Tables[0].Rows[0]["DISCPERCENT"]),
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["TERMS"]),
                                                                        socSec,
                                                                        Convert.ToString(_dsSubscriber.Tables[0].Rows[0]["CATEGORY"]),
                                                                        masCusno,
                                                                        "");
            }
            catch (Exception ex)
            {
                new Logger("ExistingCustHelper.UpdateSubscriberCustInfo() failed for cusno:" + PersonFoundBySubscriber.Cusno.ToString(), ex.ToString());
            }
        }


    }

}
