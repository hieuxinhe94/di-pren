using System;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Customer
{
    public class Customer : IDataSetObject
    {
        [DataSet(ColumnName = "CUSNO")]
        public long CustomerNumber { get; set; }

        [DataSet(ColumnName = "FIRSTNAME")]
        public string FirstName { get; set; }

        [DataSet(ColumnName = "LASTNAME")]
        public string LastName { get; set; }

        [DataSet(ColumnName = "ROWTEXT1")]
        public string RowText1 { get; set; }

        [DataSet(ColumnName = "ROWTEXT2")]
        public string RowText2 { get; set; }

        [DataSet(ColumnName = "ROWTEXT3")]
        public string RowText3 { get; set; }

        [DataSet(ColumnName = "BOOKINGDATE")]
        public DateTime BookingDate { get; set; }

        [DataSet(ColumnName = "REVERSELINE")]
        public int ReverseLine { get; set; }

        [DataSet(ColumnName = "OFFERDEN_DIR")]
        public string OfferdenDir { get; set; }

        [DataSet(ColumnName = "OFFERDEN_SAL")]
        public string OfferdenSal { get; set; }

        [DataSet(ColumnName = "COLLECTINV")]
        public string CollectInv { get; set; }

        [DataSet(ColumnName = "H_PHONE")]
        public string HomePhone { get; set; }

        [DataSet(ColumnName = "W_PHONE")]
        public string WorkPhone { get; set; }

        [DataSet(ColumnName = "O_PHONE")]
        public string OfficePhone { get; set; }

        [DataSet(ColumnName = "DISCPERCENT")]
        public float DiscPercent { get; set; }

        [DataSet(ColumnName = "DISCAMOUNT")]
        public float DiscAmount { get; set; }

        [DataSet(ColumnName = "BALANCEAMOUNT")]
        public double BalanceAmount { get; set; }

        [DataSet(ColumnName = "BONUSAMOUNT")]
        public float BonusAmount { get; set; }

        [DataSet(ColumnName = "TERMS")]
        public string Terms { get; set; }

        [DataSet(ColumnName = "TERMSNAME")]
        public string TermsName { get; set; }

        [DataSet(ColumnName = "CURRENCY")]
        public string Currency { get; set; }

        [DataSet(ColumnName = "UNFINISHED")]
        public string Unfinished { get; set; }

        [DataSet(ColumnName = "SALESNO")]
        public string SalesNo { get; set; }

        [DataSet(ColumnName = "ACCNO_BANK")]
        public string AccountNumberBank { get; set; }

        [DataSet(ColumnName = "ACCNO_ACC")]
        public string AccountNumberAccount { get; set; }

        [DataSet(ColumnName = "OTHER_CUSNO")]
        public string OtherCustomerNumber { get; set; }

        [DataSet(ColumnName = "EXPDAY")]
        public string ExpDay { get; set; }

        [DataSet(ColumnName = "LANGUAGE")]
        public string Language { get; set; }

        [DataSet(ColumnName = "SUCCESSCAMPQTY")]
        public int SuccessCampQty { get; set; }

        [DataSet(ColumnName = "CREDITCODE")]
        public string CreditCode { get; set; }

        [DataSet(ColumnName = "INVLAYOUT")]
        public string InvLayout { get; set; }

        [DataSet(ColumnName = "INVCOPYQTY")]
        public int InvCopyQty { get; set; }

        [DataSet(ColumnName = "VATCODE")]
        public string VatCode { get; set; }

        [DataSet(ColumnName = "NOTES")]
        public string Notes { get; set; }

        [DataSet(ColumnName = "SOCIALSECNO")]
        public string SocialSecNo { get; set; }

        [DataSet(ColumnName = "COMPANYID")]
        public string CompanyId { get; set; }

        [DataSet(ColumnName = "MATCH")]
        public string Match { get; set; }

        [DataSet(ColumnName = "CUSTYPE")]
        public string CusType { get; set; }

        [DataSet(ColumnName = "SALESDEN")]
        public string SalesDen { get; set; }

        [DataSet(ColumnName = "WWWUSERID")]
        public string WwwUserId { get; set; }

        [DataSet(ColumnName = "WWWPINCODE")]
        public string WwwPinCode { get; set; }

        [DataSet(ColumnName = "TITLE")]
        public string Title { get; set; }

        [DataSet(ColumnName = "EMAILADDRESS")]
        public string EmailAddress { get; set; }

        [DataSet(ColumnName = "ECUSNO")]
        public decimal ECustomerNumber { get; set; }

        [DataSet(ColumnName = "RECOM_CUSNO")]
        public long RecomCusno { get; set; }

        [DataSet(ColumnName = "RECOM_PRESENTGIVEN")]
        public string RecomPresentGiven { get; set; }

        [DataSet(ColumnName = "RECOM_UNTILDATE")]
        public DateTime RecomUntilDate { get; set; }

        [DataSet(ColumnName = "PROFILEDATE")]
        public DateTime ProfileDate { get; set; }

        [DataSet(ColumnName = "CREDITCARDNO")]
        public string CreditCardNumber { get; set; }

        [DataSet(ColumnName = "PASSWD")]
        public string Password { get; set; }

        [DataSet(ColumnName = "MASTERCUSNO")]
        public long MasterCustomerNumber { get; set; }

        [DataSet(ColumnName = "MASTERNAME")]
        public string MasterName { get; set; }

        [DataSet(ColumnName = "CUSSTATE")]
        public string CusState { get; set; }

        [DataSet(ColumnName = "CUSSTATE_DATE")]
        public DateTime CusStateDate { get; set; }

        [DataSet(ColumnName = "OFFERDEN_EMAIL")]
        public string OfferDenEmail { get; set; }

        [DataSet(ColumnName = "DENYSMSMARK")]
        public string DenySmsMark { get; set; }

        [DataSet(ColumnName = "IBAN_ACCNO")]
        public string IbanAccountNumber { get; set; }

        [DataSet(ColumnName = "CATEGORY")]
        public string Category { get; set; }

        [DataSet(ColumnName = "STREETNAME")]
        public string StreetName { get; set; }

        [DataSet(ColumnName = "HOUSENO")]
        public string HouseNumber { get; set; }

        [DataSet(ColumnName = "APARTMENT")]
        public string Apartment { get; set; }

        [DataSet(ColumnName = "STAIRCASE")]
        public string StairCase { get; set; }

        [DataSet(ColumnName = "STREET1")]
        public string Street1 { get; set; }

        [DataSet(ColumnName = "STREET2")]
        public string Street2 { get; set; }

        [DataSet(ColumnName = "STREET3")]
        public string Street3 { get; set; }

        [DataSet(ColumnName = "ZIPCODE")]
        public string ZipCode { get; set; }

        [DataSet(ColumnName = "COUNTRYCODE")]
        public string CountryCode { get; set; }

        [DataSet(ColumnName = "DOORNO")]
        public long DoorNumber { get; set; }

        [DataSet(ColumnName = "CITYNAME")]
        public string CityName { get; set; }

        [DataSet(ColumnName = "POSTNAME")]
        public string PostName { get; set; }

        [DataSet(ColumnName = "XTRA01")]
        public string Xtra01 { get; set; }

        [DataSet(ColumnName = "XTRA02")]
        public string Xtra02 { get; set; }

        [DataSet(ColumnName = "XTRA03")]
        public string Xtra03 { get; set; }

        [DataSet(ColumnName = "MARKETING_DENIAL")]
        public string MarketingDenial { get; set; }

        [DataSet(ColumnName = "MARKETING_DENIAL_REASON")]
        public string MarketingDenialReason { get; set; }

        [DataSet(ColumnName = "MARKETING_DENIAL_DATE")]
        public DateTime MarketingDenialDate { get; set; }

        [DataSet(ColumnName = "PROFILESOURCEGROUP")]
        public string ProfileSourceGroup { get; set; }

        [DataSet(ColumnName = "PROFILESOURCESPECIFICATION")]
        public string ProfileSourceGroupSpecification { get; set; }

        [DataSet(ColumnName = "PROTECTEDIDENTITY")]
        public string ProtectIdentity { get; set; }

        [DataSet(ColumnName = "STAMP_USER")]
        public string StampUser { get; set; }

        [DataSet(ColumnName = "STAMP_DATE")]
        public DateTime StampDate { get; set; }

        [DataSet(ColumnName = "STAMP_PROGRAM")]
        public string StampProgram { get; set; }

        [DataSet(ColumnName = "TRAFFICLIGHT")]
        public decimal TrafficLight { get; set; }
    }
}
