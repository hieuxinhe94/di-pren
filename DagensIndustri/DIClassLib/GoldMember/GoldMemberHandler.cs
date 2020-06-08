using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using System.Data.SqlClient;


namespace DIClassLib.GoldMember
{
    public class GoldMemberHandler
    {

        public GoldMemberHandler()
        { }

        
        /// <summary>
        /// return: DataSet.Table[0].Row[0]=customer1, DataSet.Table[1].Row[0]=customer2 
        /// </summary>
        public static DataSet FindCustomer(string birthNo)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                int i = 0;
                foreach (long cusno in GetCusnosByBirthNo(birthNo))
                {
                    DataSet dsCust = SubscriptionController.GetCustomer(cusno);
                    if (dsCust != null && dsCust.Tables[0].Rows != null)
                    {
                        dt = new DataTable();
                        dt = dsCust.Tables[0].Clone();
                        dt.ImportRow(dsCust.Tables[0].Rows[0]);
                        dt.TableName = "a" + i.ToString();
                    }

                    ds.Tables.Add(dt);
                    i++;
                }

                return ds;
            }
            catch (Exception ex)
            {
                new Logger("FindCustomer(birthNo) - failed", ex.ToString());
                return null;
            }
        }
        
        
        public static List<long> GetCusnosByBirthNo(string birthNo)
        {
            List<long> cusnos = new List<long>();

            if (birthNo.Length < 12)
                return cusnos;

            try
            {
                DataSet ds = SqlHelper.ExecuteDataset("DisePren", "GetCusnosByBirthNo", new SqlParameter("@birthNo", birthNo));

                foreach (DataRow dr in ds.Tables[0].Rows)
                    cusnos.Add(long.Parse(dr["cusno"].ToString()));

            }
            catch (Exception ex)
            {
                new Logger("GetCusnosByBirthNo() - failed", ex.ToString());
            }

            return cusnos;
        }
        
        
        public static DataSet FindCustomer(string company, string lastName, string firstName, string email, string phone, string street, string streetNo, string zip, string city)
        {
            try
            {
                string sName1 = string.IsNullOrEmpty(company) ? lastName + " " + firstName : company;
                string sName2 = string.IsNullOrEmpty(company) ? string.Empty : lastName + " " + firstName;
                string sEmail = email;
                string sPhone = phone;
                string sStreet = street;
                string sHouseNo = streetNo;
                string sZipCode = zip;
                string sPostName = city;

                if (string.IsNullOrEmpty((sName1 + sName2 + sEmail + sPhone + sStreet + sHouseNo + sZipCode + sPostName).Trim(' ')))
                    return null;
                
                //LblError.Text = "sName1: '" + sName1.Trim(' ').ToUpper() + "'<br />sName2: '" + sName2.Trim(' ').ToUpper() + "'";
                DataSet ds = SubscriptionController.FindCustomers(0, 0, 0,
                                                           sName1.Trim(' ').ToUpper(),  //sname1
                                                           sName2.Trim(' ').ToUpper(),  //sname2
                                                           "",                          //sname3
                                                           sPhone.Trim(' '),            //phone
                                                           sEmail.Trim(' '),            //email
                                                           sStreet.Trim(' ').ToUpper(), //sStreet
                                                           sHouseNo.Trim(' ').ToUpper(),          //sHouseNo
                                                           "",                          //sStairCase
                                                           "",                          //sApartment
                                                           "",                          //sCountry
                                                           sZipCode.Trim(' '),          //sZipCode
                                                           "",                          //sUserId
                                                           sPostName.Trim(' ').ToUpper() //sPostName
                                                           );
                return ds;
            }
            catch (Exception ex)
            {
                new Logger("FindCustomer(many args) - failed", ex.ToString());
                return null;
            }
        } 

    }
}
