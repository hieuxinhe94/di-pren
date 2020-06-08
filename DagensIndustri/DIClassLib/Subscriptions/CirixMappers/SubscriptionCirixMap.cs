using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DIClassLib.Subscriptions.CirixMappers
{
    [Serializable]
    public class SubscriptionCirixMap
    {

        #region props

        public long Subsno { get; set; }
        public long SubsCusno { get; set; }
        public long PayCusno { get; set; }
        public long Campno { get; set; }
        public int Extno { get; set; }
        public int SubsLenMons { get; set; }
        public int SubsLenDays { get; set; }

        public string PackageId { get; set; }
        public string ProductNo { get; set; }
        public string PaperCode { get; set; }
        public string SubsKind { get; set; }
        public string SubsState { get; set; }
        public string PriceGroup { get; set; }
        public string SubsType { get; set; }
        public string Autogiro { get; set; }

        public DateTime SubsStartDate { get; set; }
        public DateTime SubsEndDate { get; set; }
        public DateTime UnpBreakDate { get; set; }
        public DateTime InvStartDate { get; set; }
        public DateTime SuspendDate { get; set; }


        public List<SubsSleepsCirixMap> SubsSleeps
        {
            get 
            {
                
                List<SubsSleepsCirixMap> sleeps = new List<SubsSleepsCirixMap>();
                DataSet ds = SubscriptionController.GetSubsSleeps(Subsno);
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        sleeps.Add(new SubsSleepsCirixMap(dr));
                }
                
                return sleeps;
            }
        }


        public List<AddressMap> Addresses
        {
            get 
            {
                List<AddressMap> addresses = new List<AddressMap>();
                    
                //DataSet ds = CirixDbHandler.Ws.GetPendingAddressChanges_(SubsCusno, Subsno);
                DataSet ds = SubscriptionController.GetCurrentAndPendingAddressChanges(SubsCusno, Subsno);
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        addresses.Add(new AddressMap(dr));
                }

                return addresses;
            }
        }

        public List<AddressMap> TempAddresses
        {
            get
            {
                List<AddressMap> ret = new List<AddressMap>();
                foreach (AddressMap am in Addresses)
                {
                    if (am.Addrno != 1)
                        ret.Add(am);
                }

                return ret;
            }
        }

        //public bool IsHybridSubs
        //{
        //    get
        //    {
        //        int appsettHybirdCampGrId = 0;
        //        int.TryParse(MiscFunctions.GetAppsettingsValue("HybridCampGroupId"), out appsettHybirdCampGrId);

        //        if (Campno > 0 && appsettHybirdCampGrId > 0)
        //        {
        //            var ds = CirixDbHandler.GetCampaign(Campno);
        //            if (DbHelpMethods.DataSetHasRows(ds))
        //            {
        //                foreach (DataRow dr in ds.Tables[0].Rows)
        //                {
        //                    var idStr = dr["CAMPGROUPID"].ToString();
        //                    if (!string.IsNullOrEmpty(idStr))
        //                    {
        //                        int idInt = 0;
        //                        int.TryParse(idStr, out idInt);
        //                        if (appsettHybirdCampGrId == idInt)
        //                            return true;
        //                    }
        //                }
        //            }
        //        }

        //        return false;
        //    }
        //}

        #region not used props
        //ROWTEXT1HILTON STHLM SLUSSEN/ROWTEXT1 
        //CANCELREASON xml:space="preserve"/CANCELREASON 
        //SUBSKIND_CODVALFriex/SUBSKIND_CODVAL 
        //SUBSSTATE_CODVALAktiv/SUBSSTATE_CODVAL 
        //CANCELREASON_CODVAL xml:space="preserve"/CANCELREASON_CODVAL 
        //PAPERNAMEDI PÅ NÄTET/PAPERNAME 
        //PRICEGROUP_CODVALOrdinarie Sverige/PRICEGROUP_CODVAL 
        //SUBSTYPE_CODVALIngen rabatt/SUBSTYPE_CODVAL 
        //PRODUCTNAME       //DISE/PRODUCTNAME 
        #endregion

        #endregion


        public SubscriptionCirixMap(DataRow dr)
        {
            try
            {
                Subsno = long.Parse(dr["SUBSNO"].ToString());
                SubsCusno = long.Parse(dr["SUBSCUSNO"].ToString());
                PayCusno = long.Parse(dr["PAYCUSNO"].ToString());
                Campno = long.Parse(dr["CAMPNO"].ToString());
                Extno = int.Parse(dr["EXTNO"].ToString());
                SubsLenMons = int.Parse(dr["SUBSLEN_MONS"].ToString());
                SubsLenDays = int.Parse(dr["SUBSLEN_DAYS"].ToString());

                PackageId = dr["PACKAGEID"].ToString();
                ProductNo = dr["PRODUCTNO"].ToString();
                PaperCode = dr["PAPERCODE"].ToString();
                SubsKind = dr["SUBSKIND"].ToString();
                SubsState = dr["SUBSSTATE"].ToString();
                PriceGroup = dr["PRICEGROUP"].ToString();
                SubsType = dr["SUBSTYPE"].ToString();
                Autogiro = dr["AUTOGIRO"].ToString();

                SubsStartDate = DateTime.Parse(dr["SUBSSTARTDATE"].ToString());
                SubsEndDate = DateTime.Parse(dr["SUBSENDDATE"].ToString());
                UnpBreakDate = DateTime.Parse(dr["UNPBREAKDATE"].ToString());
                InvStartDate = DateTime.Parse(dr["INVSTARTDATE"].ToString());
                SuspendDate = DateTime.Parse(dr["SUSPENDDATE"].ToString());
            }
            catch (Exception ex)
            {
                new Logger("SubscriptionCirixMap() failed for Subsno: " + Subsno.ToString(), ex.ToString());
            }
        }


    }
}
