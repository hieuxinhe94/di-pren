using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DIClassLib.DbHelpers;


namespace DIClassLib.BusinessCalendar
{
    public class CompanysInJsHandler
    {

        public CompanysInJsHandler(List<Company> companys)
        {
            try
            {
                string pathBase = DIClassLib.Misc.MiscFunctions.GetAppsettingsValue("PathToPublicJsDir");
                //why tmp file? org file might be in use / on error org file is hopefully ok
                string pathTmp = pathBase + "DiGuldCalendarDataTmp.js"; 
                string pathOrg = pathBase + "DiGuldCalendarData.js";

                FileStream fs = new FileStream(pathTmp, FileMode.Truncate, FileAccess.ReadWrite);

                StreamWriter sr = new StreamWriter(fs);
                foreach (string s in GetJsFileRows(companys))
                    sr.WriteLine(s);

                sr.Close();
                fs.Close();

                FileInfo fi = new FileInfo(pathTmp);
                fi.CopyTo(pathOrg, true);               //replace org file
            }
            catch (Exception ex)
            {
                new Logger("CompanysInJsHandler() - failed", ex.ToString());
            }
        }


        private List<string> GetJsFileRows(List<Company> companys)
        {
            List<string> s = new List<string>();
            s.Add("{");
            s.Add("\"companies\": [");

            string end;
            for (int i = 0; i < companys.Count; i++)
            {
                end = (i < companys.Count - 1) ? "," : "";
                s.Add("{ \"id\": " + companys[i].CompanyId.ToString() + ", \"name\": \"" + companys[i].Name + "\" }" + end);
            }
            
            s.Add("],");
            s.Add("\"categories\" : [");
            s.Add("{ \"id\": " + BusCalSettings.GrIdUtdelningar.ToString()   + ", \"name\": \"" + BusCalHandler.GetGroupName(BusCalSettings.GrIdUtdelningar)   + "\", \"checked\": true },");
            s.Add("{ \"id\": " + BusCalSettings.GrIdStammor.ToString()       + ", \"name\": \"" + BusCalHandler.GetGroupName(BusCalSettings.GrIdStammor)       + "\", \"checked\": true },");
            s.Add("{ \"id\": " + BusCalSettings.GrIdKapmarkndagar.ToString() + ", \"name\": \"" + BusCalHandler.GetGroupName(BusCalSettings.GrIdKapmarkndagar) + "\", \"checked\": true },");
            s.Add("{ \"id\": " + BusCalSettings.GrIdRapporter.ToString()     + ", \"name\": \"" + BusCalHandler.GetGroupName(BusCalSettings.GrIdRapporter)     + "\", \"checked\": true },");
            s.Add("{ \"id\": " + BusCalSettings.GrIdEmissioner.ToString()    + ", \"name\": \"" + BusCalHandler.GetGroupName(BusCalSettings.GrIdEmissioner)    + "\", \"checked\": false },");
            s.Add("{ \"id\": " + BusCalSettings.GrIdSplit.ToString()         + ", \"name\": \"" + BusCalHandler.GetGroupName(BusCalSettings.GrIdSplit)         + "\", \"checked\": false }");
            s.Add("]");
            s.Add("}");
            
            return s;
        }

    }
}
