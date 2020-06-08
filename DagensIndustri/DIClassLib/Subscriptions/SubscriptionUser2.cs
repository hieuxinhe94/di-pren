using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using DIClassLib.Membership;
using DIClassLib.Subscriptions.CirixMappers;


namespace DIClassLib.Subscriptions
{

	[Serializable]
	public class SubscriptionUser2
	{
		#region properties

		public long Cusno { get; set; }

		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }

		public string RowText1 { get; set; }    //CompName              LastName FirstName
		public string RowText2 { get; set; }    //LastName FirstName    ''
		public bool IsCompanyCust
		{
			get
			{
				if (!string.IsNullOrEmpty(RowText2))
					return true;

				return false;
			}
		}

		public string StreetName { get; set; }
		public string HouseNo { get; set; }
		public string Staricase { get; set; }
		public string Address { get { return (StreetName + " " + HouseNo + " " + Staricase).Trim(); } }

		public string Apartment { get; set; }
		public string Street2 { get; set; }
		public string CountryCode { get; set; }
		public string Zip { get; set; }
		public string PostName { get; set; }

		public string HPhone { get; set; }
		public string WPhone { get; set; }
		public string OPhone { get; set; }
		public string SalesDen { get; set; }
		public string OfferdenDir { get; set; }
		public string OfferdenSal { get; set; }
		public string OfferdenEmail { get; set; }
		public string DenySmsMark { get; set; }
		public string AccnoBank { get; set; }
		public string AccnoAcc { get; set; }
		public string Notes { get; set; }
		public long Ecusno { get; set; }
		public string OtherCusno { get; set; }
		//public string WwwUserId { get; set; }
		public string Expday { get; set; }
		public double DiscPercent { get; set; }
		public string Terms { get; set; }
		//public string BirthNo { get; set; }
		public string SocialSecNo { get; set; }
		public string Category { get; set; }
		public long MasterCusno { get; set; }
		public string CompanyId { get; set; }


		public List<SubscriptionCirixMap> SubsActive
		{
			get
			{
				if (Cusno <= 0)
				{
					return new List<SubscriptionCirixMap>();
				}
				var subs = new List<SubscriptionCirixMap>();
				var ds = SubscriptionController.GetSubscriptions(Cusno, false);
				if (!DbHelpMethods.DataSetHasRows(ds))
				{
					return subs;
				}
				subs.AddRange(from DataRow dr in ds.Tables[0].Rows select new SubscriptionCirixMap(dr));
				return subs;
			}
		}

		public List<SubscriptionCirixMap> SubsActiveOrPassive
		{
			get
			{
				if (Cusno > 0)
				{
					var subs = new List<SubscriptionCirixMap>();
					var ds = SubscriptionController.GetSubscriptions(Cusno, true);
					if (DbHelpMethods.DataSetHasRows(ds))
					{
						foreach (DataRow dr in ds.Tables[0].Rows)
						{
							subs.Add(new SubscriptionCirixMap(dr));
						}
					}
					return subs;
				}
				return new List<SubscriptionCirixMap>();
			}
		}

		public List<AddressMap> SelectableTempAddresses
		{
			get
			{
				List<AddressMap> adrs = new List<AddressMap>();
				if (Cusno > 0)
				{
					DataSet ds = SubscriptionController.GetCusTempAddresses(Cusno, string.Empty, "TRUE");
					if (DbHelpMethods.DataSetHasRows(ds))
					{
						foreach (DataRow dr in ds.Tables[0].Rows)
							adrs.Add(new AddressMap(dr));
					}
				}

				return adrs;
			}
		}

		public List<AddressMap> PermAddresses
		{
			get
			{
				List<AddressMap> _permAddresses = new List<AddressMap>();

			    var subsActive = SubsActive;

                if (Cusno > 0 && subsActive.Count > 0)
				{
					_permAddresses = new List<AddressMap>();
                    foreach (AddressMap a in subsActive[0].Addresses)
					{
						if (a.Addrno == 1)
							_permAddresses.Add(a);
					}
				}

				return _permAddresses;
			}
		}


