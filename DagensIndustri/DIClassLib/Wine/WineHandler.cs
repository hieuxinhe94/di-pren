using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DIClassLib.DbHandlers;
using System.Runtime.Serialization;

namespace DIClassLib.Wine
{
    [DataContract]
    public class Wine
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "article_no")]
        public int Varnummer { get; set; }

        [DataMember(Name = "description")]
        public String About { get; set; }

        [DataMember(Name = "date")]
        public DateTime Date { get; set; }

        [DataMember(Name = "year")]
        public String Year { get; set; }

        [DataMember(Name = "week")]
        public int Week 
        { 
            get
            {
                IsoWeek isoWeek = new IsoWeek(this.Date);
                return isoWeek.Week;
            }
        }

        public String Type
        {
            get
            {
                if (Group != null && Group.ToLower().Contains("rött vin"))
                    return "Rött vin";
                else if (Group != null && Group.ToLower().Contains("vitt vin"))
                    return "Vitt vin";
                else if (Group != null && Group.ToLower().Contains("mousserande vin"))
                    return "Mousserande vin";
                else if (Group != null && Group.ToLower().Contains("rosévin"))
                    return "Rosévin";
                else
                    return "";
            }
        }

        public bool ThisWeek
        {
            get
            {
                IsoWeek thisWeek = new IsoWeek(DateTime.Now);
                IsoWeek wineWeek = new IsoWeek(this.Date);
                return (thisWeek.Year == wineWeek.Year && thisWeek.Week == wineWeek.Week) ;
            }
        }

        [DataMember(Name = "characterids")]
        public List<int> CharacterIds { get; set; }

        //origin1, origin2,grape,percentage
        [DataMember(Name = "characternames")]
        public List<String> CharacterNames { get; set; }

        public String Name1 { get; set; }
        public String Name2 { get; set; }

        public String Origin1 { get; set; }
        public String Origin2 { get; set; }

        public String Grape { get; set; }
        public String Percentage { get; set; }

        public String Group { get; set; }

        public String Longitude { get; set; }
        public String Latitude { get; set; }
        

        public Wine()
        {
            CharacterIds = new List<int>();
            CharacterNames = new List<string>();
        }

    }

    public class WineCharacter
    {
        public int Id { get; set; }
        public String Name { get; set; }
    }


    public class WineHandler
    {
        public static int InsertWine(int varnummer, String about, DateTime date, int[] characterIds,String longitude, String latitude)
        {
            int id = 0;

            id = MsSqlHandler.InsertWine(varnummer, about, date,longitude,latitude);

            foreach (int characterId in characterIds)
            {
                MsSqlHandler.InsertCharacterInWine(id, characterId);
            }

            return id;
        }

        public static int InsertWineCharacter(String name)
        {
            int id = 0;
            DataTable dt = MsSqlHandler.GetWineCharacters();
            if (dt != null)
            {
                DataRow[] rows = dt.Select("name = '" + name + "'");
                if (rows != null && rows.Count() > 0)
                {
                    throw new Exception("Karaktären '" + name + "' finns redan");
                }
            }
            id = MsSqlHandler.InsertWineCharacter(name);



            return id;
        }

        public static DataTable GetWineCharacters()
        {
            DataTable dt = MsSqlHandler.GetWineCharacters();
            return dt;
        }

        public static DataTable GetAllWines()
        {
            DataTable dt = MsSqlHandler.GetAllWines();
            return dt;

        }

        public static SystembolagetArticle GetSystembolagetArticleByVarnummer(int varnummer)
        {
            SystembolagetArticle article = null;
            DataTable dt = MsSqlHandler.GetSystembolagetArticleByVarnummer(varnummer);
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                article = new SystembolagetArticle(dt.Rows[0]);

            }
            return article;
        }

        public static Wine GetWine(int id)
        {
            Wine wine = null;

            DataTable dt = MsSqlHandler.GetWine(id);
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                wine = new Wine();
                wine.Id = (int)dt.Rows[0]["id"];
                wine.About = (string)dt.Rows[0]["about"];
                wine.Date = (DateTime)dt.Rows[0]["date"];
                wine.Varnummer = (int)dt.Rows[0]["varnummer"];
                if (dt.Rows[0]["longitude"] != System.DBNull.Value)
                    wine.Longitude = (string)dt.Rows[0]["longitude"];
                if (dt.Rows[0]["latitude"] != System.DBNull.Value)
                    wine.Latitude = (string)dt.Rows[0]["latitude"];

                wine.CharacterIds = new List<int>();
                
                foreach (DataRow dr in dt.Rows)
                {
                    int characterId = (int)dr["characterId"];
                    if (!wine.CharacterIds.Contains(characterId))
                    {
                        wine.CharacterIds.Add(characterId);
                    }
                }

            }
            return wine;

        }

        public static void UpdateWine(int wineId, int varnummer, string about, DateTime date, int[] updateCharacterIds,string longitude,string latitude)
        {
            Wine existingWine = GetWine(wineId);
            List<int> currentCharacterIds = existingWine.CharacterIds;


            List<int> idsToDelete = new List<int>();
            List<int> idsToInsert = new List<int>();
            foreach (int i in updateCharacterIds)
            {
                if (!currentCharacterIds.Contains(i))
                {
                    //idsToInsert.Add(i);
                    MsSqlHandler.InsertCharacterInWine(wineId, i);
                }
            }

            foreach (int i in currentCharacterIds)
            {
                if (!updateCharacterIds.Contains(i))
                {
                    MsSqlHandler.DeleteCharacterInWine(wineId, i);
                    //idsToDelete(i);
                }
            }


            MsSqlHandler.UpdateWine(wineId, varnummer, about, date,longitude,latitude);


        }

        public static void DeleteWine(int id)
        {
            MsSqlHandler.DeleteWine(id);
        
        }


        public static List<Wine> GetWinesWithCharacters()
        {
            List<Wine> wines = new List<Wine>();
            DataTable dt = MsSqlHandler.GetWinesWithCharacters();
            int currentId = -1;
            Wine wine = null;
            if (dt != null && dt.Rows != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int rowId = (int)dr["id"];
                    if (rowId != currentId)
                    {


                        wine = new Wine();
                        wine.About = (string)dr["about"];
                        wine.Id = (int)dr["id"];
                        wine.Date = (DateTime)dr["date"];
                        wine.Varnummer = (int)dr["varnummer"];
                        if(dr["namn"] != System.DBNull.Value)
                            wine.Name1 = (string)dr["namn"];
                        if (dr["namn2"] != System.DBNull.Value)
                            wine.Name2 = (string)dr["namn2"];
                        if (dr["ursprung"] != System.DBNull.Value)
                            wine.Origin1 = (string)dr["ursprung"];
                        if (dr["ursprunglandnamn"] != System.DBNull.Value)
                            wine.Origin2 = (string)dr["ursprunglandnamn"];
                        if (dr["alkoholhalt"] != System.DBNull.Value)
                            wine.Percentage = (string)dr["alkoholhalt"];
                        if (dr["ravarorbeskrivning"] != System.DBNull.Value)
                            wine.Grape = (string)dr["ravarorbeskrivning"];
                        if (dr["varugrupp"] != System.DBNull.Value)
                            wine.Group = (string)dr["varugrupp"];
                        if (dr["argang"] != System.DBNull.Value)
                            wine.Year = (string)dr["argang"];
                        if (dr["longitude"] != System.DBNull.Value)
                            wine.Longitude = (string)dr["longitude"];
                        if (dr["latitude"] != System.DBNull.Value)
                            wine.Latitude = (string)dr["latitude"];

                        currentId = wine.Id;
                        wines.Add(wine);
                    }

                    int characterId = (int)dr["CharacterId"];
                    if (!wine.CharacterIds.Contains(characterId))
                    {
                        wine.CharacterIds.Add((int)dr["CharacterId"]);
                        wine.CharacterNames.Add((string)dr["CharacterName"]);
                    }

                }
            }
            return wines;
        }

        public static void DeleteWineCharacter(int id)
        {
            List<Wine> wines = GetWinesWithCharacters();
            if(wines.Where(x => x.CharacterIds.Contains(id)).Count() > 0)
            {
                throw new Exception("Det går inte att ta bort karaktären eftersom det finns ett eller flera viner som har denna.");
                
            }
            MsSqlHandler.DeleteWineCharacter(id);
        }

        public static void UpdateWineCharacter(int id, string name)
        {
            DataTable dt = GetWineCharacters();
            if (dt != null)
            {
                DataRow[] rows = dt.Select("name = '" + name + "'");
                if (rows != null && rows.Count() > 0)
                {
                    foreach (DataRow dr in rows)
                    {
                        int rowId = (int)dr["id"];
                        if (rowId != id)
                            throw new Exception("Karaktären '" + name + "' finns redan");
                    }
                }
            }
            MsSqlHandler.UpdateWineCharacter(id,name);
        }

        public static WineCharacter GetWineCharacter(int id)
        {
            DataTable dtCharacters = GetWineCharacters();
            DataRow[] rows = dtCharacters.Select("id = " + id);
            if (rows != null && rows.Count() > 0)
            {
                WineCharacter wc = new WineCharacter()
                {
                    Id = (int)rows[0]["id"],
                    Name = (string)rows[0]["name"]
                    
                };
                return wc;
            }
            return null;
        }
    }
}
