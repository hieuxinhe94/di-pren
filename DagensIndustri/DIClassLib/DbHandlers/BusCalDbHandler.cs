using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DIClassLib.BusinessCalendar;
using DIClassLib.DbHelpers;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Collections;
using System.Configuration;


namespace DIClassLib.DbHandlers
{

    /// <summary>
    /// Use class BusCalHandler when working with the business calendar.
    /// It holds static info, so number of call to db i reduced.
    /// </summary>
    public static class BusCalDbHandler
    {
        private static string _connStr = "BusinessCalendar";

        private static string _milliStreamUrl  = ConfigurationManager.AppSettings["BusCalMilliStreamUrl"];
        private static string _milliStreamUser = ConfigurationManager.AppSettings["BusCalMilliStreamUser"];
        private static string _milliStreamPass = ConfigurationManager.AppSettings["BusCalMilliStreamPass"];

        private static int _daysBackCompEvents = BusCalSettings.DaysBackMillistreamCompEvents;
        private static int _daysFwdCompEvents = BusCalSettings.DaysFwdMillistreamCompEvents;


        public static List<Group> GetGroups()
        {
            List<Group> grs = new List<Group>();

            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(_connStr, "getGroups", null);

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Group g = new Group(int.Parse(dr["groupId"].ToString()), dr["groupName"].ToString());
                        g.TypesSubTypes = GetGroupsTypesSubTypes(g.GroupId);
                        grs.Add(g);
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetGroups() failed", ex.ToString());
            }

            return grs;
        }

