using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DIClassLib.DbHandlers;

namespace DIClassLib.Misc
{
	public static class Settings
	{
	    public static string PriceGroupTrialFree {
	        get { return "43"; }
	    }

	    public static string Nets_MerchantId { get { return ConfigurationManager.AppSettings["nets_merchantId"]; } }
		public static string Nets_Passwd { get { return ConfigurationManager.AppSettings["nets_passwd"]; } }
		public static string Nets_Token { get { return ConfigurationManager.AppSettings["nets_token"]; } }
		public static string Nets_CurrencyCode { get { return ConfigurationManager.AppSettings["nets_currencyCode"]; } }
		public static string Nets_Language { get { return ConfigurationManager.AppSettings["nets_language"]; } }
		public static string Nets_UrlTerminal { get { return ConfigurationManager.AppSettings["nets_urlTerminal"]; } }
		public static string Nets_UrlRegister { get { return ConfigurationManager.AppSettings["nets_urlRegister"]; } }
		public static string Nets_UrlQuery { get { return ConfigurationManager.AppSettings["nets_urlQuery"]; } }
		public static string Nets_UrlProcess { get { return ConfigurationManager.AppSettings["nets_urlProcess"]; } }

		public static int CacheTimeSecondsShort
		{
			get
			{
				int tmpInt;
				if (int.TryParse(ConfigurationManager.AppSettings["CacheTimeSecondsShort"], out tmpInt))
				{
					return tmpInt;
				}
				return 60;
			}
		}

		public static int CacheTimeSecondsMedium
		{
			get
			{
				int tmpInt;
				if (int.TryParse(ConfigurationManager.AppSettings["CacheTimeSecondsMedium"], out tmpInt))
				{
					return tmpInt;
				}
				return 180;
			}
		}

		public static int CacheTimeSecondsLong
		{
			get
			{
				int tmpInt;
				if (int.TryParse(ConfigurationManager.AppSettings["CacheTimeSecondsLong"], out tmpInt))
				{
					return tmpInt;
				}
				return 600;
			}
		}

		/// <summary>
		/// Default value 60
		/// </summary>
		public static int CacheTimeMinutesLong
		{
			get
			{
				int tmpInt;
				if (int.TryParse(ConfigurationManager.AppSettings["CacheTimeMinutesLong"], out tmpInt))
				{
					return tmpInt;
				}
				return 60;
			}
		}

		public static bool DoApsisUpdates
		{
			get
			{
				bool tmpBool;
				if (bool.TryParse(ConfigurationManager.AppSettings["DoApsisUpdates"], out tmpBool))
				{
					return tmpBool;
				}
				return false;
			}
		}

		public static bool UseCirixHandler
		{
			get
			{
				//cirixhandler is DEAD
				//bool tmpBool;
				//if (bool.TryParse(ConfigurationManager.AppSettings["UseCirixHandler"], out tmpBool))
				//{
				//	return tmpBool;
				//}
				return false;
			}
		}

		/// <summary>
		/// TRUE: money will be transferred from customers account. 
		/// FALSE: money will NOT be transferred from customers account (payment is only registred). 
		/// Set to FALSE if you want to do payment tests that does not transfer money.
		/// </summary>
		public static bool Nets_ProcessSale { get { return (ConfigurationManager.AppSettings["nets_processSale"].ToLower() == "true"); } }

		/// <summary>
		/// Used if no nets transaction can be found for provided transactionId. 
		/// Occurs if user manipulates transactionId value in nets return url.
		/// </summary>
		public static string Nets_Err_TransNotFoundInNets { get { return "-1"; } }

		public static string Country { get { return "SE"; } }