		//private List<TemporaryAddressCirixMap> _currentAndPendingAddressChanges = null;
		//public List<TemporaryAddressCirixMap> CurrentAndPendingAddressChanges
		//{
		//    get 
		//    {
		//        if (_currentAndPendingAddressChanges == null && Cusno > 0)
		//        {
		//            _currentAndPendingAddressChanges = new List<TemporaryAddressCirixMap>();
		//            DataSet ds = CirixDbHandler.Ws.GetCusTempAddresses_(Cusno, "", "TRUE");
		//            if(DbHelpMethods.DataSetHasRows(ds))
		//            {
		//                foreach(DataRow dr in ds.Tables[0].Rows)
		//                    _selectableTempAddresses.Add(new TemporaryAddressCirixMap(dr));
		//            }
		//        }

		//        return (_selectableTempAddresses != null) ? _selectableTempAddresses : new List<TemporaryAddressCirixMap>(); 
		//    }

		//    set 
		//    { 
		//        _selectableTempAddresses = value; 
		//    }
		//}


		//private List<SubscriptionCirixMap> _subsActivePaperCodeDi = null;
		//public List<SubscriptionCirixMap> SubsActivePaperCodeDi
		//{
		//    get
		//    {
		//        if (_subsActivePaperCodeDi == null && Cusno > 0)
		//        {
		//            _subsActivePaperCodeDi = new List<SubscriptionCirixMap>();
		//            foreach (SubscriptionCirixMap sub in SubsActive)
		//            {
		//                if (sub.PaperCode == Settings.PaperCode_DI)
		//                    _subsActivePaperCodeDi.Add(sub);
		//            }
		//        }

		//        return _subsActivePaperCodeDi;
		//    }

		//    set
		//    {
		//        _subsActivePaperCodeDi = value;
		//    }
		//}

		//public long SubscriptionNumber { get; set; }
		//public long PayCustomerNumber { get; set; }
		//public DateTime SubStart { get; set; }
		//public DateTime SubEnd { get; set; }
		//public string SubscriptionPaperCode { get; set; }


		//public DataSet HolidayStops
		//{
		//    get
		//    {
		//        return CirixDbHandler.Ws.GetSubsSleeps_(SubscriptionNumber);
		//    }
		//}

		//public DataSet PendingAddresses
		//{
		//    get
		//    {
		//        return CirixDbHandler.Ws.GetPendingAddressChanges_(Cusno, SubscriptionNumber);
		//    }
		//}

		//public DataSet TemporaryAddresses
		//{
		//    get
		//    {
		//        return CirixDbHandler.Ws.GetCusTempAddresses_(Cusno, "", "TRUE");
		//    }
		//}

		#endregion


		#region Constructors
		public SubscriptionUser2()
		{
			Init(System.Web.HttpContext.Current.User.Identity.Name);
		}

		public SubscriptionUser2(string userName)
		{
			Init(userName);
		}

		public SubscriptionUser2(long customerNumber)
		{
			Cusno = customerNumber;
			Init(null);
		}
		#endregion


		#region Methods
		private void Init(string userName)
		{
			if (!string.IsNullOrEmpty(userName))
				SetPropsFromDisePren(userName);

			//Continue init subscriber if CustomerNumber is found in DISEPren
			if (Cusno > 0)
			{
				SetPropsByGetCustomer(Cusno);
			}
		}

