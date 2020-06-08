using ApsisMailSort.ClassLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;


namespace ApsisMailSort.Handlers
{
    public class DbHandler
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ApsisMailSender"].ToString();

        //Creates a connection to DB (SP getPaperCode). Selects all PaperCodes and creates a list of theese to return.
        public List<StringPair> GetPaperCodeList()
        {
            var PaperCodeList = new List<StringPair>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("getPaperCode", conn))
                {
                    var myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    PaperCodeList.Add(new StringPair("PaperCode", "0"));

                    while (myReader.Read())
                    {
                        var pc = myReader["PaperCode"].ToString();
                        PaperCodeList.Add(new StringPair(pc, pc));
                    }
                }
                conn.Close();
            }
            return PaperCodeList;
        }

        //Creates a connection to DB (SP getProductNo). Selects all ProductNo's and creates a list of theese to return.
        public List<StringPair> GetProductNoList()
        {
            var ProductNoList = new List<StringPair>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("getProductNo", conn))
                {
                    var myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    ProductNoList.Add(new StringPair("ProductNo", "0"));

                    while (myReader.Read())
                    {
                        var pn = myReader["ProductNo"].ToString();

                        ProductNoList.Add(new StringPair(pn, pn));
                    }
                    conn.Close();
                }
                return ProductNoList;
            }
        }

        //Sets a connection to DB (SP getFilteredCostumersReport). Creates a GridViewListData(see class) to return.
        //The search filter from the user specifies which parameters to send to DB.
        public List<GridViewListData> GetGridViewData(string sDateFromDatePicker, string sDateToDatePicker, string sSelectedMail, string sPaperCode, ArrayList SubsLen_Mons, string sProductNo)
        {
            var GridViewData = new List<GridViewListData>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("getFilteredCustomersReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(sDateFromDatePicker) || !string.IsNullOrEmpty(sDateToDatePicker))
                    {
                        SqlParameter pickedDateFromDatePicker = new SqlParameter("@date1", sDateFromDatePicker);
                        SqlParameter pickedDateToDatePicker = new SqlParameter("@date2", sDateToDatePicker);

                        cmd.Parameters.Add(pickedDateFromDatePicker);
                        cmd.Parameters.Add(pickedDateToDatePicker);
                    }
                    if (sSelectedMail != "0")
                    {
                        SqlParameter ddlMail = new SqlParameter("@selectedMail", sSelectedMail);
                        cmd.Parameters.Add(ddlMail);
                    }
                    if (sPaperCode != "0")
                    {
                        SqlParameter PaperCodes = new SqlParameter("@papercode", sPaperCode);
                        cmd.Parameters.Add(PaperCodes);
                    }
                    if (SubsLen_Mons.Count != 0)
                    {
                        //Note to self
                        //This code is for SQL2005 or any other SQL Server where creating a datatable in SQL is not supported.
                        //Sends in a string to SP which is converted to int's using the function called iter$simple_intlist_to_tbl and then check against column SubsLen_Mons.
                        var sbSubsLen_Mons = new StringBuilder();
                        foreach (var Month in SubsLen_Mons)
                            sbSubsLen_Mons.Append(Month + ",");

                        string sSubsLen_Mons = sbSubsLen_Mons.ToString().Trim(',');

                        SqlParameter SubsLen_Monss = new SqlParameter("@subslength", sSubsLen_Mons);
                        cmd.Parameters.Add(SubsLen_Monss);

                        #region
                        //Note to self
                        //This code is for SQL2008 and later versions where creating tables in SQL is supported.
                        //Creats a DataTable for which is sent to the SP.
                        //var dtSelectedMons = new DataTable();

                        //dtSelectedMons.Columns.Add("integerValue", typeof(int));

                        //foreach (var sm in SubsLen_Mons)
                        //{
                        //    dtSelectedMons.Rows.Add(sm);
                        //}

                        //SqlParameter SubsLen_Monss = new SqlParameter("@subslength", dtSelectedMons) { SqlDbType = SqlDbType.Structured };
                        //cmd.Parameters.Add(SubsLen_Monss);
                        #endregion

                    }
                    if (sProductNo != "0")
                    {
                        SqlParameter ProductNos = new SqlParameter("@productNo", sProductNo);
                        cmd.Parameters.Add(ProductNos);
                    }

                    SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (myReader.Read())
                    {
                        var GridViewListData = new GridViewListData();

                        GridViewListData.customerId = myReader["customerId"].ToString();
                        GridViewListData.name = myReader["name"].ToString();
                        GridViewListData.email = myReader["email"].ToString();
                        GridViewListData.userName = myReader["userName"].ToString();

                        if (!string.IsNullOrEmpty(myReader["dateSaved"].ToString()))
                            GridViewListData.dateSaved = myReader["dateSaved"].ToString().Remove(10);

                        else
                            GridViewListData.dateUpdated = myReader["dateSaved"].ToString();

                        if (!string.IsNullOrEmpty(myReader["dateUpdated"].ToString()))
                            GridViewListData.dateUpdated = myReader["dateUpdated"].ToString().Remove(10);

                        else
                            GridViewListData.dateUpdated = myReader["dateUpdated"].ToString();

                        if (!string.IsNullOrEmpty(myReader["dateRegularLetter"].ToString()))
                            GridViewListData.dateRegularLetter = myReader["dateRegularLetter"].ToString().Remove(10);

                        else
                            GridViewListData.dateRegularLetter = myReader["dateRegularLetter"].ToString();

                        GridViewListData.ApsisUpdateCheckServicePlus = myReader["ApsisUpdateCheckServicePlus"].ToString();
                        GridViewListData.HaveServicePlusAccount = myReader["HaveServicePlusAccount"].ToString();
                        GridViewListData.PaperCode = myReader["PaperCode"].ToString();
                        GridViewListData.ProductNo = myReader["ProductNo"].ToString();
                        GridViewListData.SubsLen_Mons = myReader["SubsLen_Mons"].ToString();

                        if (myReader["PaperCode"].ToString().ToUpper() == "AGENDA")
                        {
                            if (myReader["ProductNo"].ToString() == "01")
                                GridViewListData.ProductName = "ENERGI";

                            if (myReader["ProductNo"].ToString() == "02")
                                GridViewListData.ProductName = "VÅRD OCH OMSORG";

                            if (myReader["ProductNo"].ToString() == "03")
                                GridViewListData.ProductName = "UTRIKESHANDEL";
                        }

                        if (myReader["PaperCode"].ToString().ToUpper() == "DI")
                        {
                            if (myReader["ProductNo"].ToString() == "01")
                                GridViewListData.ProductName = "DI";

                            if (myReader["ProductNo"].ToString() == "02")
                                GridViewListData.ProductName = "TALTIDNING";

                            if (myReader["ProductNo"].ToString() == "05")
                                GridViewListData.ProductName = "DI HELG";
                        }

                        if (myReader["PaperCode"].ToString().ToUpper() == "DISE" && myReader["ProductNo"].ToString() == "01")
                            GridViewListData.ProductName = "DISE";

                        if (myReader["PaperCode"].ToString().ToUpper() == "IPAD" && myReader["ProductNo"].ToString() == "01")
                            GridViewListData.ProductName = "IPAD";


                        GridViewData.Add(GridViewListData);
                    }
                }
            }
            return GridViewData;
        }
    }
}