		public static string sSalesNo { get { return ConfigurationManager.AppSettings["camp_sSalesNo"]; } }
		public static string sTargetGroup { get { return ConfigurationManager.AppSettings["camp_sTargetGroup"]; } }
		public static string sReceiveType { get { return ConfigurationManager.AppSettings["camp_sReceiveType"]; } }
		public static double dblDiscAmount { get { return double.Parse(ConfigurationManager.AppSettings["camp_dblDiscAmount"]); } }
		public static string sCirculation { get { return ConfigurationManager.AppSettings["camp_sCirculation"]; } }
		public static string sCollectInv { get { return ConfigurationManager.AppSettings["camp_sCollectInv"]; } }
		public static string sCusType { get { return ConfigurationManager.AppSettings["camp_sCusType"]; } }      //comment 2
		public static string sCusTypeCorp { get { return ConfigurationManager.AppSettings["camp_sCusTypeCorp"]; } }  //comment 2
		public static string sNotes { get { return ConfigurationManager.AppSettings["camp_sNotes"]; } }
		public static string sExpDay { get { return ConfigurationManager.AppSettings["camp_sExpDay"]; } }
		public static string sTerms { get { return ConfigurationManager.AppSettings["camp_sTerms"]; } }
		public static string sCategory { get { return ConfigurationManager.AppSettings["camp_sCategory"]; } }
		public static long lMasterCusno { get { return long.Parse(ConfigurationManager.AppSettings["camp_lMasterCusno"]); } }

		public static double VatPaper { get { return double.Parse(ConfigurationManager.AppSettings["DI_VATPercentage"]); } }
		public static double VatIpad { get { return double.Parse(ConfigurationManager.AppSettings["IPAD_VATPercentage"]); } }
		public static double VatGasellFlow { get { return double.Parse(ConfigurationManager.AppSettings["GasellFlow_VATPercentage"]); } }

		public static int MinMonthsSinceLastTrial
		{
			get
			{
				int returnvalue;
				if (!int.TryParse(ConfigurationManager.AppSettings["MinMonthsSinceLastTrial"], out returnvalue))
				{
					returnvalue = 3;
				}
				return returnvalue;
			}
		}

		public static int MinMonthsSinceLastTrialAgenda
		{
			get
			{
				int returnvalue;
				if (!int.TryParse(ConfigurationManager.AppSettings["MinMonthsSinceLastTrialAgenda"], out returnvalue))
				{
					returnvalue = 3;
				}
				return returnvalue;
			}
		}

		// CIRIX only:
		public static string InvoiceMode_AutoGiro { get { return "04"; } }
		public static string InvoiceMode_BankGiro { get { return "04"; } }
		public static string InvoiceMode_KontoKort { get { return "05"; } }
		// END CIRIX only:

		// Kayak only:
		public static string InvoiceModeKayakAutoGiro { get { return "01"; } }     //real value is "03". Staff changes to "03" in Kayak when autogiro paper approval arrives.
		public static string InvoiceModeKayakBankGiro { get { return "01"; } }
		public static string InvoiceModeKayakCreditCard { get { return "05"; } }
		// END Kayak only:

		public static string DefaultCustomerInvoiceMode
		{
			get { return InvoiceModeKayakAutoGiro; }
		}

		public static string PaperCode_DI { get { return "DI"; } }
		public static string PaperCode_IPAD { get { return "IPAD"; } }
		public static string PaperCode_DISE { get { return "DISE"; } }
		public static string PaperCode_DIY { get { return "DIY"; } }
		public static string PaperCode_AGENDA { get { return "AGENDA"; } }

		public static string ProductNo_Regular { get { return "01"; } }
		public static string ProductNo_IPAD { get { return "IPAD"; } }
		public static string ProductNo_Weekend { get { return "05"; } }
		public static string ProductNo_Agenda_Energy { get { return "01"; } }
		public static string ProductNo_Agenda_HealthCare { get { return "02"; } }
		public static string ProductNo_Agenda_ImportExport { get { return "03"; } }

		public static string SubscriberPrivate { get { return "01"; } }
		public static string SubscriberCompany { get { return "02"; } }

		/// <summary>
		/// 00=not active yet, 01=active, 02=break, 30=renewal not active yet
		/// </summary>
		public static List<string> SubsStateActiveValues = new List<string> { "00", "01", "02", "30" };

		public static string GetName_Product(string paperCode, string productNo)
		{
			if (paperCode == PaperCode_DI)
			{
				if (productNo == ProductNo_Regular)
					return "Di 6-dagars";

				if (productNo == ProductNo_Weekend)
					return "Di Weekend";

				//if (productNo == "02")
				//    return "Di taltidning";
			}

			if (paperCode == PaperCode_IPAD)
				return "Dagens industri i läsplatta";

			if (paperCode == PaperCode_DISE)
				return PaperCode_DISE;

			if (paperCode == PaperCode_DIY)
				return "Dagens industri Y";

			if (paperCode == PaperCode_AGENDA)
			{
				if (productNo == ProductNo_Agenda_Energy)
					return "Agenda Energi";

				if (productNo == ProductNo_Agenda_HealthCare)
					return "Agenda Vård och omsorg";

				if (productNo == ProductNo_Agenda_ImportExport)
					return "Agenda Import och export";
			}

			return string.Empty;    //"? (paperCode:" + paperCode + ", productNo:" + productNo + ")";
		}

