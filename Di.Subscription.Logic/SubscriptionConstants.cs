namespace Di.Subscription.Logic
{
    public class SubscriptionConstants
    {
        public const string DefaultUserId = "DIWEB";
        public const string DefaultLanguage = "SV";

        #region PaperCodes

        public const string PaperCodeAll = "ALL";
        public const string PaperCodeDi = "DI";
        public const string PaperCodeIpad = "IPAD";
        public const string PaperCodeDise = "DISE";

        #endregion

        #region Vat

        public const decimal VatPercentDi = 6;
        public const decimal VatPercentIpad = 25;

        #endregion

        #region ProductNos

        public const string ProductNoDi = "01";
        public const string ProductNoIpad = "IPAD";
        public const string ProductNoWeekend = "05";

        #endregion

        #region SubsKind

        public const string SubsKindTimed = "02";

        public const string SubsKindOngoing = "01";

        #endregion

        public const string DefaultRecieveType = "04";

        public const string Source = "CRX";

        public const string CodeGroupTargetGroups = "TARGETGRPS";
        public const string CodeGroupReceiveTypes = "RCVTYPE";

        public const string PropertyCodeEmail = "22";
        public const string PropertyCodeMobilePhone = "23";
        public const string PropertyCodeApproval = "94";

    }
}