        private static List<TypeSubType> GetGroupsTypesSubTypes(int groupId)
        {
            List<TypeSubType> tSt = new List<TypeSubType>();

            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(_connStr, "getGroupsTypesSubTypes", new SqlParameter("@groupId", groupId));

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        tSt.Add(new TypeSubType(GetNullableInt(dr, "typeId"), GetNullableInt(dr, "subTypeId"), GetNullableString(dr, "typeName")));
                }
            }
            catch (Exception ex)
            {
                new Logger("GetGroupsTypesSubTypes() failed", ex.ToString());
            }

            return tSt;
        }


        public static void InsertSubscription(Subscription sub)
        {
            try
            {
                SqlParameter[] arr = new SqlParameter[] 
                { 
                    new SqlParameter("@subsId", sub.SubsId),
                    new SqlParameter("@cusno", sub.Cusno),
                    new SqlParameter("@disabled", sub.Disabled)
                };

                SqlHelper.ExecuteNonQuery(_connStr, "insertSubs", arr);

                foreach (EventInSubs eis in sub.EventsInSubs)
                    InsertEventInSubs(sub.SubsId, eis);

            }
            catch (Exception ex)
            {
                new Logger("InsertSubscription() failed", ex.ToString());
            }
        }


        public static Subscription GetSubscription(Guid subsId)
        {
            try
            {
                DataSet dsSubs = SqlHelper.ExecuteDataset(_connStr, "getSubs", new SqlParameter("@subsId", subsId));

                if (dsSubs != null && dsSubs.Tables[0] != null && dsSubs.Tables[0].Rows.Count > 0)
                {
                    Subscription sub = new Subscription();
                    sub.SubsId = subsId;
                    sub.Cusno = int.Parse(dsSubs.Tables[0].Rows[0]["cusno"].ToString());
                    sub.Disabled = false;   //not reterived if false
                    sub.DateSaved = DateTime.Parse(dsSubs.Tables[0].Rows[0]["dateSaved"].ToString());

                    sub.EventsInSubs = GetEventsInSubs(subsId);

                    return sub;
                }
            }
            catch (Exception ex)
            {
                new Logger("GetSubscription() failed", ex.ToString());
            }

            DoLog("GetSubscription - could not get subs for subsId:" + subsId.ToString());

            return null;
        }

        private static List<EventInSubs> GetEventsInSubs(Guid subsId)
        {
            List<EventInSubs> evs = new List<EventInSubs>();

            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(_connStr, "getEventsInSubs", new SqlParameter("@subsId", subsId));

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        evs.Add(new EventInSubs(int.Parse(dr["companyId"].ToString()), GetNullableInt(dr, "typeId"), GetNullableInt(dr, "subTypeId"))); 
                }
            }
            catch (Exception ex)
            {
                new Logger("GetEventsInSubs() failed", ex.ToString());
            }

            return evs;
        }


        public static void InsertEventInSubs(Guid subsId, EventInSubs eis)
        {
            try
            {
                SqlParameter[] arr = new SqlParameter[] 
                { 
                    new SqlParameter("@subsId", subsId),
                    new SqlParameter("@companyId", eis.CompanyId),
                    new SqlParameter("@typeId", eis.TypeId),
                    new SqlParameter("@subTypeId", eis.SubTypeId)
                };

                if (eis.TypeId == null)
                    arr[2] = new SqlParameter("@typeId", DBNull.Value);

                if (eis.SubTypeId == null)
                    arr[3] = new SqlParameter("@subTypeId", DBNull.Value);



                SqlHelper.ExecuteNonQuery(_connStr, "insertEventInSubs", arr);
            }
            catch (Exception ex)
            {
                new Logger("InsertEventInSubs() failed", ex.ToString());
            }
        }

        public static void DeleteEventInSubs(Guid subsId, EventInSubs eis)
        {
            try
            {
                SqlParameter[] arr = new SqlParameter[] 
                { 
                    new SqlParameter("@subsId", subsId),
                    new SqlParameter("@companyId", eis.CompanyId),
                    new SqlParameter("@typeId", eis.TypeId),
                    new SqlParameter("@subTypeId", eis.SubTypeId)
                };

                if (eis.TypeId == null)
                    arr[2] = new SqlParameter("@typeId", DBNull.Value);

                if (eis.SubTypeId == null)
                    arr[3] = new SqlParameter("@subTypeId", DBNull.Value);

                SqlHelper.ExecuteNonQuery(_connStr, "deleteEventInSubs", arr);
            }
            catch (Exception ex)
            {
                new Logger("DeleteEventInSubs() failed", ex.ToString());
            }
        }

        
        private static int? GetNullableInt(DataRow dr, string fieldName)
        {
            int? ret = null;

            if (!dr.IsNull(fieldName))
                ret = int.Parse(dr[fieldName].ToString());

            return ret;
        }

        private static string GetNullableString(DataRow dr, string fieldName)
        {
            string ret = null;

            if (!dr.IsNull(fieldName))
                ret = dr[fieldName].ToString();

            return ret;
        }

        public static void DoLog(string descr)
        {
            try
            {
                SqlParameter[] arr = new SqlParameter[] { new SqlParameter("@descr", descr) };

                SqlHelper.ExecuteNonQuery(_connStr, "insertLog", arr);
            }
            catch (Exception ex)
            {
                new Logger("DoLog() - Busniness Calendar - failed", ex.ToString());
            }
        }


        public static List<CompanyEvent> GetCompEvsDiConference()
        {
            List<CompanyEvent> evs = new List<CompanyEvent>();

            try
            {
                DataSet ds = SqlHelper.ExecuteDataset("Conference", "busCalGetUpcomingDiConf", null);

                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int id = int.Parse(dr["epiPageId"].ToString());
                        DateTime dt = DateTime.Parse(dr["dateConferenceStart"].ToString());
                        string ev = dr["headline"].ToString();

                        evs.Add(new CompanyEvent(BusCalSettings.DiCompanyId, BusCalSettings.DiCompanyName, BusCalSettings.DiTypeIdConferece, id, dt, "", ev));
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetCompEvsDiConference() failed", ex.ToString());
            }

            DoLog("GetCompEvsDiConference() - num events:" + evs.Count.ToString());

            return evs;
        }

        public static List<CompanyEvent> GetCompEvsDiGasell()
        {
            List<CompanyEvent> evs = new List<CompanyEvent>();

            try
            {
                DataSet ds = SqlHelper.ExecuteDataset("DISEMiscDB", "busCalGetUpcomingGasellConf", null);

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int id = int.Parse(dr["id"].ToString());
                        DateTime dt = DateTime.Parse(dr["startdate"].ToString());
                        string ev = dr["city"].ToString();

                        evs.Add(new CompanyEvent(BusCalSettings.DiCompanyId, BusCalSettings.DiCompanyName, BusCalSettings.DiTypeIdGasell, id, dt, "", ev));
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetCompEvsDiGasell() - failed", ex.ToString());
            }

            DoLog("GetCompEvsDiGasell() - num events:" + evs.Count.ToString());

            return evs;
        }

        //public static List<CompanyEvent> GetCompEvsDiEvents()
        //{
        //    List<CompanyEvent> evs = new List<CompanyEvent>();

            //try
            //{
            //    DataSet ds = SqlHelper.ExecuteDataset("Campaign", "busCalGetUpcomingDiEvents", null);

            //    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in ds.Tables[0].Rows)
            //        {
            //            int id = int.Parse(dr["eventid"].ToString());
            //            DateTime dt = DateTime.Parse(dr["datestart"].ToString());
            //            string ev = dr["categorytext"].ToString();

            //            evs.Add(new CompanyEvent(BusCalSettings.DiCompanyId, BusCalSettings.DiCompanyName, BusCalSettings.DiTypeIdEvent, id, dt, "", ev));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    new Logger("GetCompEvsDiEvents() - failed", ex.ToString());
            //}

            //DoLog("GetCompEvsDiEvents() - num events:" + evs.Count.ToString());

        //    return evs;
        //}



        /// <summary>
        /// Gets data from Millistream
        /// </summary>
        /// <returns></returns>
        public static List<CompanyEvent> GetCompEvsMillistream()
        {
            List<CompanyEvent> compEvs = new List<CompanyEvent>();

            try
            {
                DateTime dt = DateTime.Now;

                StringBuilder url = new StringBuilder();
                url.Append(_milliStreamUrl);
                url.Append("?usr=" + _milliStreamUser);
                url.Append("&pwd=" + _milliStreamPass);
                url.Append("&cmd=corporateactions");
                url.Append("&marketplace=35201,35181,35197,29929");
                url.Append("&instrumenttype=4");
                url.Append("&fields=insref,name,type,subtype");
                url.Append("&startdate=" + dt.AddDays(-_daysBackCompEvents).ToString("yyyy-MM-dd"));
                url.Append("&enddate=" + dt.AddDays(_daysFwdCompEvents).ToString("yyyy-MM-dd"));

                XmlTextReader reader = new XmlTextReader(url.ToString());
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);

                XmlNode root = doc.SelectSingleNode("milliresult");

                foreach (XmlNode compNode in root.SelectNodes("instrument"))
                {
                    int companyId = 0;
                    string compName = "";

                    if (compNode.SelectSingleNode("insref") != null)
                        companyId = int.Parse(compNode.SelectSingleNode("insref").InnerText);

                    if (compNode.SelectSingleNode("name") != null)
                        compName = compNode.SelectSingleNode("name").InnerText;

                    foreach (XmlNode eventNode in compNode.SelectNodes("corporateaction"))
                    {
                        int? typeId = null;
                        int? subTypeId = null;
                        DateTime dtEv = DateTime.MinValue;

                        if (eventNode.SelectSingleNode("type") != null)
                            typeId = int.Parse(eventNode.SelectSingleNode("type").InnerText);

                        if (eventNode.SelectSingleNode("subtype") != null)
                            subTypeId = int.Parse(eventNode.SelectSingleNode("subtype").InnerText);

                        if (eventNode.Attributes["date"] != null)
                            dtEv = DateTime.Parse(eventNode.Attributes["date"].InnerText);

                        compEvs.Add(new CompanyEvent(companyId, compName, typeId, subTypeId, dtEv, "", ""));
                    }
                }

                if (reader != null)
                    reader.Close();

            }
            catch (Exception ex)
            {
                new Logger("GetCompEvsMillistream() failed", ex.ToString());
            }


            DoLog("GetCompEvsMillistream() - Millistream call - num events:" + compEvs.Count.ToString());

            return compEvs;
        }

        /// <summary>
        /// Gets data from Millistream
        /// </summary>
        /// <returns></returns>
        public static List<Company> GetCompanysFromMS()
        {
            List<Company> comps = new List<Company>();

            try
            {
                StringBuilder url = new StringBuilder();
                url.Append(_milliStreamUrl);
                url.Append("?usr=" + _milliStreamUser);
                url.Append("&pwd=" + _milliStreamPass);
                url.Append("&cmd=quote&marketplace=35201,35181,35197,29929&instrumenttype=4&fields=insref,name,isin");

                XmlTextReader reader = new XmlTextReader(url.ToString());
                //object companyNode = reader.NameTable.Add("instrument");

                int compId = 0;
                string name = "";
                string isin = "";

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals("insref"))
                            compId = int.Parse(reader.ReadString());

                        if (reader.Name.Equals("name"))
                            name = reader.ReadString();

                        if (reader.Name.Equals("isin"))
                        {
                            isin = reader.ReadString();
                            comps.Add(new Company(compId, name, isin));
                        }
                    }
                }

                if (reader != null)
                    reader.Close();

            }
            catch (Exception ex)
            {
                new Logger("GetCompanysFromMS() failed", ex.ToString());
            }

            DoLog("GetCompanysFromMS() - Millistream call - num companys:" + comps.Count.ToString());

            return comps;
        }



        public static ArrayList GetCusnoAndSubsId(string userId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(_connStr, "getCusnoAndSubsId", new SqlParameter("@userId", userId));

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    int? cusno = null;
                    Guid? g = null;

                    if (ds.Tables[0].Rows[0]["cusno"] != DBNull.Value)
                        cusno = int.Parse(ds.Tables[0].Rows[0]["cusno"].ToString());

                    if (ds.Tables[0].Rows[0]["subsId"] != DBNull.Value)
                        g = new Guid(ds.Tables[0].Rows[0]["subsId"].ToString());

                    ArrayList al = new ArrayList();
                    al.Add(cusno);
                    al.Add(g);

                    return al;
                }
            }
            catch (Exception ex)
            {
                new Logger("GetCusnoAndSubsId() failed", ex.ToString());
            }

            DoLog("GetCusnoAndSubsId - no subsId in db for userId:" + userId);

            return null;
        }


        #region old
        //internal static void DeleteSubscription(Guid subsId)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteNonQuery(_connStr, "deleteSubs", new SqlParameter[] { new SqlParameter("@subsId", subsId) });
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("DeleteSubscription() failed", ex.ToString());
        //    }
        //}
        #endregion
    }
}
