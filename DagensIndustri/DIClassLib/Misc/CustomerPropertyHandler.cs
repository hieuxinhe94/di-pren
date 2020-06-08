using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using System.Data;
using System.Configuration;
using DIClassLib.DbHelpers;


namespace DIClassLib.Misc
{
	/// <summary>
	/// Class working against the cusproperty table in cirix. 
	/// </summary>
	public class CustomerPropertyHandler
	{

		private long _cusno;
		private List<CustomerProp> _currentCusProps;

		public List<CustomerProp> AllCustomerProperties { get { return _currentCusProps; } }

		public CustomerPropertyHandler(long cusno)
		{
			_cusno = cusno;
			_currentCusProps = GetCustomersCusProps(cusno);
		}


		/// <summary>
		/// Input values: "" ==> REMOVES property, null ==> KEEPS property. 
		/// isPersonalEx=true only inserts cusProp if OkToAddPersonalEx()==true
		/// </summary>
		public CustomerPropertyHandler(long cusno, string email, string phoneMobile, string birthNo, bool? isGoldMember, bool? isPersonalEx)
		{
			_cusno = cusno;
			_currentCusProps = GetCustomersCusProps(cusno);
			List<CustomerProp> cusPropsNew = GetNewCusProps(email, phoneMobile, birthNo, isGoldMember, isPersonalEx);
			InsertCusProps(_currentCusProps, cusPropsNew);
		}


		public bool UserPassesGoldRules()
		{
			CustomerPropPair current_11_12_CusPropPair = new CustomerPropPair(GetCurrentCusProp("11"), GetCurrentCusProp("12"));

			foreach (CustomerPropPair cpp in CusPropPairsOkForJoiningGold)
			{
				if (cpp.Compare(current_11_12_CusPropPair))
					return true;
			}

			return false;
		}



		#region private methods

		private CustomerProp GetCurrentCusProp(string propCode)
		{
			foreach (CustomerProp cp in _currentCusProps)
			{
				if (cp.PropCode == propCode)
					return new CustomerProp(cp.PropCode, cp.PropValue);
			}

			return new CustomerProp(null, null);
		}

		private List<CustomerPropPair> CusPropPairsOkForJoiningGold
		{
			get
			{
				List<CustomerPropPair> ret = new List<CustomerPropPair>();                                  //11=EXEMPLAR, 12=KUNDTYP
				ret.Add(new CustomerPropPair(new CustomerProp(null, null), new CustomerProp(null, null)));  //11=blank, 12=blank
				ret.Add(new CustomerPropPair(new CustomerProp(null, null), new CustomerProp("12", "1")));   //11=blank, 12=1
				ret.Add(new CustomerPropPair(new CustomerProp(null, null), new CustomerProp("12", "2")));   //...
				ret.Add(new CustomerPropPair(new CustomerProp(null, null), new CustomerProp("12", "3")));
				ret.Add(new CustomerPropPair(new CustomerProp(null, null), new CustomerProp("12", "4")));
				ret.Add(new CustomerPropPair(new CustomerProp("11", "4"), new CustomerProp(null, null)));   //11,4 = privatex
				ret.Add(new CustomerPropPair(new CustomerProp("11", "4"), new CustomerProp("12", "1")));    //företag   
				ret.Add(new CustomerPropPair(new CustomerProp("11", "4"), new CustomerProp("12", "2")));    //privatperson
				ret.Add(new CustomerPropPair(new CustomerProp("11", "4"), new CustomerProp("12", "3")));    //student
				ret.Add(new CustomerPropPair(new CustomerProp("11", "4"), new CustomerProp("12", "4")));    //pensionär
				ret.Add(new CustomerPropPair(new CustomerProp("11", "4"), new CustomerProp("12", "5")));    //bibliotek
				ret.Add(new CustomerPropPair(new CustomerProp("11", "4"), new CustomerProp("12", "6")));    //skola
				ret.Add(new CustomerPropPair(new CustomerProp("11", "4"), new CustomerProp("12", "7")));    //mottagning/väntsal
				ret.Add(new CustomerPropPair(new CustomerProp("11", "4"), new CustomerProp("12", "8")));    //hotell
				ret.Add(new CustomerPropPair(new CustomerProp("11", "4"), new CustomerProp("12", "9")));    //avtalskund
				return ret;
			}
		}

		public static List<CustomerProp> GetCustomersCusProps(long cusNo)
		{
			var cusProps = new List<CustomerProp>();
			var ds = SubscriptionController.GetCustomerProperties(cusNo);
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				var bIsCirixSystem = ds.Tables[0].Columns.Contains("PROPCODE") && ds.Tables[0].Columns.Contains("PROPVALUE");
				var bIsKayakSystem = ds.Tables[0].Columns.Contains("PROPERTYCODE") && ds.Tables[0].Columns.Contains("PROPERTYVALUECODE");

				if (bIsCirixSystem)
				{
					cusProps.Add(new CustomerProp(dr["PROPCODE"].ToString(), dr["PROPVALUE"].ToString()));
				}
				else if (bIsKayakSystem)
				{
                    var propValueCode = (string)dr["PROPERTYVALUECODE"] ?? string.Empty;
                    var propValue = propValueCode == "-1" ? (string)dr["USERVALUE"] : propValueCode;

                    cusProps.Add(new CustomerProp(dr["PROPERTYCODE"].ToString(), propValue));
				}
			}
			return cusProps;
		}

