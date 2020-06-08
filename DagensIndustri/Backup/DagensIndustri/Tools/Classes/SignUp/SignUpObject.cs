using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DIClassLib.DbHandlers;

namespace DagensIndustri.Tools.Classes.SignUp
{
    [Serializable]
    public class SignUpObject
    {
        #region Properties

        public int EpiPageId { get; set; }

        public string City { get; set; }

        public string Date { get; set; }

        #endregion

        public SignUpObject(int epiPageID, string city, string date)
        {
            EpiPageId = epiPageID;
            City = city;
            Date = date;

            if (string.IsNullOrEmpty(City))
            {
                City = string.Empty;
            }

            DateTime dt;

            if (!string.IsNullOrEmpty(Date))
                dt = Convert.ToDateTime(Date);
            else
                dt = DateTime.Now;

            //todo make it more dynamic
            //MsSqlHandler.InsertAndUpdateGasellObject(EpiPageId, City, dt);
        }
    }
}