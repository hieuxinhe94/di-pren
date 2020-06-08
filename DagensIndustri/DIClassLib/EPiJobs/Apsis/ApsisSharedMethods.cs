using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DIClassLib.BonnierDigital;
using DIClassLib.DbHandlers;
using System.Data;

using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DIClassLib.EPiJobs.Apsis
{
    public class ApsisSharedMethods
    {
        MailSenderDbHandler _mailSenderDb;
        private ApsisWsHandler _apsisHandler;
        private const string ValueNo = "N";
        private const string ValueYes = "Y";
        public const string IsActive = "Aktiv";
        public const string IsInActive = "Inaktiv";

        public ApsisSharedMethods()
        {
            _mailSenderDb = new MailSenderDbHandler();
            _apsisHandler = new ApsisWsHandler();
        }
       
        /// <summary>
        /// Set demographic data fields on customer in Apsis account
        /// </summary>
        public string UpdateApsisCustomer(ApsisCustomer customer)
        {
            var data = SetApsisFields(customer);
            //TODO: Nicer design to update HaveServicePlusAccount column somewhere earlier. Need refactoring, but for now, do it here!
            _mailSenderDb.SetHaveServicePlusAccountField(customer.CustomerId, customer.HaveServicePlusAccount);
            //return string.IsNullOrEmpty(data.PaperCode) ? string.Format("({0}) Kunde inte hitta kampanjdata för kunden", customer.Email) : UpdateApsisCustomer(customer, data);

            return UpdateApsisCustomer(customer, data);
        }

        /// <summary>
        /// Get the data as it should be provided to Apsis webservice methods.
        /// If adding/changing the demographic datafields in Apsis they must be added/changed here
        /// aswell to not be overwritten with blank space on update.
        /// DD1=Anvandarnamn, DD2=Status, DD3=Kundnummer, DD4=StartDatum, DD5=Mobilnummer,
        /// DD6=Personligkod, DD7=Pren_DI, DD8=Pren_Pren, DD9=Pren_Info, DD10=Pren_Gasell,
        /// DD11=Pren_Konferens, DD12=Pren_Guld, DD13=Pren_Annons, DD14=ValkomstmailSkickatDatum
        /// DD15=Di_Konto, DD16=PaperCode, DD17=ProductNo, DD18=Subslength, DD19=Product_Description
        /// DD20=Utfall_S2, DD21=Datum_Utfall_S2, DD22=ImportDatum
        /// </summary>
        /// <param name="apsisData">The object filled with the correct values to send</param>
        /// <returns>If providing apsisData as null the key-string is returned to be used in string.Format(), otherwise the valuestring is returned</returns>
        public static string GetApsisDemographicKeysValues(ApsisFields apsisData = null)
        {
            if (apsisData == null)
            {
                return "DD1;DD2;DD3;DD4;DD5;DD6;DD7;DD8;DD9;DD10;DD11;DD12;DD13;DD14;DD15;DD16;DD17;DD18;DD19;DD20;DD21;DD22";
            }
            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21}",
                apsisData.UserName ?? string.Empty,
                apsisData.Status ?? string.Empty,
                apsisData.CustomerNumber ?? string.Empty,
                apsisData.StartDate ?? string.Empty,
                apsisData.MobileNumber ?? string.Empty,
                apsisData.PersonalCode ?? string.Empty,
                apsisData.PrenDi ?? string.Empty,
                apsisData.PrenPren ?? string.Empty,
                apsisData.PrenInfo ?? string.Empty,
                apsisData.PrenGasell ?? string.Empty,
                apsisData.PrenConference ?? string.Empty,
                apsisData.PrenGold ?? string.Empty,
                apsisData.PrenAdvertisment ?? string.Empty,
                apsisData.WelcomeEmailSentDate ?? string.Empty,
                apsisData.DiAccount ?? string.Empty,
                apsisData.PaperCode ?? string.Empty,
                apsisData.ProductNo ?? string.Empty,
                apsisData.SubsLength ?? string.Empty,
                apsisData.ProductDescription ?? string.Empty,
                apsisData.OutcomeS2 ?? string.Empty,
                apsisData.OutcomeS2Date ?? string.Empty,
                apsisData.ImportDate ?? string.Empty
                );
        }
        
        public void SendRegularLetter(int customerId, string email, bool updateByExt)
        {
            ApsisCustomer c = SendRegLettHelper(customerId);
            HandleEmailOnRegularLetter(c, email, null, updateByExt);
        }

        public void SendRegularLetter(int customerId, string email, bool updateByExt, DataSet dsEmailRules)
        {
            ApsisCustomer c = SendRegLettHelper(customerId);
            HandleEmailOnRegularLetter(c, email, dsEmailRules, updateByExt);
        }

        public void UpdateAllEmailsAndSetForceRetry(int customerId, string email, bool forceRetry, bool updateByExt)
        {
            _mailSenderDb.UpdateEmailInMailSenderDb(customerId, email, forceRetry, updateByExt);
            _mailSenderDb.UpdateEmailInCustomer(customerId, email);
            SubscriptionController.UpdateEmailInCirix(customerId, email);
        }

        //2015-02-23 Changing to use customer.InvStartDate instead of customer.SubsStartDate
        public static ApsisFields SetApsisFields(ApsisCustomer customer)
        {
            var data = new ApsisFields();
            try
            {
                data.CustomerHaveOptOut = customer.f_OfferdenEmail != null && customer.f_OfferdenEmail.ToLower() == "y";
                //Y = customer does NOT want any more emails. Will be put to Apsis OptOutAll list
                //If customer is not optout in Cirix, check if customer have optout from MDB
                if (!data.CustomerHaveOptOut)
                {
                    data.CustomerHaveOptOut = MdbDbHandler.IsEmailInOptOut(customer.Email);
                }
                data.CustomerNumber = customer.CustomerId.ToString();
                data.StartDate = (customer.InvStartDate  == DateTime.MinValue)
                    ? ""
                    : customer.InvStartDate.ToString("yyyy-MM-dd");
                data.PersonalCode = MsSqlHandler.GetSsoCustomerCode(customer.CustomerId);
                data.WelcomeEmailSentDate = DateTime.Now.ToString("yyyy-MM-dd");
                var customerHaveServicePlus = BonDigHandler.UserExistInServicePlus(customer.Email);
                customer.HaveServicePlusAccount = customerHaveServicePlus;
                data.DiAccount = customerHaveServicePlus ? ValueYes : ValueNo;
                data.PaperCode = customer.f_PaperCode;
                data.ProductNo = customer.f_ProductNo;
                data.SubsLength = customer.SubsLenMonsFromCirix.ToString();
                data.ActiveInCirix = SubscriptionController.CustomerIsActive(customer.CustomerId, customer.f_PaperCode,
                    customer.f_ProductNo);
                data.ProductDescription = (!string.IsNullOrEmpty(customer.f_PaperCode) &&
                                           !string.IsNullOrEmpty(customer.f_ProductNo))
                    ? SubscriptionController.GetProductName(customer.f_PaperCode, customer.f_ProductNo)
                    : string.Empty;
                if (!string.IsNullOrEmpty(data.ProductDescription))
                {
                    return data;
                }

                //If papercode/productno/productName is not set on customer, try find it from campaign
                //Is customer related to a campaign, then get all details from that campaign?  

                if (string.IsNullOrEmpty(customer.CampId))
                {
                    return data;
                }

                var camp = SubscriptionController.GetCampaign(customer.CampId);
                if (!DbHelpMethods.DataSetHasRows(camp))
                {
                    return data;
                }
                data.PaperCode = camp.Tables[0].Rows[0]["PAPERCODE"].ToString();
                data.ProductNo = camp.Tables[0].Rows[0]["PRODUCTNO"].ToString();
                data.ActiveInCirix = SubscriptionController.CustomerIsActive(customer.CustomerId, data.PaperCode,
                    data.ProductNo);
                data.ProductDescription = SubscriptionController.GetProductName(data.PaperCode, data.ProductNo);

                int subsLength;
                if (!int.TryParse(camp.Tables[0].Rows[0]["SUBSLENGTH"].ToString(), out subsLength))
                {
                    data.SubsLength = "-1";
                    return data;
                }

                var subsLengthInMonths = subsLength;
                var lengthUnit = camp.Tables[0].Rows[0]["LENGTHUNIT"].ToString();
                switch (lengthUnit.ToUpper())
                {
                    case "DD":
                    case "IQ":
                    case "WW":
                        subsLengthInMonths = 0;
                        break;
                    case "YY":
                        subsLengthInMonths = subsLength*12;
                        break;
                    case "":
                        subsLengthInMonths = 1;
                        break;
                }
                data.SubsLength = subsLengthInMonths.ToString();
            }
            catch (Exception ex)
            {
                new Logger(
                    string.Format("ApsisSharedMethods.SetApsisFields() failed, cusno:{0} Email:{1}", customer.CustomerId,
                        customer.Email), ex.ToString());
            }
            return data;
        }

        public void UpdateDeclineInfoOnCustomer(string email,ApsisFields data)
        {
            var tmpCust = new ApsisCustomer() {Email = email};
            UpdateApsisCustomerDataField(tmpCust, "20", data.OutcomeS2);
            UpdateApsisCustomerDataField(tmpCust, "21", data.OutcomeS2Date);
        }

        #region Private methods
        
        private string UpdateApsisCustomer(ApsisCustomer customer, ApsisFields data)
        {
            var errors = new StringBuilder();
            
            errors.Append(UpdateApsisCustomerDataField(customer, "2", data.ActiveInCirix ? IsActive : IsInActive));
            errors.Append(UpdateApsisCustomerDataField(customer, "15", data.DiAccount));
            errors.Append(UpdateApsisCustomerDataField(customer, "16", data.PaperCode ?? string.Empty));
            errors.Append(UpdateApsisCustomerDataField(customer, "17", data.ProductNo ?? string.Empty));
            errors.Append(UpdateApsisCustomerDataField(customer, "18", data.SubsLength ?? string.Empty));
            errors.Append(UpdateApsisCustomerDataField(customer, "19", data.ProductDescription ?? string.Empty));
            if (data.CustomerHaveOptOut && !string.IsNullOrEmpty(customer.Email))
            {
                var result = _apsisHandler.MoveSubscriberToOptOutAll(customer.Email);
                if (result != 1)
                {
                    errors.Append(string.Format("Could not add {0} to Apsis OptOut list!", customer.Email));
                }
            }
            return errors.ToString().Trim();
        }
        
        /// <summary>
        /// Update one demographic datafield on a Apsis customer
        /// </summary>
        /// <param name="customer">ApsisCustomer object</param>
        /// <param name="key">Number of the demographic datafield as string e.g. "15"</param>
        /// <param name="value">String value of the demographic datafield</param>
        /// <returns></returns>
        private string UpdateApsisCustomerDataField(ApsisCustomer customer, string key, string value)
        {
            var result = _apsisHandler.UpdateSubscriberData(string.Empty, customer.Email, key, value);
            if (result == -1000)
            {
                return "Om ett anrop till API genererar felkod - 1000 betyder detta att kontot i APSIS Pro, som krävs för att göra kopplingen, inte finns";
            }
            if (result == 1)
            {
                return string.Empty;
            }
            var errorTemplate = GetErrorMessageTemplate(result) + Environment.NewLine;
            return string.Format(errorTemplate, customer.Email);
        }

        /// <summary>
        /// Return template including Apsis error message that assume 1 parameter to be provided if used with string.Format()
        /// </summary>
        /// <param name="errorCode">Apsis error code -1 ot -7</param>
        /// <returns></returns>
        private static string GetErrorMessageTemplate(int errorCode)
        {
            switch (errorCode)
            {
                case -1:
                    return "({0}) Internt serverfel - Det fanns ett problem med API-servern. Vänligen kontakta APSIS support med begäran för att lösa problemet.";
                case -2:
                    return "({0}) Valideringsfel - En eller fler parameterar i begäran är felaktig.";
                case -3:
                    return "({0}) Hittades inte - URL'en är felaktig. Ingen API-metod hittades som kunde hantera begäran.";
                case -4:
                    return "({0}) Obehörig - behörighet saknas för att kunna utföra API begäran, exempelvis p.g.a. en felaktig API-nyckel.";
                case -5:
                    return "({0}) Upptagen - API servern är för tillfället inte tillgänglig.";
                case -6:
                    return "({0}) API inaktiverad - API-tjänsten är tillfälligt otillgänglig p.g.a. underhållsarbete.";
                case -7:
                    return "({0}) Dålig förfrågan - begäran felaktigt formaterad.";
                default:
                    return "({0}) Okänt fel uppstod.";
            }
        }
        
        private ApsisCustomer SendRegLettHelper(int customerId)
        {
            ApsisCustomer c = _mailSenderDb.GetCustomer(customerId);
            List<ApsisCustomer> custs = new List<ApsisCustomer>();
            custs.Add(c);
            SubscriptionController.FlagCustsInLetter(custs, ValueNo);

            _mailSenderDb.SetDateRegularLetter(customerId);

            return c;
        }
        
        private void HandleEmailOnRegularLetter(ApsisCustomer c, string newEmail, DataSet dsEmailRules, bool updateByExt)
        {
            //will not try to send new mail to customer
            bool retry_FALSE = false;

            //delete not vaild email
            if (!MiscFunctions.IsValidEmail(newEmail))
            {
                UpdateAllEmailsAndSetForceRetry(c.CustomerId, "", retry_FALSE, updateByExt);
                return;
            }

            //email not changed
            if (newEmail.ToLower() == c.Email.ToLower())
            {
                if (dsEmailRules == null)
                    dsEmailRules = _mailSenderDb.GetEmailRules();

                //remove [NOT RULE]@... (most likely a bounce address)
                if (_mailSenderDb.EmailNotEmptyAndPassesRules(newEmail, dsEmailRules))
                    UpdateAllEmailsAndSetForceRetry(c.CustomerId, "", retry_FALSE, updateByExt);

                //keep [RULE]@... address

            }
            else //email changed
            {
                UpdateAllEmailsAndSetForceRetry(c.CustomerId, newEmail, retry_FALSE, updateByExt);
            }
        }

        #endregion
    }
}