		private void DeleteCustomersCusProps()
		{
			SubscriptionController.CleanCustomerProperties(_cusno);
		}

		private List<CustomerProp> GetNewCusProps(string email, string phoneMobile, string birthNo, bool? isGoldMember, bool? isPersonalEx)
		{
			List<CustomerProp> cusProps = new List<CustomerProp>();

			if (email != null)
				cusProps.Add(new CustomerProp("22", email));

			if (phoneMobile != null)
				cusProps.Add(new CustomerProp("23", phoneMobile));

			if (birthNo != null)
			{
				string bn = FormatCustPropBirthNo(birthNo);
				if (!string.IsNullOrEmpty(bn))
					cusProps.Add(new CustomerProp("20", bn));
			}

			if (isGoldMember != null)
			{
				if (isGoldMember == true)
					cusProps.Add(new CustomerProp("97", "01"));
				else
					cusProps.Add(new CustomerProp("97", ""));
			}

			if (isPersonalEx != null)
			{
				//no other 11,X combination allowed if 11,4 is to be saved
				if (OkToHandleCusPropPrivateEx)
				{
					if (isPersonalEx == true)
						cusProps.Add(new CustomerProp("11", "4"));
					else
						cusProps.Add(new CustomerProp("11", ""));
				}
			}

			return cusProps;
		}

		private bool OkToHandleCusPropPrivateEx
		{
			get
			{
				//no propcode=11 in list
				if (!PropCodeInList("11", _currentCusProps))
					return true;

				foreach (CustomerProp cp in _currentCusProps)
				{
					if (cp.PropCode == "11" && cp.PropValue != "4")
						return false;
				}

				return true;
			}
		}


		/// <summary>
		/// Format to AppSettings["CustPropBirthNoFormat"]
		/// </summary>
		/// <param name="birthNo">yyyyMMdd / yyyyMMddXXXX / dd.MM.yyyy</param>
		/// <returns></returns>
		private string FormatCustPropBirthNo(string birthNo)
		{
			string bn = birthNo;

			if (!string.IsNullOrEmpty(bn) && bn.Length >= 8)
			{
				try
				{
					DateTime dt = new DateTime();

					if (!bn.Contains('.'))  //SWE (yyyyMMddXXXX)
					{
						dt = new DateTime(int.Parse(bn.Substring(0, 4)),
										  int.Parse(bn.Substring(4, 2)),
										  int.Parse(bn.Substring(6, 2)));
					}
					else                    //FIN (dd.MM.yyyy)
					{
						dt = new DateTime(int.Parse(bn.Substring(6, 4)),
										  int.Parse(bn.Substring(3, 2)),
										  int.Parse(bn.Substring(0, 2)));
					}

					return dt.ToString(ConfigurationManager.AppSettings["CustPropBirthNoFormat"]);
				}
				catch (Exception ex)
				{
					new Logger("FormatCustPropBirthNo failed for in-param:" + birthNo, ex.ToString());
				}
			}

			return string.Empty;
		}

		public void InsertCusProps(List<CustomerProp> cusPropsAll, List<CustomerProp> cusPropsNew)
		{
			//delete all cusProps
			DeleteCustomersCusProps();

			//insert new cusProps
			foreach (CustomerProp cpNew in cusPropsNew)
			{
				if (!PropCodeInList(cpNew.PropCode, cusPropsAll) && cpNew.PropValue.Length > 0)
					SubscriptionController.InsertCustomerProperty(_cusno, cpNew.PropCode, cpNew.PropValue);
			}

			//insert current cusProps, do update
			foreach (CustomerProp cp in cusPropsAll)
			{
				foreach (CustomerProp cpNew in cusPropsNew)
				{
					if (cp.PropCode == cpNew.PropCode)
					{
						cp.PropValue = cpNew.PropValue;
						break;
					}
				}

				if (cp.PropValue.Length > 0)
					SubscriptionController.InsertCustomerProperty(_cusno, cp.PropCode, cp.PropValue);
			}
		}

		private bool PropCodeInList(string propCode, List<CustomerProp> cusProps)
		{
			foreach (CustomerProp cp in cusProps)
			{
				if (cp.PropCode == propCode)
					return true;
			}

			return false;
		}

		#endregion

	}


	public class CustomerPropPair
	{
		public CustomerProp CusProp11;
		public CustomerProp CusProp12;

		public CustomerPropPair(CustomerProp cp11, CustomerProp cp12)
		{
			CusProp11 = cp11;
			CusProp12 = cp12;
		}

		internal bool Compare(CustomerPropPair cpp)
		{
			return (CusProp11.Compare(cpp.CusProp11) && CusProp12.Compare(cpp.CusProp12));
		}
	}

	//data carrier class
	public class CustomerProp
	{
		public string PropCode;
		public string PropValue;

		public CustomerProp(string propCode, string propValue)
		{
			PropCode = propCode;
			PropValue = propValue;
		}

		internal bool Compare(CustomerProp cp)
		{
			return (PropCode == cp.PropCode && PropValue == cp.PropValue);
		}
	}



}
