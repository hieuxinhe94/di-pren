using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;

namespace DIClassLib.Wine
{
    public class SystembolagetImport
    {
        private string _importUri = "";

        //private static SqlConnection GetConnection()
        //{
        //    //SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=Vintest;Integrated Security=True");

        //    SqlConnection conn = DIClassLib.DbHandlers.MsSqlHandler.Get
        //    return conn;
        //}

        public SystembolagetImport(string uri)
        {
            _importUri = uri;
        }

        public int UpdateDatabase()
        {
            return UpdateDatabase(-1);
        }

        public int UpdateDatabase(int limit)
        {
            int importedCount = 0;

            System.Xml.Linq.XDocument xdoc = XDocument.Load(_importUri);
            XElement xeArticles = xdoc.Element("artiklar");
            IEnumerable<XElement> articles = null;
            if (limit > 0)
            {
                articles = xeArticles.Elements("artikel").Take(limit);
            }
            else
            {
                articles = xeArticles.Elements("artikel");
            }
            

            foreach (XElement xeArticle in articles)
            {
                if (xeArticle.Element("Varugrupp") != null
                    && xeArticle.Element("Varugrupp").Value != null
                    && xeArticle.Element("Varugrupp").Value.ToLower().Contains("vin"))
                {

                    SystembolagetArticle article = ParseArticle(xeArticle);
                    InsertOrUpdateDatabaseArticle(article);
                    ++importedCount;
                }

            }

            return importedCount;
        }

        private void InsertOrUpdateDatabaseArticle(SystembolagetArticle article)
        {
            
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@nr", article.Nr),
                new SqlParameter("@artikelid", article.Artikelid),
                new SqlParameter("@varnummer", article.Varnummer),
                new SqlParameter("@namn", article.Namn),
                new SqlParameter("@namn2", article.Namn2),
                new SqlParameter("@prisinklmoms", article.Prisinklmoms),
                new SqlParameter("@volymiml", article.Volymiml),
                new SqlParameter("@varugrupp", article.Varugrupp),
                new SqlParameter("@ursprung", article.Ursprung),
                new SqlParameter("@ursprunglandnamn", article.Ursprunglandnamn),
                new SqlParameter("@producent", article.Producent),
                new SqlParameter("@argang", article.Argang),
                new SqlParameter("@alkoholhalt", article.Alkoholhalt),
                new SqlParameter("@sortiment", article.Sortiment),
                new SqlParameter("@ekologisk", article.Ekologisk),
                new SqlParameter("@ravarorbeskrivning", article.RavarorBeskrivning)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "Wine_InsertOrUpdateSystembolagetArticle", parameters);
   
        }

        private SystembolagetArticle ParseArticle(XElement xeArticle)
        {

            string varugrupp = xeArticle.Element("Varugrupp").Value;
            int nr = 0;
            Int32.TryParse(xeArticle.Element("nr").Value, out nr);

            int artikelid = 0;
            Int32.TryParse(xeArticle.Element("Artikelid").Value, out artikelid);

            int varnummer = 0;
            Int32.TryParse(xeArticle.Element("Varnummer").Value, out varnummer);

            string namn = xeArticle.Element("Namn").Value;
            string namn2 = xeArticle.Element("Namn2").Value;
            string prisinklmoms = xeArticle.Element("Prisinklmoms").Value;
            string volymiml = xeArticle.Element("Volymiml").Value;
            string ursprung = xeArticle.Element("Ursprung").Value;
            string ursprungLandnamn = xeArticle.Element("Ursprunglandnamn").Value;
            string producent = xeArticle.Element("Producent").Value;
            string argang = xeArticle.Element("Argang").Value;
            string alkoholhalt = xeArticle.Element("Alkoholhalt").Value;
            string sortiment = xeArticle.Element("Sortiment").Value;
            string sEkologisk = xeArticle.Element("Ekologisk").Value;

            string ravarorBeskrivning = null;
            if (xeArticle.Element("RavarorBeskrivning") != null)
                ravarorBeskrivning = xeArticle.Element("RavarorBeskrivning").Value;

            SystembolagetArticle article = new SystembolagetArticle()
            {
                Varugrupp = varugrupp,
                Nr = nr,
                Artikelid = artikelid,
                Varnummer = varnummer,
                Namn = namn,
                Namn2 = namn2,
                Prisinklmoms = prisinklmoms,
                Volymiml = volymiml,
                Ursprung = ursprung,
                Ursprunglandnamn = ursprungLandnamn,
                Producent = producent,
                Argang = argang,
                Alkoholhalt = alkoholhalt,
                Sortiment = sortiment,
                Ekologisk = (sEkologisk != null && sEkologisk == "1"),
                RavarorBeskrivning = ravarorBeskrivning
            };
            return article;
        }
    }

    public class SystembolagetArticle
    {
        public SystembolagetArticle()
        {
        }

        public SystembolagetArticle(DataRow dr)
        {
            Id = (int)dr["id"];
            Varugrupp = (string)dr["varugrupp"];
            Nr = (int)dr["nr"];
            Artikelid = (int)dr["Artikelid"];

            Varnummer = (int)dr["Varnummer"];
            Namn = (string)dr["Namn"];

            if (dr["Namn2"] != System.DBNull.Value)
                Namn2 = (string)dr["Namn2"];

            if (dr["Prisinklmoms"] != System.DBNull.Value)
                Prisinklmoms = (string)dr["Prisinklmoms"];

            if (dr["Volymiml"] != System.DBNull.Value)
                Volymiml = (string)dr["Volymiml"];

            if (dr["Ursprung"] != System.DBNull.Value)
                Ursprung = (string)dr["Ursprung"];

            if (dr["Ursprunglandnamn"] != System.DBNull.Value)
                Ursprunglandnamn = (string)dr["Ursprunglandnamn"];

            if (dr["Producent"] != System.DBNull.Value)
                Producent = (string)dr["Producent"];

            if (dr["Argang"] != System.DBNull.Value)
                Argang = (string)dr["Argang"];

            if (dr["Alkoholhalt"] != System.DBNull.Value)
                Alkoholhalt = (string)dr["Alkoholhalt"];

            if (dr["Sortiment"] != System.DBNull.Value)
                Sortiment = (string)dr["Sortiment"];

            if (dr["Ekologisk"] != System.DBNull.Value)
                Ekologisk = (bool)dr["Ekologisk"];

            if (dr["RavarorBeskrivning"] != System.DBNull.Value)
                RavarorBeskrivning = (string)dr["RavarorBeskrivning"];
        }
        public int Id { get; set; }
        public string Varugrupp { get; set; }
        public int Nr { get; set; }
        public int Artikelid { get; set; }
        public int Varnummer { get; set; }
        public string Namn { get; set; }
        public string Namn2 { get; set; }
        public string Prisinklmoms { get; set; }
        public string Volymiml { get; set; }
        public string Ursprung { get; set; }
        public string Ursprunglandnamn { get; set; }
        public string Producent { get; set; }
        public string Argang { get; set; }
        public string Alkoholhalt { get; set; }
        public string Sortiment { get; set; }
        public bool Ekologisk { get; set; }
        public string RavarorBeskrivning { get; set; }

    }
}