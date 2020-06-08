using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Configuration;
using Microsoft.VisualBasic;


namespace DIClassLib.BonnierDigital
{
    public class User
    {
        public User() { }

        public UserOutput CreateUser(string email, string firstName, string lastName, string phoneNumber, string password)
        {
            UserInput ui = new UserInput(email, firstName, lastName, phoneNumber, password);
            string json = RequestHandler.WebReqPostJson(ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString(), ui.ToJson());
            UserOutput uo = new UserOutput();
            uo = uo.GetUserOutput(json);
            return uo;
        }


        //public UserOutput GetUser(string cusno)
        //{
        //    UserGet ug = new UserGet(cusno);
        //    string json = RequestHandler.MakeRequestByPost(ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString(), ug.ToJson());
        //    UserOutput uo = new UserOutput();
        //    uo = uo.GetUserOutput(json);
        //    return uo;
        //}

        //not used at the moment
        //public UserOutput CreateUser(string email, string firstName, string lastName, string phoneNumber, string password)
        //{
        //    UserInput ui = new UserInput(email, firstName, lastName, phoneNumber, password);
        //    string json = RequestHandler.PostRequest(ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString(), ui.ToJson());
        //    UserOutput uo = new UserOutput();
        //    uo = uo.GetUserOutput(json);
        //    return uo;
        //}
    }


    //public class UserGet
    //{
    //    public string USER_ID;

    //    public UserGet(string cusno_)
    //    {
    //        USER_ID = cusno_;
    //    }

    //    public string ToJson()
    //    {
    //        JavaScriptSerializer js = new JavaScriptSerializer();
    //        string s = js.Serialize(this);
    //        return s;
    //    }
    //}


    public class UserInput
    {
        public string email;
        public string password;
        public string active;       //set to avoid bonnier digital welcome mail with confirmation link
        public string firstName;
        public string lastName;
        public string phoneNumber;
        //public string brandId;

        #region old constructor
        //public UserInput(string email_, string password_)
        //{
        //    email = email_;
        //    password = password_;
        //    active = GetMsSince1970();
        //    //firstName = "fn";
        //    //lastName = "en";
        //    //phoneNumber = "07055555555";
        //}
        #endregion

        public UserInput(string email_, string firstName_, string lastName_, string phoneNumber_, string password_)
        {
            email = email_;
            firstName = firstName_;
            lastName = lastName_;
            phoneNumber = phoneNumber_;
            password = password_;
            active = BonDigMisc.GetMsSince1970(DateTime.Now);
            //brandId = "";
        }

        public string ToJson()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string s = js.Serialize(this);
            return s;
        }

        //private string GetMsSince1970()
        //{
        //    DateTime dtStart = new DateTime(1970, 1, 1);
        //    DateTime dtEnd = DateTime.Now;
        //    long act = long.Parse((DateAndTime.DateDiff(DateInterval.Second, dtStart, dtEnd) * 1000).ToString());
        //    return act.ToString();   
        //}
    }

    public class UserOutput
    {
        public string httpResponseCode;
        public string requestId;
        public UserOutputUser user;

        public UserOutput() { }

        public UserOutput GetUserOutput(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            UserOutput uo = js.Deserialize<UserOutput>(json);
            return uo;
        }
    }


    public class UserOutputUser
    {
        public string id;
        public string updated;
        public string created;
        public string location;
        public string brandId;
        public string accountId;
        public string type;
        public string email;
        public string firstName;
        public string lastName;
        public string phoneNumber;
        public string active;
        public string termsAndConditionsAccepted;

        public UserOutputUser() { }
    }


    //ret from S+ login/create account page
    //"requestId":"1cE6PhwQa9B6AVgkx4z2rV","httpResponseCode":"200","user"
    //    :{
    //        "id":"6Fr2gO7AhquH7Cjd3Lj70Z",
    //        "created":"1351061028102",
    //        "updated":"1351087501762",
    //        "location":"/6Fr2gO7AhquH7Cjd3Lj70Z",
    //        "brandId":"5DuzcZz0j8u0zArSNzZgHO",
    //        "accountId":"7yUQECg8Hx5yMQ6MErtfsC",
    //        "type":"CUSTOMER",                      //new
    //        "email":"petter@di.se",
    //        "firstName":"Petter",
    //        "lastName":"Luotsinen",
    //        "active":"1351062066819",
    //        "termsAndConditionsAccepted":"1351061028052"  //can be null
    //    }

    //create new ret from test
    //"requestId":"18dposTAfzloZsFixBZeEk","httpResponseCode":"201","user"
    //    :{
    //        "id":"0AKRRDDrtFFrc3n5BS9Tqu",
    //        "created":"1351326924631",
    //        "updated":"1351326924631",
    //        "location":"/0AKRRDDrtFFrc3n5BS9Tqu",
    //        "brandId":"5DuzcZz0j8u0zArSNzZgHO",
    //        "accountId":"0jPAOMhXaTrJTLY7JKZ2mV",
    //        "type":"CUSTOMER",                      //new
    //        "email":"ok62@petter.it",
    //        "firstName":"Pet61",
    //        "lastName":"Luo60",
    //        "phoneNumber":"0705555555",           //can be null
    //        "active":"1351334122000"
    //    }

}
