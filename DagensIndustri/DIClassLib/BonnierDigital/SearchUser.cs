using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Configuration;


namespace DIClassLib.BonnierDigital
{


    //{"httpResponseCode":"200","requestId":"3odkmZrx9jUxjFwir3hNzs","numItems":"1","startIndex":"0","totalItems":"1","query":"email_s:petter.luotsinen@di.se",
        //"users":{"id":"37CuBEwCK4pUqL0RHMlMYm","updated":"1312351612720","created":"1297175837607","location":"/37CuBEwCK4pUqL0RHMlMYm","brandId":"5DuzcZz0j8u0zArSNzZgHO","accountId":"58qAXgkcSrcHPOFSWCaZYf","email":"petter.luotsinen@di.se","firstName":"Petter","lastName":"Luotsinen","phoneNumber":"709132046"}
    //}
    public class SearchOutput
    {
        public string httpResponseCode;
        public string requestId;
        public string numItems;
        public string startIndex;
        public string totalItems;
        public string query;
        public FoundUser users;

        public SearchOutput() { }

        public SearchOutput GetSearchOutput(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            SearchOutput uo = js.Deserialize<SearchOutput>(json);
            return uo;
        }
    }


    public class FoundUser
    {
        public string id;
        public string updated;
        public string created;
        public string location;
        public string brandId;
        public string accountId;
        public string email;
        public string firstName;
        public string lastName;
        public string phoneNumber;

        public FoundUser() { }
    }


}