		public static string GetName_InvoiceMode(string invMode)
		{
			if (SubscriptionController.ActiveHandler == SubscriptionController.AvailableHandlers.Kayak)
			{
				if (invMode == InvoiceModeKayakBankGiro)
					return "Bankgiro";

				if (invMode == InvoiceModeKayakAutoGiro)
					return "Autogiro";

				if (invMode == InvoiceModeKayakCreditCard)
					return "Kontokort";
			}

			if (invMode == InvoiceMode_BankGiro)
				return "Bankgiro";

			if (invMode == InvoiceMode_KontoKort)
				return "Kontokort";

			return "? (" + invMode + ")";
		}

		public static string GetName_SubsKind(string subsKind)
		{
			if (subsKind == SubsKind_tillsvidare)
				return "Tillsvidare";

			if (subsKind == SubsKind_tidsbestamd)
				return "Tidsbestämd";

			if (subsKind == SubsKind_kontrollex)
				return "Kontrollex";

			if (subsKind == SubsKind_losnummer)
				return "Lösnummer";

			if (subsKind == SubsKind_friex)
				return "Friex";

			return "? (" + subsKind + ")";
		}

		public const string SubsKind_tillsvidare = "01";
		public const string SubsKind_tidsbestamd = "02";
		public const string SubsKind_kontrollex = "03";
		public const string SubsKind_losnummer = "04";
		public const string SubsKind_friex = "05";

		//corresponds to value in db table: dbDagensIndustriMISC / LogEventDescr
		//(add values in that table when values are added here)
		public static int LogEvent_TempAddressChange { get { return 1; } }
		public static int LogEvent_HolidayStop { get { return 2; } }
		public static int LogEvent_PermAddressChange { get { return 3; } }
		public static int LogEvent_FreeDi { get { return 4; } }
		public static int LogEvent_CompanyCalendar { get { return 5; } }
		public static int LogEvent_CompanySearch { get { return 6; } }
		public static int LogEvent_RosenRummetBooking { get { return 7; } }


		//used on campaign pages
		public static int TargetGroupType_Regular { get { return 1; } }   //comes to camp page without url arg
		public static int TargetGroupType_Email { get { return 2; } }   //comes to camp page with code in url
		public static int TargetGroupType_Postal { get { return 3; } }   //types code on page ...url/camp/adr
		public static int TargetGroupType_Mobile { get { return 4; } }

		public static int PhoneMaxNoOfDigits { get { return 20; } }


		private static List<long> cusnosCanReadTomorrowPdf = new List<long>();
		public static List<long> CusnosAllowReadPdf
		{
			get
			{
				if (cusnosCanReadTomorrowPdf.Count == 0)
				{
					string[] arr = ConfigurationManager.AppSettings["CusnosAllowReadPdf"].Split(',');
					foreach (string cusno in arr)
						if (MiscFunctions.IsNumeric(cusno))
							cusnosCanReadTomorrowPdf.Add(long.Parse(cusno));
				}

				return cusnosCanReadTomorrowPdf;
			}
		}


		public static string AdressChangeType_Current { get { return "Current"; } }
		public static string AdressChangeType_Definitive { get { return "Definitive"; } }
		public static string AdressChangeType_Temporary { get { return "Temporary"; } }
		public static string AdressChangeType_Splitted { get { return "Splitted"; } }

		public static List<int> CampEpiPageIdsRedirToMobPage
		{
			get
			{
				List<int> epiIds = new List<int>();
				string[] arr = ConfigurationManager.AppSettings["CampEpiPageIdsRedirToMobPage"].Split(',');
				foreach (string id in arr)
					if (MiscFunctions.IsNumeric(id))
						epiIds.Add(int.Parse(id));

				return epiIds;
			}
		}

