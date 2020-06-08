using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DIClassLib.Antipodes
{
    public static class AntipodesWinesHandler
    {
        //todo - move to settings
        static string _baseUrl   = "http://api.antipodeswines.com/1.0/";
        static string _pId       = "DIV";
        static string _secretKey = "653e34f8557d4a1ec2f1ea75f2730923";

        private static string ChallangeToken
        {
            get
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_baseUrl + "auth?pid=" + _pId);
                return doc.SelectSingleNode("antipodeswines/auth/token").InnerText;
            }
        }
        private static string AuthToken
        {
            get { return MiscFunctions.MD5EncodeStr(_secretKey + ChallangeToken); }
        }


        public static WineItem GetWineItem(int id)
        {
            WineItem wi = new WineItem();
            
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_baseUrl + "export-wine?md=all&id=" + id.ToString() + "&pid=" + _pId + "&token=" + AuthToken);
                return PopulateWineItem(wi, doc);
            }
            catch (Exception ex)
            {
                new Logger("GetWineItem() - failed", ex.ToString());
            }

            return wi;

            #region xml structure
            //<?xml version="1.0" encoding="UTF-8"?>
            //    <antipodeswines>
            //        <response-status>success</response-status>
            //        <wines total="1">
            //            <wine id="767">
            //                <modified>2011-03-15 10:34:29</modified>
            //                <status>active</status>
            //                <name>Dog Point Chardonnay 2008</name>
            //                <description>Robert Parker: 91 poäng. Äntligen inne igen, den nya årgången av Dog Point Chardonnay, ett vin som kommit att bli ett av våra medlemmars favoritviner. Perfekt mogna chardonnaydruvor har vinifierats varsamt och därefte lagrats i 18 månader på franska ekfat, något som ger integrerade ektoner och en unik smakpalett. Ett vin som fungerar lika bra till fisk och skaldjur, gärna med något kraftigare tillbehör, eller lättare rätter med kyckling eller kalv. 91 poäng av Robert Parker.</description>
            //                <type>White</type>
            //                <region>Marlborough</region>
            //                <country>New Zealand</country>
            //                <bottle-size>750</bottle-size>
            //                <unit>ml</unit>
            //                <alcohol>13.50</alcohol>
            //                <image>http://api.antipodeswines.com/images/wine_uploads/image_1299576345_767.png</image>
            //            </wine>
            //        </wines>
            //    </antipodeswines>
            #endregion
        }
        
        private static WineItem PopulateWineItem(WineItem wi, XmlDocument doc)
        {
            string root = "antipodeswines";

            if (doc.SelectSingleNode(root + "/response-status") != null)
                if (doc.SelectSingleNode(root + "/response-status").InnerText != "success")
                    return wi;

            string item = root + "/wines/wine";

            if (doc.SelectSingleNode(item) == null)
                return wi;

            XmlNode n = doc.SelectSingleNode(item);
                wi.Id = int.Parse(n.Attributes["id"].Value);


            if (doc.SelectSingleNode(item + "/modified") != null)
                wi.DateModified = DateTime.Parse(doc.SelectSingleNode(item + "/modified").InnerText);

            if (doc.SelectSingleNode(item + "/status") != null)
                wi.IsActive = (doc.SelectSingleNode(item + "/status").InnerText == "active") ? true : false;

            if (doc.SelectSingleNode(item + "/name") != null)
                wi.Name = doc.SelectSingleNode(item + "/name").InnerText;

            if (doc.SelectSingleNode(item + "/description") != null)
                wi.Descr = doc.SelectSingleNode(item + "/description").InnerText;

            if (doc.SelectSingleNode(item + "/type") != null)
                wi.Type = doc.SelectSingleNode(item + "/type").InnerText;

            if (doc.SelectSingleNode(item + "/region") != null)
                wi.Region = doc.SelectSingleNode(item + "/region").InnerText;

            if (doc.SelectSingleNode(item + "/country") != null)
                wi.Country = doc.SelectSingleNode(item + "/country").InnerText;

            if (doc.SelectSingleNode(item + "/bottle-size") != null)
                wi.BottleSize = int.Parse(doc.SelectSingleNode(item + "/bottle-size").InnerText);

            if (doc.SelectSingleNode(item + "/unit") != null)
                wi.Unit = doc.SelectSingleNode(item + "/unit").InnerText;

            if (doc.SelectSingleNode(item + "/alcohol") != null)
                wi.Alcohol = doc.SelectSingleNode(item + "/alcohol").InnerText.Replace('.', ',');

            if (doc.SelectSingleNode(item + "/image") != null)
                wi.ImagePath = doc.SelectSingleNode(item + "/image").InnerText;

            return wi;
        }

        public static List<WineItem> GetAllWines()
        {
            List<WineItem> wineList = new List<WineItem>();
            string root = "antipodeswines";
            string item = root + "/wines/wine";
            XmlDocument doc = GetAllExportWines();

            XmlNodeList nodes = doc.SelectNodes(item);

            foreach (XmlNode node in nodes)
            {
                WineItem wi = new WineItem();

                if (node.SelectSingleNode("modified") != null)
                    wi.DateModified = DateTime.Parse(node.SelectSingleNode("modified").InnerText);

                if (node.SelectSingleNode("status") != null)
                    wi.IsActive = (node.SelectSingleNode("status").InnerText == "active") ? true : false;

                if (node.SelectSingleNode("name") != null)
                    wi.Name = node.SelectSingleNode("name").InnerText;

                if (node.SelectSingleNode("description") != null)
                    wi.Descr = node.SelectSingleNode("description").InnerText;

                if (node.SelectSingleNode("type") != null)
                    wi.Type = node.SelectSingleNode("type").InnerText;

                if (node.SelectSingleNode("region") != null)
                    wi.Region = node.SelectSingleNode("region").InnerText;

                if (node.SelectSingleNode("country") != null)
                    wi.Country = node.SelectSingleNode("country").InnerText;

                if (node.SelectSingleNode("bottle-size") != null)
                    wi.BottleSize = int.Parse(node.SelectSingleNode("bottle-size").InnerText);

                if (node.SelectSingleNode("unit") != null)
                    wi.Unit = node.SelectSingleNode("unit").InnerText;

                if (node.SelectSingleNode("alcohol") != null)
                    wi.Alcohol = node.SelectSingleNode("alcohol").InnerText.Replace('.', ',');

                if (node.SelectSingleNode("image") != null)
                    wi.ImagePath = node.SelectSingleNode("image").InnerText;

                wineList.Add(wi);
            }

            return wineList;
        }

        public static List<string> GetAllWinesID()
        {
            List<string> wineIDs = new List<string>();
            string root = "antipodeswines";
            string item = root + "/wines/wine";
            XmlDocument doc = GetAllExportWines();

            XmlNodeList nodes = doc.SelectNodes(item);

            foreach (XmlNode node in nodes)
            {
                wineIDs.Add(node.Attributes["id"].Value);
            }

            return wineIDs;        
        }
        
        //not used at the moment
        private static XmlDocument GetAllExportWines()
        {
            XmlDocument doc = new XmlDocument();
            
            try
            {
                doc.Load(_baseUrl + "export-wine?md=all&pid=" + _pId + "&token=" + AuthToken);
            }
            catch (Exception ex)
            {
                new Logger("GetAllExportWines() - failed", ex.ToString());
            }

            return doc;

            #region xml structure
            //<?xml version="1.0" encoding="UTF-8"?>
            //    <antipodeswines>
            //        <response-status>success</response-status>
            //        <wines total="1">
            //            <wine id="25">
            //            <modified>2008-10-16 16:07:50</modified>
            //            <status>active</status>
            //            <name>Keith Tulloch Shiraz Viognier 2003</name>
            //            <description>Ripe blood plum & spice with silky mouth-feel & soft tannins. The subtle French Oak adds to the alluring perfumed length. Silky smooth and soft in texture, with supple tannin balance. 169 kr/btl.</description>
            //            <type>Red</type>
            //            <region>Lower Hunter Valley, Pokolbin</region>
            //            <country>Australia</country>
            //            <bottle-size>750</bottle-size>
            //            <unit>ml</unit>
            //            <alcohol>13.50</alcohol>
            //            <image>http://api.antipodeswines.com/images/wine_uploads/1117442501_58.jpg</image>
            //            </wine>
            //        </wines>
            //    </antipodeswines>
            #endregion
        }

        //not used at the moment
        private static XmlDocument GetCiabs()
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(_baseUrl + "export-ciab?md=all&pid=" + _pId + "&token=" + AuthToken);
            }
            catch (Exception ex)
            {
                new Logger("GetCiabs() - failed", ex.ToString());
            }

            return doc;

            #region xml structure
            //<?xml version="1.0" encoding="UTF-8"?>
            //    <antipodeswines>
            //        <response-status>success</response-status>
            //        <ciabs total="379">

            //            <ciab id="1099" type="single_order">
            //                <status>active</status>
            //                <modified>2011-02-23 14:33:45</modified>
            //                <name>Voyager Estate Chardonnay 2006</name>
            //                <description>Cliff Royle som  är vinmakare på Voyager Estate är naturligtvis ganska partisk när det gäller hans egna viner, men om sanningen ska fram så har han all rätt att vara stolt! Cliff är skolad klassiskt när det gäller viner och föredrar att göra ”traditionella” vita viner. Så också i det här fallet! Voyager Chardonnay 2007 är ett krispigt, rent och smakrikt vitt vin som har en elegant ek. Det här är strålande till mat och han rekommenderar att man dricker vinet lite varmare än vad man vanligen gör med ett vitt vin för att smakerna ska komma fram ordentligt!</description>
            //                <color>white</color>
            //                <image />
            //                <bottle-no>6</bottle-no>
            //                <price>1689</price>
            //                <in-stock>yes</in-stock>
            //                <wines>
            //                    <wine id="105" quantity="6" />
            //                </wines>
            //            </ciab>

            //        </ciabs>
            //    </antipodeswines>

            #endregion
        }

    }

}