		private void SetPropsFromDisePren(string userName)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset("DisePren", "getUserInfo", new SqlParameter("@Login", userName));
				if (ds != null && ds.Tables != null && ds.Tables[0] != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows[0]["cusno"] != DBNull.Value)
				{
					DataRow dr = ds.Tables[0].Rows[0];
					Cusno = long.Parse(dr["cusno"].ToString());
					UserName = dr["userid"].ToString();
					Password = dr["password"].ToString();
					Email = dr["email"].ToString();
					//BirthNo = dr["birthNo"].ToString();
					SocialSecNo = dr["birthNo"].ToString();
				}
			}
			catch (Exception ex)
			{
				new Logger("SetPropsFromDisePren() failed for username: " + userName, ex.ToString());
			}
		}

		private void SetPropsByGetCustomer(long CustomerNumber)
		{
			try
			{
				var ds = SubscriptionController.GetCustomer(CustomerNumber);

				if (ds == null || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count <= 0)
				{
					return;
				}
				var dr = ds.Tables[0].Rows[0]; //A:               B:
				RowText1 = dr["ROWTEXT1"].ToString(); //FÖRETAGET        ENAMN FNAMN
				RowText2 = dr["ROWTEXT2"].ToString(); //ENAMN FNAMN      ''

				//Address = dr["STREETNAME"] + " " + dr["HOUSENO"] + " " + dr["STAIRCASE"];
				//FirstName = dr["FIRSTNAME"] as string;      //FNAMN
				//LastName = dr["LASTNAME"] as string;        //ENAMN

				StreetName = dr["STREETNAME"].ToString(); //GATAN
				HouseNo = dr["HOUSENO"].ToString(); //1
				Staricase = dr["STAIRCASE"].ToString(); //A

				Apartment = dr["APARTMENT"].ToString(); //2TR
				Street2 = dr["STREET2"].ToString(); //CAREOF 2001
				CountryCode = dr["COUNTRYCODE"].ToString();
				Zip = dr["ZIPCODE"].ToString();
				PostName = dr["POSTNAME"].ToString(); //BROMMA
				Email = dr["EMAILADDRESS"].ToString();
				HPhone = dr["H_PHONE"].ToString();
				WPhone = dr["W_PHONE"].ToString();
				OPhone = dr["O_PHONE"].ToString();
				SalesDen = dr["SALESDEN"].ToString();
				OfferdenDir = dr["OFFERDEN_DIR"].ToString();
				OfferdenSal = dr["OFFERDEN_SAL"].ToString();
				OfferdenEmail = dr["OFFERDEN_EMAIL"].ToString();
				DenySmsMark = dr["DENYSMSMARK"].ToString();
				AccnoBank = dr["ACCNO_BANK"].ToString();
				AccnoAcc = dr["ACCNO_ACC"].ToString();
				Notes = dr["NOTES"].ToString();

				long tmpEcusno;
				if (long.TryParse(dr["ECUSNO"].ToString(), out tmpEcusno))
					Ecusno = tmpEcusno;

				OtherCusno = dr["OTHER_CUSNO"].ToString();
				//WwwUserId = customerDr["WWWUSERID"] as string;
				Expday = dr["EXPDAY"].ToString();

				double tmpDiscPercent;
				if (double.TryParse(dr["DISCPERCENT"].ToString(), out tmpDiscPercent))
					DiscPercent = tmpDiscPercent;

				Terms = dr["TERMS"].ToString();

				var ss = dr["SOCIALSECNO"].ToString();
				if (string.IsNullOrEmpty(SocialSecNo) && !string.IsNullOrEmpty(ss))
					SocialSecNo = ss;

				Category = dr["CATEGORY"].ToString();

				long tmpMasterCusno;
				if (long.TryParse(dr["MASTERCUSNO"].ToString(), out tmpMasterCusno))
					MasterCusno = tmpMasterCusno;

				if (dr.Table.Columns.Contains("COMPANYID"))
					CompanyId = dr["COMPANYID"].ToString();

				UserName = SubscriptionController.GetWwwUserId(CustomerNumber);
				Password = SubscriptionController.GetWwwPassword(CustomerNumber);
			}
			catch (Exception ex)
			{
				new Logger("SetPropsByGetCustomer() failed for cusno: " + CustomerNumber, ex.ToString());
			}
		}


		public string CreateHolidayStop(long subsno, DateTime fromDate, DateTime toDate, string sleepType, string allowWebPaper, string creditType)
		{
			new Logger("SubscriptionUSer2.CreateHolidayStop() is using CreateSubssleep_CII()");
			//return SubscriptionController.CreateHolidayStop(Cusno, null, subsno, fromDate, toDate, sleepType, allowWebPaper, creditType);
			var lResult = SubscriptionController.CreateSubssleep_CII(subsno, fromDate, toDate, sleepType, creditType, allowWebPaper, string.Empty, Settings.sReceiveType, string.Empty);
			return lResult == 0 ? "OK" : "FAILED";
		}

		public string DeleteHolidayStop(long subsno, DateTime fromDate, DateTime toDate)
		{
			return SubscriptionController.DeleteHolidayStop(Cusno, null, subsno, fromDate, toDate);
		}

		public string CreateTemporaryAddress(long subsno, AddressMap am, DateTime fromDate, DateTime toDate)
		{
			var topExtno = GetTopExtno(subsno);
			var ret = SubscriptionController.TemporaryAddressChange("DIWEB", Cusno, subsno, topExtno, am, fromDate, toDate, string.Empty, string.Empty, string.Empty);

			//on fail: retry with topExtno-1. (TopExtno might belong to a later sub generation).
			if (ret != "OK" && topExtno > 0)
				ret = SubscriptionController.TemporaryAddressChange("DIWEB", Cusno, subsno, topExtno - 1, am, fromDate, toDate, string.Empty, string.Empty, string.Empty);

			return ret;
		}

		//150417 - obsolete
		//public string CreateTemporaryNewAddress(long subsno, string street, string houseNo, string stairCase, string apartment, string co, string zip, DateTime fromDate, DateTime toDate)
		//{
		//	return SubscriptionController.TemporaryAddressChangeNewAddress(Cusno, subsno, street, houseNo, stairCase, apartment, co, "", "SE", zip, "", "", fromDate, toDate, "");
		//}

		public string DeleteTemporaryAddress(long subsno, int addrno, DateTime fromDate)
		{
			int topExtno = GetTopExtno(subsno);
			var ret = SubscriptionController.TemporaryAddressChangeRemove("DIWEB", Cusno, subsno, topExtno, addrno, fromDate);

			//on fail: retry with topExtno-1. (TopExtno might belong to a later sub generation).
			if (ret != "OK" && topExtno > 0)
				ret = SubscriptionController.TemporaryAddressChangeRemove("DIWEB", Cusno, subsno, topExtno - 1, addrno, fromDate);

			return ret;
		}

		public int GetTopExtno(long subsno)
		{
			int extno = 0;

			foreach (var sub in SubsActiveOrPassive)
			{
				if (sub.Subsno == subsno && sub.Extno > extno)
					extno = sub.Extno;
			}

			return extno;
		}

		/*public string TemporaryAddressChangePeriod(long cusno, long subsno, int addrno, DateTime orgStartDate, DateTime newStartDate, DateTime newEndDate)
		{
			return CirixDbHandler.Ws.TemporaryAddressChangePeriod_(null, cusno, subsno, addrno, orgStartDate, newStartDate, newEndDate);
		}

		public string EndAvailableTemporaryAddress(long lCusno, int iAddrno, System.DateTime dOriginalStartDate)
		{
			return CirixDbHandler.Ws.EndAvailableTemporaryAddress_(lCusno, iAddrno, dOriginalStartDate);
		}*/

		public string CreatePermanentAddress(string street, string houseNo, string staircase, string apartment, string street2, string zip, DateTime startDate)
		{
			//return CirixDbHandler.Ws.DefinitiveAddressChange_(Cusno, street, houseNo, staircase, apartment, street2, "", "SE", zip, fromDate);
			//return CirixDbHandler.DefinitiveAddressChange(Cusno, street, houseNo, staircase, apartment, street2, zip, startDate);
			string ret = SubscriptionController.DefinitiveAddressChange(Cusno, street, houseNo, staircase, apartment, street2, zip, startDate);

			if (ret == "OK")
				OneByOne.Obo.OboUnsubscribe(Cusno);

			return ret;
		}

		public string DefinitiveAddressChangeRemove(DateTime startDate)
		{
			return SubscriptionController.DefinitiveAddressChangeRemove(Cusno, startDate);
		}

		public DataSet GetOpenInvoices()
		{
			var payCusno = (from x in SubsActive where x.PayCusno > 0 select x.PayCusno).FirstOrDefault();

			return SubscriptionController.GetOpenInvoices(payCusno > 0 ? payCusno : Cusno);
		}

		public bool UpdateEmail(string email)
		{
			try
			{
				if (!string.IsNullOrEmpty(email))
					email = email.ToLower();

				CompanyId = "1234567890";

				new CustomerPropertyHandler(Cusno, email, null, null, null, null);
				SubscriptionController.UpdateCustomerInformation(Cusno, email, HPhone, WPhone, OPhone, SalesDen, OfferdenDir, OfferdenSal, OfferdenEmail, DenySmsMark, AccnoBank, AccnoAcc, Notes, Ecusno, OtherCusno, UserName, Expday, DiscPercent, Terms, SocialSecNo, Category, MasterCusno, CompanyId);
				MsSqlHandler.UpdateCustomer(Cusno, UserName, Password, email, SocialSecNo);   //update mssql login table

				Email = email;
				return true;
			}
			catch (Exception ex)
			{
				new Logger("UpdateEmail() failed", ex.ToString());
			}

			return false;
		}

		public bool UpdatePhoneMobile(string phoneMob)
		{
			try
			{
				if (!string.IsNullOrEmpty(phoneMob))
					phoneMob = MiscFunctions.FormatPhoneNumber(phoneMob, Settings.PhoneMaxNoOfDigits, true);

				SubscriptionController.UpdateCustomerInformation(Cusno, Email, HPhone, WPhone, phoneMob, SalesDen, OfferdenDir, OfferdenSal, OfferdenEmail, DenySmsMark, AccnoBank, AccnoAcc, Notes, Ecusno, OtherCusno, UserName, Expday, DiscPercent, Terms, SocialSecNo, Category, MasterCusno, CompanyId);
				new CustomerPropertyHandler(Cusno, null, phoneMob, null, null, null);

				OPhone = phoneMob;
				return true;
			}
			catch (Exception ex)
			{
				new Logger("UpdatePhoneMobile() failed", ex.ToString());
			}
			return false;
		}


		/// <summary>
		/// Returns: 1=success, -1=username in use, -2=error
		/// </summary>
		public int UpdateUserName(string username)
		{
			try
			{
				username = username.ToUpper();

				int tmp = IsUserNameInUse(username);
				if (tmp != 0)
					return tmp;

				SubscriptionController.UpdateCustomerInformation(Cusno, Email, HPhone, WPhone, HPhone, SalesDen, OfferdenDir, OfferdenSal, OfferdenEmail, DenySmsMark, AccnoBank, AccnoAcc, Notes, Ecusno, OtherCusno, username, Expday, DiscPercent, Terms, SocialSecNo, Category, MasterCusno, CompanyId);
				MsSqlHandler.UpdateCustomer(Cusno, username, Password, Email, SocialSecNo);   //update mssql login table

				UserName = username;
				return 1;
			}
			catch (Exception ex)
			{
				new Logger("UpdateUserName() failed", ex.ToString());
				return -2;
			}
		}

		/// <summary>
		/// Checks if username is occupied.
		/// Returns: 0=ok to use, -1=username in use, -2=error
		/// </summary>
		private int IsUserNameInUse(string userName)
		{
			try
			{
				long tmp = SubscriptionController.IdentifyCustomer(0, 0, 0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, userName.ToUpper());

				if (tmp > 0)    //one hit, cusno returned
					return -1;

				return int.Parse(tmp.ToString());
			}
			catch (Exception ex)
			{
				new Logger("IsUserNameInUse() - failed for username: " + userName.ToUpper(), ex.ToString());
				return -2;
			}
		}

		//public long IsUserNameInUse(string userName)
		//{
		//    return CirixDbHandler.Ws.IdentifyCustomer_(0, 0, 0, "", "", "", "", "", "", "", "", "", "", "", "", userName.ToUpper());
		//}

		public bool UpdatePasswd(string newPassword)
		{
			try
			{
				SubscriptionController.UpdateWwwPassword(Cusno, newPassword, newPassword, true, Password);
				MsSqlHandler.UpdateCustomer(Cusno, UserName, newPassword, Email, SocialSecNo);   //update mssql login table

				Password = newPassword;
				return true;
			}
			catch (Exception ex)
			{
				new Logger("UpdatePasswd() failed", ex.ToString());
			}

			return false;
		}


		public bool UpdateUser(string newUserName, string newPassword, string newEmail)
		{
			try
			{
				string userNameOrg = UserName;

				if (!string.IsNullOrEmpty(newUserName))
					UserName = newUserName;

				if (!string.IsNullOrEmpty(newEmail))
				{
					Email = newEmail;
					new CustomerPropertyHandler(Cusno, Email, null, null, null, null);
				}

				//Update username, email
				if (!string.IsNullOrEmpty(newUserName) || !string.IsNullOrEmpty(newEmail))
					SubscriptionController.UpdateCustomerInformation(Cusno, Email, HPhone, WPhone, OPhone, SalesDen, OfferdenDir, OfferdenSal, OfferdenEmail, DenySmsMark, AccnoBank, AccnoAcc, Notes, Ecusno, OtherCusno, UserName, Expday, DiscPercent, Terms, SocialSecNo, Category, MasterCusno, CompanyId);

				//Update password
				if (!string.IsNullOrEmpty(newPassword))
					SubscriptionController.UpdateWwwPassword(Cusno, newPassword, newPassword, true, Password);

				//update mssql login table
				MsSqlHandler.UpdateCustomer(userNameOrg, newUserName, newPassword, newEmail);

				return true;
			}
			catch (Exception ex)
			{
				new Logger("UpdateUser failed", ex.ToString());
			}

			return false;
		}

		public bool UpdateUserOnJoinGold(string newEmail, string newMobilePhone, string newSocSecNo)
		{
			try
			{
				string socSecToSave = SocialSecNo;
				if (!string.IsNullOrEmpty(newSocSecNo))
					socSecToSave = newSocSecNo;

				//cirix cust
				SubscriptionController.UpdateCustomerInformation(Cusno, newEmail, HPhone, WPhone, newMobilePhone, SalesDen, OfferdenDir, OfferdenSal, OfferdenEmail, DenySmsMark, AccnoBank, AccnoAcc, Notes, Ecusno, OtherCusno, UserName, Expday, DiscPercent, Terms, socSecToSave, Category, MasterCusno, CompanyId);

				//cirix cusprops - use newSocSecNo (can be null, that matters)
				new CustomerPropertyHandler(Cusno, newEmail, newMobilePhone, newSocSecNo, true, true);

				//mssql login table
				MsSqlHandler.UpdateUserJoinGuld(Cusno, newEmail, socSecToSave);

			    if (!string.IsNullOrEmpty(UserName))
			    {
                    //DiRoleHandler.AddUserToRoles(new string[] { DiRoleHandler.RoleDiGold });
                    DiRoleHandler.AddUserToRoles(UserName, new string[] { DiRoleHandler.RoleDiGold });
                    LoginUtil.ReLoginUserRefreshCookie(UserName, Password);
			    }

				return true;
			}
			catch (Exception ex)
			{
				string e = (newEmail == null) ? "null" : newEmail;
				string m = (newMobilePhone == null) ? "null" : newMobilePhone;
				string s = (newSocSecNo == null) ? "null" : newSocSecNo;
				string ss = (SocialSecNo == null) ? "null" : SocialSecNo;
				string prms = "newEmail:" + e + ", newMobilePhone:" + m + ", newSocSecNo:" + s + ", SocialSecNo:" + ss;
				new Logger("UpdateUserJoinGuld() failed for cusno: " + Cusno.ToString() + " - params - " + prms, ex.ToString());
			}

			return false;
		}


		//private void TrySetActiveDiSubProps(long CustomerNumber)
		//{
		//    try
		//    {
		//        DataSet subDs = CirixDbHandler.Ws.GetSubscriptions_(CustomerNumber, "TRUE", null);

		//        List<string> activeSubsStates = new List<string> { "01", "02" };    //01=active 02=break
		//        foreach (DataRow dr in subDs.Tables[0].Rows)                        //Get first active subscription with papercode=DI
		//        {
		//            string subsState = dr["SUBSSTATE"].ToString();
		//            if (Settings.SubsStateActiveValues.Contains(subsState))
		//            {
		//                SubStart = DateTime.Parse(dr["INVSTARTDATE"].ToString());
		//                SubEnd = DateTime.Parse(dr["SUBSENDDATE"].ToString());
		//                SubscriptionNumber = long.Parse(dr["SUBSNO"].ToString());
		//                PayCustomerNumber = long.Parse(dr["PAYCUSNO"].ToString());
		//                SubscriptionPaperCode = dr["PAPERCODE"].ToString();
		//            }
		//            if (activeSubsStates.Contains(subsState) && SubscriptionPaperCode == Settings.PaperCode_DI)
		//                break;
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        new Logger("TrySetActiveDiSubProps() failed for cusno: " + CustomerNumber.ToString(), ex.ToString());
		//    }
		//}

		#endregion



		//public DateTime GetClosestIssueDateByUsersRole(System.Security.Principal.IPrincipal iPrincipal, DateTime dt)
		//{
		//    if (iPrincipal.IsInRole(DiRoleHandler.RoleDiY))
		//    {
		//        DateTime tmp = CirixDbHandler.GetNextIssueDateIncDiRules(dt, Settings.PaperCode_DIY, Settings.ProductNo_Regular);
		//        if (tmp >= DateTime.Now.Date)
		//            return tmp;
		//    }

		//    if (iPrincipal.IsInRole(DiRoleHandler.RoleDiWeekend))
		//        return CirixDbHandler.GetNextIssueDateIncDiRules(dt, Settings.PaperCode_DI, Settings.ProductNo_Weekend);
		//    else
		//        return CirixDbHandler.GetNextIssueDateIncDiRules(dt, Settings.PaperCode_DI, Settings.ProductNo_Regular);
		//}
	}
}
