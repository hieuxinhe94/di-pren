namespace DIClassLib.Subscriptions.AddCustAndSub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DIClassLib.Misc;

    /// <summary>
    /// 
    /// </summary>
    public class Message
    {
        //public int ErrorCode { get; set; }
        public string MessageCustomer { get; set; }
        public CustomerMessageType CustMessageType { get; set; }
        public string MessageSweStaff { get; set; }
        public string MessageTechStaff { get; set; }


        #region static messages
        public static string ErrMess_DenyShortSub
        {
            get
            {
                return "Du kan dessvärre bara prova på en gång per " + Settings.MinMonthsSinceLastTrial + "-månadersperiod. Enligt våra uppgifter har du redan haft en provperiod. Du kan alltid teckna dig för något av våra <a href='http://www.di.se/pren/kampanj/prensidorna/'>ordinarie erbjudanden</a>. Har du några frågor är du varmt välkommen att kontakta kundtjänst på tel 08-573 651 00 eller mejla till <a href='mailto:pren@di.se'>pren@di.se</a>.";
            }
        }

        public static string ErrMess_DenyShortSubAgenda
        {
            get
            {
                return "Du kan dessvärre bara prova på en gång per " + Settings.MinMonthsSinceLastTrialAgenda + "-månadersperiod. Enligt våra uppgifter har du redan haft en provperiod. Du kan alltid teckna dig för något av våra <a href='http://www.di.se/pren/kampanj/prensidorna/'>ordinarie erbjudanden</a>. Har du några frågor är du varmt välkommen att kontakta kundtjänst på tel 08-573 651 00 eller mejla till <a href='mailto:pren@di.se'>pren@di.se</a>.";
            }
        }

        public static string ErrMess_ContactSupport
        {
            get
            {
                return "Var god kontakta kundtjänst på tel 08-573 651 00 eller mejla till <a href='mailto:pren@di.se'>pren@di.se</a>.";
            }
        }

        public static string ErrMess_DiWillSolveProblemElseContactCust
        {
            get
            {
                return "Tack för din beställning! Ett tekniskt problem uppstod men vi kommer att hantera din beställning enligt önskemål. Skulle vi behöva kompletterande information så kontaktar vi dig.";
            }
        }
        #endregion


        public Message() { }

        public Message(CustomerMessageType custMessType, string messCust, string messSweStaff, string messTechStaff)
        {
            //ErrorCode = errCode;
            CustMessageType = custMessType;
            MessageCustomer = messCust;
            MessageSweStaff = messSweStaff;
            MessageTechStaff = messTechStaff;
        }

        public override string ToString()
        {
            //return base.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("<i>CustMessageType:</i> " + CustMessageType.ToString() + "<br>");
            sb.Append("<i>MessageCustomer:</i> " + MessageCustomer + "<br>");
            sb.Append("<i>MessageSweStaff:</i> " + MessageSweStaff + "<br>");
            sb.Append("<i>MessageTechStaff:</i> " + MessageTechStaff + "<br><br>");
            return sb.ToString();
        }


        #region return messages
        internal static Message MessValidateSubMissingCampId()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Tekniskt fel: kampanj ID saknas. " + ErrMess_ContactSupport,
                "Kampanj ID saknas",
                "Campaign ID is missing");
        }

        internal static Message MessValidateSubCampIdNotValid(Subscription sub)
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Tekniskt fel: kampanjfakta hittades inte för kampanj ID: " + sub.CampId + ". " + ErrMess_ContactSupport,
                "Kampanjfakta hittades inte för kampanj ID: " + sub.CampId,
                "Campaign facts was not found for campaign ID: " + sub.CampId);
        }

        internal static Message MessValidateSubscriberFirstName()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Prenumerantens förnamn måste anges",
                "",
                "The subscribers first name is mandatory");
        }

        internal static Message MessValidateSubscriberLastName()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Prenumerantens efternamn måste anges",
                "",
                "The subscribers last name is mandatory");
        }

        internal static Message MessValidateSubscriberEmail()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Prenumerantens epostadress måste anges",
                "",
                "The subscribers email address is mandatory");
        }

        internal static Message MessValidateSubscriberEmailFormat()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Prenumerantens epostadress är ogiltig",
                "",
                "The subscribers email address is not valid");
        }

        internal static Message MessValidateSubscriberPhoneFormat()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Prenumerantens mobilnummer är ogiltigt (måste vara ett svenskt nummer)",
                "",
                "The subscribers mobile phone number is not valid (must be a Swedish number)");
        }

        internal static Message MessValidateSubscriberZipFormat()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Prenumerantens postnummer är ogiltigt (måste bestå av 5 siffror)",
                "",
                "The subscribers zip code is not valid (must consist of 5 digits)");
        }

        internal static Message MessValidateSubscriberStreet()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Prenumerantens gatuadress måste anges",
                "",
                "The subscribers street address is mandatory");
        }

        internal static Message MessValidateSubscriberZip()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Prenumerantens postnummer måste anges",
                "",
                "The subscribers zip code is mandatory");
        }

        internal static Message MessValidatePayerPhone()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Betalarens telefonnummer måste anges",
                "",
                "The payers phone number is mandatory");
        }

        internal static Message MessValidatePayerPhoneFormat()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Betalarens telefonnummer är ogiltigt (måste vara ett svenskt nummer)",
                "",
                "The payers phone number is not valid (must be a Swedish phone number)");
        }

        internal static Message MessValidatePayerZip()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Betalarens postnummer måste anges",
                "",
                "The payers zip code is mandatory");
        }

        internal static Message MessValidatePayerZipFormat()
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Betalarens postnummer är ogiltigt (måste bestå av 5 siffror)",
                "",
                "The payers zip code is not valid (must consist of 5 digits)");
        }

        
        
        internal static Message MessTrialPeriodDenied(long cusno)
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                ErrMess_DenyShortSub,
                "Provprenumeration nekades eftersom kunden redan haft en provperiod de senaste " + Settings.MinMonthsSinceLastTrial + " månaderna. Kundnr: " + cusno,
                "Trial subscription (short time subscription) was denied because the customer has had another trial period during the last " + Settings.MinMonthsSinceLastTrial + " months.");
        }

        internal static Message MessTrialPeriodDeniedAgenda(long cusno)
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                ErrMess_DenyShortSubAgenda,
                "Provprenumeration nekades eftersom kunden redan haft en provperiod för Agenda de senaste " + Settings.MinMonthsSinceLastTrialAgenda + " månaderna. Kundnr: " + cusno,
                "Trial subscription (short time subscription) was denied because the customer has had another trial period during the last " + Settings.MinMonthsSinceLastTrialAgenda + " months.");
        }

        internal static Message MessSubscriberHasHadDifferentRole(long cusno)
        {
            return new Message(
                CustomerMessageType.FriendlyMessage,
                ErrMess_DiWillSolveProblemElseContactCust,
                "Antingen har prenumeranten tidigare haft rollen betalare, ELLER så kunde inte tidigare roll utläsas. Kundnr: " + cusno,
                "The customer wants to be a subscriber, but has previously been a payer. Subscription system cannot handle this. Customer support will solve the problem.");
        }

        internal static Message MessPayerHasHadDifferentRole(long cusno)
        {
            return new Message(
                CustomerMessageType.FriendlyMessage,
                ErrMess_DiWillSolveProblemElseContactCust,
                "Antingen har betalaren tidigare haft rollen prenumerant, ELLER så kunde inte tidigare roll utläsas. Kundnr: " + cusno,
                "The customer wants to be a payer, but has previously been a subscriber. Subscription system cannot handle this. Customer support will solve the problem.");
        }

        internal static Message MessSubscriberHasActiveSubOfSameKind(long cusno)
        {
            return new Message(
                CustomerMessageType.ErrorMessage,
                "Vi ser att du redan har en aktiv pågående prenumeration och därför har inte denna beställning gått igenom. Behöver du ändå komma i kontakt med kundtjänst så når du oss på tel: 08-573 651 00 alt <a href='mailto:pren@di.se'>pren@di.se</a>.",
                "Prenumeranten har redan en aktiv prenumeration av samma typ. Kundnr: " + cusno,
                "The subscriber already has an active subscription of the same kind.");
        }
        
        internal static Message MessCustomerCouldNotBeSaved(bool isSubscriber, long cusno)
        {
            return new Message(
                CustomerMessageType.FriendlyMessage,
                ErrMess_DiWillSolveProblemElseContactCust,
                GetCustTypeName(isSubscriber, true) + " kunde inte sparas i prenumerationssystemet. Teknisk information: " + GetAddCustErrMess(cusno, true),
                GetCustTypeName(isSubscriber, false) + " could not be saved to Crix. Technical infomation: " + GetAddCustErrMess(cusno, false));
        }

        internal static Message MessCreateRenewalFailed(long subscriberCusno, long subRenewSubsno, string cirixReturn)
        {
            return new Message(
                CustomerMessageType.FriendlyMessage,
                ErrMess_DiWillSolveProblemElseContactCust,
                "Skapa förnyelse misslyckades. Kundnr: " + subscriberCusno + ", prennr: " + subRenewSubsno + ", prensystemfel: " + cirixReturn + ". Försök åtgärda detta manuellt.",
                "Create renewal of sub failed. Staff will handle it.");
        }
        
        internal static Message MessAddNewSubsFailed(string cirixReturn)
        {
            return new Message(
                CustomerMessageType.FriendlyMessage,
                ErrMess_DiWillSolveProblemElseContactCust,
                "Det gick inte att spara prenumerationen i prenumerationssystemet. Felmeddelande: " + cirixReturn,
                "Save subscription to subscription system failed. Staff will handle it.");
        }

        internal static Message MessFailedToCreateImportInServicePlus(long cusno, long subsno)
        {
            return new Message(
                CustomerMessageType.FriendlyMessage,
                "Tack för din beställning! Tyvärr uppstod ett tekniskt fel som gör att det kan dröja upp till fem timmar innan ditt konto aktiveras. För att omgående ta del av våra digitala tjänster så kontakta kundtjänst på tel: 08-573 651 00 alt <a href='mailto:pren@di.se'>pren@di.se</a>.",
                "Att spara prenumerationen som entitlement i S+ misslyckades. Det kan ta upp till 5 timmar innan kunden synkas och får samtliga behörigheter. Prensystem info - kundnr: " + cusno + ", prennr: " + subsno,
                "Call to Service plus method 'createImport' failed. User will probably not have correct entitlements before sync from subscription system to S+ has been executed next time. Info - cusno: " + cusno + ", subsno: " + subsno);
        }

        internal static Message MessMultipleCirixCusnosInServicePlus(List<long> cusnos)
        {
            var sb = new StringBuilder();
            foreach (var cusno in cusnos)
                sb.Append(cusno + " ");

            return new Message(
                CustomerMessageType.ErrorMessage,
                ErrMess_ContactSupport,
                "Kunden har flera kundnummer i prenumerationssystemet kopplade till samma Di-konto i S+ (" + sb.ToString() + ")",
                "The customer has multiple cusno's in the subscription system connected to the S+ account (" + sb.ToString() + ")");
        }



        private static string GetCustTypeName(bool isSubscriber, bool inSwedish)
        {
            if (isSubscriber)
                return (inSwedish) ? "Prenumeranten" : "The subscriber";

            return (inSwedish) ? "Betalaren" : "The payer";
        }

        private static string GetAddCustErrMess(long errCode, bool inSwedish)
        {
            if (errCode == -10)
                return (inSwedish) ? "Tekniskt fel (exception)." : "Technical error (exception).";

            if (errCode == -2)
                return (inSwedish) ? "Webbservicetransaktionen falerade." : "Webservice transaction falied.";

            if (errCode == -1)
                return (inSwedish) ? "Identisk kund finns redan i prenumerationssystemet." : "Identical customer already exist in the subscription system.";

            return string.Empty;
        }
        #endregion
    }


    public enum CustomerMessageType
    {
        ErrorMessage = 0,
        FriendlyMessage
    }
}