		public static string GetBonDigProductId(string paperCode, string productNo)
		{
			if (paperCode == PaperCode_DI)
			{
				if (productNo == ProductNo_Regular)
					return ConfigurationManager.AppSettings["BonDigProductIdDi6Days"];

				if (productNo == ProductNo_Weekend)
					return ConfigurationManager.AppSettings["BonDigProductIdDiWeekend"];

				if (productNo == ProductNo_IPAD)
					return ConfigurationManager.AppSettings["BonDigProductIdIpad"];
			}

			if (paperCode == PaperCode_IPAD)
				return ConfigurationManager.AppSettings["BonDigProductIdIpad"];

			if (paperCode == PaperCode_DISE)
				return ConfigurationManager.AppSettings["BonDigProductIdDise"];

			if (paperCode == PaperCode_AGENDA)
			{
				if (productNo == ProductNo_Agenda_Energy)
					return ConfigurationManager.AppSettings["BonDigProductIdAgendaEnergy"];

				if (productNo == ProductNo_Agenda_HealthCare)
					return ConfigurationManager.AppSettings["BonDigProductIdAgendaHealthCare"];

				if (productNo == ProductNo_Agenda_ImportExport)
					return ConfigurationManager.AppSettings["BonDigProductIdAgendaImportExport"];
			}

			return string.Empty;
		}

		private static string BonDigExtProdIdPaper6Days { get { return ConfigurationManager.AppSettings["BonDigExtProdIdPaper6Days"]; } }
		private static string BonDigExtProdIdPaperWeekend { get { return ConfigurationManager.AppSettings["BonDigExtProdIdPaperWeekend"]; } }
		private static string BonDigExtProdIdIpad { get { return ConfigurationManager.AppSettings["BonDigExtProdIdIpad"]; } }
		private static string BonDigExtProdIdIpadKayak { get { return ConfigurationManager.AppSettings["BonDigExtProdIdIpadKayak"]; } }
		private static string BonDigExtProdIdDise { get { return ConfigurationManager.AppSettings["BonDigExtProdIdDise"]; } }
		private static string BonDigExtProdIdAgendaEnergy { get { return ConfigurationManager.AppSettings["BonDigExtProdIdAgendaEnergy"]; } }
		private static string BonDigExtProdIdAgendaHealthCare { get { return ConfigurationManager.AppSettings["BonDigExtProdIdAgendaHealthCare"]; } }
		private static string BonDigExtProdIdAgendaImportExport { get { return ConfigurationManager.AppSettings["BonDigExtProdIdAgendaImportExport"]; } }
		public static string BonDigExtRescIdPdfPaper { get { return ConfigurationManager.AppSettings["BonDigExtResourceIdPdfPaper"]; } }
        //public static List<string> BonDigExtProdIdsWithCirixCusnos = new List<string>
        //{
        //    BonDigExtProdIdIpad, BonDigExtProdIdIpadKayak, BonDigExtProdIdPaper6Days, BonDigExtProdIdPaperWeekend, BonDigExtProdIdDise,
        //    BonDigExtProdIdAgendaEnergy, BonDigExtProdIdAgendaHealthCare, BonDigExtProdIdAgendaImportExport
        //};

        public static List<string> BonDigExtProdIdsWithCirixCusnos = ConfigurationManager.AppSettings["ServicePlusExternalProductIdsToInclude"].Split(',').ToList();


		public static string SleepType_Break { get { return "01"; } }    //uppehåll
		public static string SleepType_Complaint { get { return "02"; } }    //reklamation

		public static string CreditType_Days { get { return "01"; } }    //dagar
		public static string CreditType_Money { get { return "02"; } }    //pengar
		public static string CreditType_NoCompensation { get { return "03"; } }    //ingen kompensation


		public static bool HideSubsSleepsForAutogiroCust { get { return (ConfigurationManager.AppSettings["HideSubsSleepsForAutogiroCust"].ToLower() == "true"); } }

		public static string TargetGroupRecurringPayment { get { return ConfigurationManager.AppSettings["TargetGroupRecurringPayment"]; } }

		public static List<long> HybridCampGroupIds = new List<long> { 68 };

		public static List<DateTime> DigitalIssues = new List<DateTime>() { new DateTime(2015, 1, 2) };

	}
}
