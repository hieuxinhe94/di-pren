using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DIClassLib.CardPayment.Nets;
using DIClassLib.Misc;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using EPiServer.SpecializedProperties;
using EPiServer.Filters;
using DagensIndustri.Tools.Classes;


namespace DagensIndustri
{
	public partial class tmp : DiTemplatePage
	{

		//public SsoLoginHandler SsoLoginHndler 
		//{
		//    get
		//    {
		//        if (Session["SsoLoginHandler"] != null)
		//        {
		//            SsoLoginHandler lg = new SsoLoginHandler(System.Web.HttpContext.Current.Request.Url.AbsoluteUri + "?code=123");
		//            Session["SsoLoginHandler"] = lg;
		//        }

		//        return (SsoLoginHandler)Session["SsoLoginHandler"];
		//    }
		//    set
		//    {
		//        Session["SsoLoginHandler"] = value;
		//    }
		//}


		//protected override void OnInit(EventArgs e)
		//{
		//    base.OnInit(e);
		//    base.UserMessageControl = UserMessageControl;
		//}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				bool displayTestGui = false;
				bool.TryParse(MiscFunctions.GetAppsettingsValue("DisplayTestGui").ToLower(), out displayTestGui);
				PlaceholderTestGui.Visible = displayTestGui;
			}

			if (Request["ResponseCode"] != null)
			{
				var ret = new NetsCardPayReturn();
				//var em = ret.NetsPreparePersisted.EmailAddress;

				ret.HandleNetsReturn("", "petter@di.se");
			}


			//var x = CustomerPropertyHandler.GetCustomersCusProps(3513209);
			//foreach (var a in x)
			//{
			//	Response.Write("PropCode:" + a.PropCode + " , PropValue:" + a.PropValue + "<br>");
			//}

			//XDocTest();

			string s;

			#region old tests
			//Serializer sz = new Serializer();
			//object o = sz.GetObjectFromFile(31358);

			//ConferenceExposer exp = new ConferenceExposer();
			//exp.GetUpcomingConferences();


			//Subscriber sc = new Subscriber("Petter", "Luotsinen", "", "petter@di.se", "", "", "", "", "", "");

			//AddCustAndSubHandler hdlr = new AddCustAndSubHandler();
			//var ret = hdlr.TryAddCustAndSub("P1301001E", "", false, PaymentMethod.TypeOfPaymentMethod.Invoice, DateTime.Now, "", sc);

			//Response.Write(ret.ToString());



			//string[] s = MdbDbHandler.GetCustomerName(3513209);
			//if (s != null && s.Length > 0)
			//    Response.Write(s[0]);
			//else
			//    Response.Write("no data");

			//int cusno = MembershipDbHandler.GetCusno(HttpContext.Current.User.Identity.Name);

			//Dictionary<String, Object> dic = GetRequest("http://account.qa.newsplus.se/check-logged-in/json");  //{"token":"","id":"","remembered":"false"}
			//Dictionary<String, Object> dic = GetRequest("http://account.qa.newsplus.se/check-logged-in");
			//string s;
			#endregion

		}


		public void XDocTest()
		{
			#region test url:s
			//test url with PanHash
			//url.Append("https://epayment-test.bbs.no/Netaxept/Query.aspx?merchantId=520893&token=7g!DF2f-&TransactionID=2336ec6a29e744a9b65b75ddd3636315");

			//test url without PanHash
			//url.Append("https://epayment-test.bbs.no/Netaxept/Query.aspx?merchantId=520893&token=7g!DF2f-&TransactionID=3749ae4847634842acaa6a64083604bb");

			//test url with Error
			//https://epayment-test.bbs.no/Netaxept/Query.aspx?merchantId=520893&token=7g!DF2f-&TransactionID=6bafde3c774241e1ac270e2d146a305a
			#endregion

			var url = new StringBuilder();
			url.Append(Settings.Nets_UrlQuery);
			url.Append("?merchantId=" + Settings.Nets_MerchantId);
			url.Append("&token=" + Settings.Nets_Token);
			url.Append("&TransactionID=6bafde3c774241e1ac270e2d146a305a");

			try
			{
				var doc = XDocument.Load(url.ToString());

				//var query = from d in doc.Root.Descendants()
				//            where d.Name.LocalName == "ErrorLog"
				//            select d.Value;


				var bo = doc.Descendants("TransactionId").Any();

				//var bo = doc.Descendants("ErrorLog").Descendants("PaymentError").Any();
				//var bo2 = doc.Descendants("ErrorLog").Descendants("asdf").Any();
				//var bo3 = doc.Descendants("asdf").Descendants("asdf").Any();

				//var errRespCode = doc.Descendants("ErrorLog").Descendants("PaymentError").Descendants("ResponseCode");
				//if (errRespCode.Any())
				//{
				//    var x = errRespCode.FirstOrDefault().Value;
				//}

				//var itm2 = doc.Descendants("asdf").Descendants("PaymentError").Descendants("ResponseCode");
				//if (itm2.Any())
				//{
				//    var x = itm2.FirstOrDefault().Value;
				//}

				//if (doc.Descendants("ErrorLog").Any())
				//{
				//    //var items = doc.Descendants("ErrorLog").Descendants("");
				//}

				foreach (var n in doc.Nodes())
				{
					var x = n.ToString();
				}
			}
			catch (Exception ex)
			{
				var s = ex.ToString();
			}
		}


		//step: go to Nets
		protected void Button1_Click(object sender, EventArgs e)
		{
			//var awh = new AutowithdrawalHandler();
			//string ret = awh.MakeAutowithdrawalPayments();

			string a = "";
		}


		//private void MakeAutoRedrawalPayment()
		//{
		//List<Subscription> subs = new List<Subscription>();
		//long aurigaSubsId = 4079204;
		//int cusno = 1;
		//int subsno = 2;
		//int campno = 3;
		//Guid pageGuid = new Guid("28233667-6512-4D0E-9E18-4B56F4D52EC1");
		//DateTime dateSubsEnd = DateTime.Parse("2013-04-05 13:27:50".ToString());
		//Awd aw = new Awd(aurigaSubsId, cusno, subsno, campno, pageGuid, dateSubsEnd.Date, subs);
		//}


		private void EpiStuff()
		{
			PageData pd = new PageData();
			pd = CurrentPage;
			pd = EPiServer.DataFactory.Instance.GetPage(PageReference.StartPage);

			string s = pd.Property["MainIntro"].ToString(); //kollar bara på CurrentPage
			string s2 = pd["MainIntro"].ToString(); //om MainIntro ej finns på sidan - kollar då om MainIntro finns som dynamisk egenskap

			PageReference pr = pd["StartNode"] as PageReference;

			pd = EPiServer.DataFactory.Instance.GetPage(pr);
			pd = EPiServer.DataFactory.Instance.GetPage(PageReference.Parse("112"));
			pd = EPiServer.DataFactory.Instance.GetPage(PageReference.Parse(pd["StartNode"].ToString()));

			CurrentPage.CreateWritableClone();

			PageData parent = EPiServer.DataFactory.Instance.GetPage(pd.ParentLink);

			EPiServer.DataFactory.Instance.GetChildren(CurrentPage.PageLink);
			EPiServer.DataFactory.Instance.GetDescendents(CurrentPage.PageLink);
			EPiServer.DataFactory.Instance.Move(CurrentPage.PageLink, PageReference.WasteBasket);
			pr = pd.PageLink;
			string url = pd.LinkURL;

			PageDataCollection pdc = EPiServer.DataFactory.Instance.GetChildren(CurrentPage.PageLink);
			//PageList1.DataSource = pdc;
			//PageList1.DataBind();

			PropertyLinkCollection plc = pd["LinkCollection"] as PropertyLinkCollection; //Use DiLinkCollection instead
			PageReferenceCollection prc = new PageReferenceCollection();
			//EPiServer.DataFactory.Instance.GetDescendents(CurrentPage.PageLink);

			EPiServer.Core.Html.TextIndexer.StripHtml(s, 0);

			//EPiServer.DataAbstraction.Category
			//EPiServer.Global.EPDataFactory // (some of them obsolete)

			PageDataCollection pdc2 = EPiServer.DataFactory.Instance.GetChildren(CurrentPage["ConferenceStartNode"] as PageReference);
			FilterForVisitor.Filter(pdc);
			new FilterPropertySort("Date", FilterSortDirection.Ascending).Filter(pdc);

			string next = Translate("/common/next");

			if (IsValue("MainIntro")) //Kollar om CurrentPage property MainIntro är null eller tom sträng
			{ }
			else if (CurrentPage.Property["MainIntro"] != null && CurrentPage.Property["MainIntro"].Value != null)
			{ }
			else if (CurrentPage["MainIntro"] != null) // kollar om det finns dynamic property med
			{ }
			else if (EPiFunctions.HasValue(CurrentPage, "MainIntro")) //Som isvalue men det kollar på den sida som man skickar in, inte dynamic property
			{ }

			EPiFunctions.SettingsPageSetting(pd, "test");

			//Response.Redirect(EPiServer.DataFactory.Instance.GetPage(PageReference.StartPage).LinkURL);

			//EPiServer.Filters.FilterForVisitor.Filter(pdc);
		}

		//protected void btnSendSMSTest_Click(object sender, EventArgs e)
		//{
		//Try Apsis Send SMS!
		//ApsisWsHandler awh = new ApsisWsHandler(true);

		//var phoneNumberInfo = MiscFunctions.GetSeparatedCountryCodePhoneNumber("0702018817");
		//String s = awh.ApsisSendSms("Pers functional test from testserver Dagens industri", phoneNumberInfo[0], phoneNumberInfo[1]);
		//btnSendSMSTest.Enabled = false;
		//btnSendSMSTest.Text = s;


		//Try Apsis transactional email!
		//string identifier = Guid.NewGuid().ToString();
		//ApsisCustomer demoCust = new ApsisCustomer() { Email = "perxx.lundkvistxx@di.se", Name = "Per", CustomerId = 3513209, ApsisProjectGuid = "fa9644f7-d821-4c1e-be53-a514da84d4b0" };//42475F467346415A44714143, 41435C477641425B44734143
		//int statusId = awh.ApsisSendEmail(identifier, demoCust);   //test mode: no mail sent, mailId -10 to -1000
		//btnSendSMSTest.Text = statusId.ToString();


		//MailSenderDbHandler db = new MailSenderDbHandler();
		//int numNewBounces = db.AddBouncesFromApsis(true);
		//btnSendSMSTest.Text = "Antal studsar: " + numNewBounces.ToString();
		//}

		//protected void btnPerTest_Click(object sender, EventArgs e)
		//{
		//ApsisWsHandler awh = new ApsisWsHandler(true);
		//btnPerTest.Text = awh.PerIsTesting();
		//}


		#region old tests


		//public string query = "Dikonferens";
		//string count = "3";
		//public string url = "https://api.twitter.com/1.1/statuses/user_timeline.json";
		////public string query = "petterluo";
		////public string url = "https://api.twitter.com/1.1/users/search.json" ;        
		////public string url = "https://api.twitter.com/1.1/statuses/home_timeline.json";
		////public string url = "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=petterluo";

		//findUserTwitter(url, query);

		//public void findUserTwitter(string resource_url, string screenName)
		//{

		//    // oauth application keys
		//    var oauth_token = "91660986-A2Fl24SZB4baLXCwvvqOofo28t0bOy90dfDtJzgtu"; 
		//    var oauth_token_secret = "aFeN7eOQgXwVydcDk1KZrxqsrTNCLAB4okDxOq074Rs"; 
		//    var oauth_consumer_key = "FoiVpnDjih7Il4WytqQMyA";
		//    var oauth_consumer_secret = "oQPSo9Ms9E65oUMxDBvuiLcCNG5QvNUQqfRzCjbiA4";

		//    // oauth implementation details
		//    var oauth_version = "1.0";
		//    var oauth_signature_method = "HMAC-SHA1";

		//    // unique request details
		//    var oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
		//    var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		//    var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();


		//    // create oauth signature
		//    var baseFormat = "count={7}&oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
		//                    "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&screen_name={6}";

		//    var baseString = string.Format(baseFormat,
		//                                oauth_consumer_key,
		//                                oauth_nonce,
		//                                oauth_signature_method,
		//                                oauth_timestamp,
		//                                oauth_token,
		//                                oauth_version,
		//                                Uri.EscapeDataString(screenName),
		//                                Uri.EscapeDataString(count) 
		//                                );

		//    baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

		//    var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
		//                            "&", Uri.EscapeDataString(oauth_token_secret));

		//    string oauth_signature;
		//    using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
		//    {
		//        oauth_signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
		//    }

		//    // create the request header
		//    var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
		//                       "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
		//                       "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
		//                       "oauth_version=\"{6}\"";

		//    var authHeader = string.Format(headerFormat,
		//                            Uri.EscapeDataString(oauth_nonce),
		//                            Uri.EscapeDataString(oauth_signature_method),
		//                            Uri.EscapeDataString(oauth_timestamp),
		//                            Uri.EscapeDataString(oauth_consumer_key),
		//                            Uri.EscapeDataString(oauth_token),
		//                            Uri.EscapeDataString(oauth_signature),
		//                            Uri.EscapeDataString(oauth_version)
		//                    );



		//    ServicePointManager.Expect100Continue = false;

		//    // make the request
		//    //var postBody = "screen_name=" + Uri.EscapeDataString(query);
		//    var postBody = "count=" + Uri.EscapeDataString(count) + "&screen_name=" + Uri.EscapeDataString(screenName);
		//    resource_url += "?" + postBody;
		//    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
		//    request.Headers.Add("Authorization", authHeader);
		//    request.Method = "GET";
		//    request.ContentType = "application/x-www-form-urlencoded";
		//    var response = (HttpWebResponse)request.GetResponse();
		//    var reader = new StreamReader(response.GetResponseStream());
		//    var objText = reader.ReadToEnd();
		//    myDiv.InnerHtml = objText;/**/
		//    StringBuilder sb = new StringBuilder();
		//    try
		//    {
		//        JavaScriptSerializer js = new JavaScriptSerializer();
		//        //var jsonObj = js.DeserializeObject(objText);
		//        //var jsonObj = (List<Dictionary<string, object>>)js.DeserializeObject(objText);


		//        //var obj = js.Deserialize<List<Dictionary<string, object>>>(objText);
		//        List<Dictionary<string, object>> obj = js.Deserialize<List<Dictionary<string, object>>>(objText);
		//        foreach (var item in obj)
		//        {
		//            //foreach (KeyValuePair<string, object> o in (Dictionary<string, object>)obj[0])
		//            foreach (KeyValuePair<string, object> o in (Dictionary<string, object>)item)
		//            {
		//                sb.Append(o.Key + " ");

		//                if (o.Value is Dictionary<string, object>)
		//                {
		//                    //var title = (o as Dictionary<string, object>)["title"];
		//                    sb.Append("---- ny dic ----");
		//                }
		//                else
		//                    sb.Append(o.Value);

		//                sb.Append("<br>");
		//            }
		//            sb.Append("<hr>");
		//        }

		//        //var jsonDat = js.Deserialize<Dictionary<string, object>>(objText);
		//        //var jsonDat = js.DeserializeDynamic(objText);

		//        //foreach (KeyValuePair<string, object> item in (Dictionary<string, object>)jsonObj)
		//        //{
		//        //    html += item.Key + "=" + item.Value + "<br/>";
		//        //}

		//        //JArray jsonDat = JArray.Parse(objText);
		//        //for (int x = 0; x < jsonDat.Count(); x++)
		//        //{
		//        //    //html += jsonDat[x]["id"].ToString() + "<br/>";
		//        //    html += jsonDat[x]["text"].ToString() + "<br/>";
		//        //    // html += jsonDat[x]["name"].ToString() + "<br/>";
		//        //    html += jsonDat[x]["created_at"].ToString() + "<br/>";

		//        //}
		//        myDiv.InnerHtml = sb.ToString();
		//    }
		//    catch (Exception twit_error)
		//    {
		//        myDiv.InnerHtml = sb + twit_error.ToString();
		//    }
		//}




		//SearchOutput searchRes = RequestHandler.SearchByEmail("petter@di.se");
		//string userId = searchRes.users.id;  //6Fr2gO7AhquH7Cjd3Lj70Z

		//ImportOutput imp = BonDigHandler.CreateImport("2aNh0ERDIpMTpsSQubMPt1", "6Fr2gO7AhquH7Cjd3Lj70Z", 0, 0);

		//string json = "{\"requestId\":\"2kQRBBTbCE3t3jYCHEr6cv\",\"httpResponseCode\":\"201\",\"entitlement\":{\"id\":\"42Zs6dPqG2XJdDIxqc7rJ5\",\"created\":\"1370256006936\",\"updated\":\"1370256006936\",\"location\":\"/42Zs6dPqG2XJdDIxqc7rJ5\",\"brandId\":\"5DuzcZz0j8u0zArSNzZgHO\",\"productId\":\"2aNh0ERDIpMTpsSQubMPt1\",\"userId\":\"6Fr2gO7AhquH7Cjd3Lj70Z\",\"renewable\":\"true\",\"type\":\"TEMPORARY_SUBSCRIPTION\",\"state\":\"VALID\",\"productTags\":[\"Di-Wineclub\"],\"validFrom\":\"1370256006912\",\"validTo\":\"1370342406912\",\"externalSubscriptionId\":\"0\",\"externalSubscriberId\":\"0\"}}";
		//JavaScriptSerializer js = new JavaScriptSerializer();
		//ImportOutput uo = js.Deserialize<ImportOutput>(json);


		//DIClassLib.StudentVerification.StudentVerifier sv = new DIClassLib.StudentVerification.StudentVerifier();
		//string s0 = sv.VerifyByBirthNum("7510101234");


		//string token = RequestHandler._getAccessToken;
		//string t = "1J2ID2JFWknYfIzOHarCgh"; //"5TFekuWqXZ7PnkNuwSG5pM";
		//List<long> op = RequestHandler.TryGetCirixCusnosFromBonDig(t);
		//string s = "";

		//ApsisWsHandler ws = new ApsisWsHandler();
		//DataSet ds = ws.GetNewsletterDetails("843330");
		//int i = 0;

		//LabelRet.Text = "";



		//protected void Button1_Click(object sender, EventArgs e)
		//{
		//    //List<long> op = RequestHandler.TryGetCirixCusnosFromBonDig(TextBox1.Text);
		//    string apsisProjGuid = TextBoxProjGuid.Text;
		//    string customerName = TextBoxName.Text;
		//    string email = TextBoxEmail.Text;
		//    string cusno = TextBoxCusno.Text;
		//    ApsisWsHandler ws = new ApsisWsHandler();
		//    LabelRet.Text = ws.InsertTransaction_ForTesting(apsisProjGuid, customerName, email, cusno);
		//}


		//protected void btnSPLogin_Click(object sender, EventArgs e)
		//{
		//String t = RequestHandler.GetAccessToken;

		//String result = RequestHandler.WebReqPostParam("https://api.bonnier.se/v1/oauth/token", "client_id=" + tbUsername.Text + "&client_secret=" + tbPassword.Text + "&grant_type=client_credentials&scope=DITABLET");


		//string ret = "";
		////string token = GetAccessToken;
		//String url = "https://api.bonnier.se/v1/oauth/token";
		////String parameters = "client_id=" + tbUsername.Text + "&client_secret=" + tbPassword.Text + "&grant_type=client_credentials&scope=DITABLET";
		//WebRequest request = WebRequest.Create(url);
		//request.Method = "POST";

		////byte[] byteArray = Encoding.UTF8.GetBytes(parameters);
		//request.ContentType = "application/x-www-form-urlencoded";
		////request.ContentLength = byteArray.Length;

		//Stream dataStream = request.GetRequestStream();
		////dataStream.Write(byteArray, 0, byteArray.Length);
		//dataStream.Close();

		//WebResponse response = request.GetResponse();
		//string tmp = ((HttpWebResponse)response).StatusDescription;

		//dataStream = response.GetResponseStream();
		//StreamReader reader = new StreamReader(dataStream);
		//ret = reader.ReadToEnd();

		//reader.Close();
		//dataStream.Close();
		//response.Close();

		//return CleanUpJson(ret);



		//String response = RequestHandler.WebReqPostParam("http://api.qa.newsplus.se/v1/oauth/token", "client_id=" + tbUsername.Text + "&client_secret=" + tbPassword.Text + "&grant_type=client_credentials&scope=DITABLET");


		//curl -i 'http://api.qa.newsplus.se/v1/oauth/token' -X POST \
		//-dclient_id='end.user@email.com' \
		//    -dclient_secret='secret password' \
		//-dgrant_type='client_credentials' \
		//-dscope='appId:DN%2B'



		//Token token = new Token();
		//HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.qa.newsplus.se/v1/oauth/token");
		//request.Method = "POST";

		//Stream sw = request.GetRequestStream();
		//byte[] byteData = Encoding.ASCII.GetBytes("client_id=" + tbUsername.Text + "&client_secret=" + tbPassword.Text + "&grant_type=client_credentials&scope=DITABLET");
		//sw.Write(byteData, 0, byteData.Length);
		//sw.Close();

		//HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		//StreamReader sr = new StreamReader(response.GetResponseStream());
		//string json = sr.ReadToEnd();
		//sr.Close();

		//JavaScriptSerializer js = new JavaScriptSerializer();




		//token = js.Deserialize<Token>(CleanUpJson(json));
		//token.Expires = DateTime.Now.AddSeconds(double.Parse(token.expires_in));

		//return token;
		//}


		//private string SendMba()
		//{
		//    DiMBA.DIMBA ws = new DiMBA.DIMBA();

		//    string firstname = "Petter TEST";
		//    string lastname = "Luotsinen TEST";
		//    string address = "TESTGATAN 1";
		//    string zip = "12345";
		//    string city = "TESTSTAD";
		//    string mobile = "0705555555";
		//    string email = "petter@di.se";
		//    string linkedin = "";
		//    string company = "DI TEST";
		//    string position = "TESTARE";
		//    string personalNumber = "19100821";
		//    string education = "TESTSKOLA";
		//    string educationPoints = "180";
		//    string workYears = "5";
		//    string motivation = "TEST TEST TEST";
		//    string nomination = "";
		//    string englishLevel = "5";

		//    return ws.InsertContestant(nomination, email, firstname, lastname, personalNumber, company, position, address, zip, city, mobile, education, educationPoints, workYears, englishLevel, linkedin, motivation);
		//}

		//private void TestCusProps()
		//{
		//    long cusno = long.Parse(TextBoxCusno.Text);
		//    string email = GetStrVal(TextBoxEmail.Text);
		//    string phone = GetStrVal(TextBoxPhone.Text);
		//    string birth = GetStrVal(TextBoxBirth.Text);
		//    bool? isGold = GetBoVal(TextBoxIsGold.Text);
		//    bool? isPersonal = GetBoVal(TextBoxIsPersonal.Text);
		//    new CustomerPropertyHandler(cusno, email, phone, birth, isGold, isPersonal);
		//}

		//private string GetStrVal(string s)
		//{
		//    if (s.ToLower() == "null")
		//        return null;

		//        return s;
		//}

		//private bool? GetBoVal(string s)
		//{
		//    s = s.ToLower();

		//    if (s == "null")
		//        return null;
		//    if (s == "true")
		//        return true;
		//    if (s == "false")
		//        return false;

		//    return null;
		//}


		//private void RunVerifyServices()
		//{

		//    ServiceVerifierStatus adlibrisStatus = ServiceVerifier.VerifyAdlibris();
		//    Response.Write("<b>Adlibris:</b> " + adlibrisStatus.ToString());
		//    Response.Write("<br/><b> - IsValid:</b> " + ServiceVerifier.AdlibrisIsValid);

		//    ServiceVerifierStatus oboStatus = ServiceVerifier.VerifyOBO();
		//    Response.Write("<br/><b>OBO:</b> " + oboStatus.ToString());
		//    Response.Write("<br/><b> - IsValid:</b> " + ServiceVerifier.OBOIsValid);

		//    ServiceVerifierStatus apsisStatus = ServiceVerifier.VerifyApsis();
		//    Response.Write("<br/><b>Aspsis:</b> " + apsisStatus.ToString());
		//    Response.Write("<br/><b> - IsValid:</b> " + ServiceVerifier.ApsisIsValid);

		//    ServiceVerifierStatus businessCalStatus = ServiceVerifier.VerifyBusinessCalendar();
		//    Response.Write("<br/><b>BusinessCalendar:</b> " + apsisStatus.ToString());
		//    Response.Write("<br/><b> - IsValid:</b> " + ServiceVerifier.BusinessCalendarIsValid);

		//    ServiceVerifierStatus diRssStatus = ServiceVerifier.VerifyDiRss();
		//    Response.Write("<br/><b>Di RSS:</b> " + diRssStatus.ToString());
		//    Response.Write("<br/><b> - IsValid:</b> " + ServiceVerifier.DiRssIsValid);

		//    ServiceVerifierStatus busCalStatus = ServiceVerifier.VerifyBusinessCalendar();
		//    Response.Write("<br/><b>Google Map:</b> " + busCalStatus.ToString());
		//    Response.Write("<br/><b> - IsValid:</b> " + ServiceVerifier.BusinessCalendarIsValid);
		//}


		//public string ImportTidningsKungenCusts()
		//{
		//    StringBuilder sb = new StringBuilder();
		//    Subscription sub = null;

		//    DataSet ds = DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("DagensIndustriMISC", "x_getTidningsKungen", null);

		//    if (DbHelpMethods.DataSetHasRows(ds))
		//    {
		//        foreach (DataRow dr in ds.Tables[0].Rows)
		//        {
		//            int id = int.Parse(dr["id"].ToString());

		//            try
		//            {
		//                string name = dr["namn"].ToString().Trim();
		//                string comp = dr["firma"].ToString().Trim();
		//                Person subscriber = new Person(true, true, "", "", "", comp, dr["gatuadress"].ToString().Trim(), dr["gatunr"].ToString().Trim(),
		//                                    dr["uppg"].ToString().Trim(), dr["trappor"].ToString().Trim(), dr["lghNr"].ToString().Trim(), dr["postnr"].ToString().Trim(), dr["ort"].ToString().Trim(),
		//                                    dr["telMob"].ToString().Trim(), "", "", "", "", "");
		//                SetCirixNames(comp, name, subscriber);


		//                string payComp = dr["betFirma"].ToString().Trim();
		//                string payName = dr["betNamn"].ToString().Trim();
		//                Person payer = new Person(false, true, "", "", "", comp, dr["betGatuadress"].ToString().Trim(), dr["betGatunr"].ToString().Trim(),
		//                                    dr["betUppg"].ToString().Trim(), dr["betTrappor"].ToString().Trim(), "", dr["betPostr"].ToString().Trim(), dr["betOrt"].ToString().Trim(),
		//                                    "", "", "", "", "", "");
		//                SetCirixNames(payComp, payName, payer);

		//                string campId = dr["campId"].ToString().Trim();
		//                if (campId == "N1101000E")  //N12120020E
		//                {
		//                    long campno = 1173;

		//                    if (sub == null)
		//                        sub = new Subscription(PaymentMethod.TypeOfPaymentMethod.Invoice, campno);

		//                    sub.TargetGroup = dr["targetgroup"].ToString().Trim();
		//                    sub.Subscriber = subscriber;
		//                    sub.SubscriptionPayer = payer;

		//                    string res = CirixDbHandler.TryInsertSubscription2(sub, null);
		//                    if (!string.IsNullOrEmpty(res))
		//                        sb.Append(id + ": " + res + "<br>");
		//                }
		//            }
		//            catch (Exception ex)
		//            {
		//                sb.Append("EX id: " + id + "<br>");
		//                new Logger("ImportTidningsKungenCusts failed for name: " + name, ex.ToString());
		//            }
		//        }

		//        return sb.ToString();
		//    }

		//    return "test";
		//}



		//try
		//{
		//    int high = int.Parse(TextBoxHigh.Text.ToString());
		//    int low = int.Parse(TextBoxLow.Text.ToString());

		//    if (high > low)
		//    {
		//        StringBuilder sb = new StringBuilder();
		//        sb.Append("id high: " + high.ToString() + ", id low: " + low.ToString() + "<br>");
		//        sb.Append("start: " + DateTime.Now.ToString() + "<br>");
		//        //sb.Append(DIClassLib.EPiJobs.SyncSubs.SyncSubsHandler.SyncChangedCusts(high, low));
		//        sb.Append("<br>done:" + DateTime.Now.ToString());

		//        LabelMess.Text = sb.ToString();
		//    }
		//}
		//catch (Exception ex)
		//{
		//    LabelMess.Text = ex.ToString();
		//}


		//public List<Subscription> SortSubsByEndDateDesc()
		//{
		//    List<Subscription> s = new List<Subscription>();
		//    //s.Add(new Subscription2() { Subsno = 1, SubsEndDate = new DateTime(2011, 07, 02), SuspendDate = new DateTime(2011, 07, 01) });
		//    s.Add(new Subscription() { SubsNo = 4, SubsEndDate = new DateTime(2011, 6, 1) });
		//    s.Add(new Subscription() { SubsNo = 2, SubsEndDate = new DateTime(2011, 7, 20), SuspendDate = new DateTime(2011, 7, 1) });
		//    s.Add(new Subscription() { SubsNo = 3, SubsEndDate = new DateTime(2011, 6, 15) });
		//    s.Add(new Subscription() { SubsNo = 1, SubsEndDate = new DateTime(2011, 7, 10) });
		//    s.Sort();
		//    return s;
		//}


		//private void CallMbaWs()
		//{
		//    try
		//    {
		//        DiMBA.DIMBA ws = new DiMBA.DIMBA();

		//        string firstname = "FirstNameInput";
		//        string lastname = "LastNameInput";
		//        string address = "AddressInput";
		//        string zip = "12345";
		//        string city = "CityInput";
		//        string mobile = "TelephoneInput";
		//        string email = "EmailInput";
		//        string linkedin = "LinkedInInput";
		//        string company = "CompanyInput";
		//        string position = "PositionInput";
		//        string personalNumber = "PersonalInput";
		//        string education = "AcademicEducationInput";
		//        string educationPoints = "250";
		//        string workYears = "5";
		//        string motivation = "MotivationInput";
		//        string nomination = "NominationRadioButtonList";
		//        string englishLevel = "3";

		//        string mbaResponse = ws.InsertContestant(nomination, email, firstname, lastname, personalNumber, company, position, address, zip, city, mobile, education, educationPoints, workYears, englishLevel, linkedin, motivation);

		//        Response.Write("mbaResponse " + mbaResponse);
		//    }
		//    catch (Exception ex)
		//    {
		//        Response.Write("MBAForm() - ex<br>" + ex.ToString());
		//        //new Logger("MBAForm() - failed", ex.ToString());
		//    }
		//}


		//BonDigHandler.AddSubsToBonDig(3371693, "petter.luotsinen@di.se", "pass1");
		//bool bo = BonDigHandler.AddSubsToBonDig(1, "petter.luotsinen@di.se", "pass1");

		//UserOutput uo = BonDigHandler.GetUser("3371693");
		//UserOutput uo = BonDigHandler.GetUser("1");

		//SearchParam sp = new SearchParam("test@test.se");
		//SearchInput si = new SearchInput(sp);
		//string s = si.ToJson();

		//string json = RequestHandler.PostRequest(ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString(), s);

		//string s = RequestHandler.PostRequest(ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString(), "{\"q\":\"{\"email_s\":\"petter.luotsinen@di.se\"}\"}");
		//string s = RequestHandler.PostRequest(ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString(), "{\"q\":\"[\"email_s\":\"petter.luotsinen@di.se\"]\"}");

		//The remote server returned an error: (400) Bad Request.
		//string s = RequestHandler.MakeRequestByPost(ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString(), "{\"q=email_s\":\"petter.luotsinen@di.se\"}");

		//string s = RequestHandler.MakeRequestByGet(ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString(), "q=email_s:petter.luotsinen@di.se");

		//string jsonRet = RequestHandler.SearchByEmail("petter.luotsinen@di.se");
		//SearchOutput op = new SearchOutput();
		//op = op.GetSearchOutput(jsonRet);

		//string jsonRet2 = RequestHandler.SearchByEmail("petter.luotsinenxxx@di.se");
		//SearchOutput op2 = new SearchOutput();
		//op2 = op2.GetSearchOutput(jsonRet2);

		//The remote server returned an error: (500) Internal Server Error.
		//string s = RequestHandler.PostRequest(ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString(), "{q=email_s:\"petter.luotsinen@di.se\"}");

		//BonDigHandler.AddSubsToBonDig(3371693, "petter.luotsinenmmmmmm@di.se", "test");
		//string s = "";


		//ShowMessage("apa", false, true);
		//SubscriptionUser DiGoldMember = new SubscriptionUser();
		//string s = string.Format(Translate("/mba/mail/nominee/subject"), "APA");
		//string s = System.Web.HttpContext.Current.User.Identity.Name;
		//DataSet ds = MsSqlHandler.GetCampaignTargetGroups(399);


		//DiGoldMembershipPopup diGoldmembershipPopup = EPiFunctions.FindDiGoldMembershipPopup(Page) as DiGoldMembershipPopup;
		//if (diGoldmembershipPopup != null)
		//{
		//    //Register script for Buy hyperlink
		//    diGoldmembershipPopup.RegisterSetReturnURLScript(LinkButton1, "http://localhost/templates/public/pages/digoldstartpage.aspx?id=45");
		//}



		//Person2 p1 = new Person2(true, true, "erik", "lennmark", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
		//List<Person2> p2 = CirixDbHandler.FindCustomers(p1);

		//AddSubsToExistingCust();

		//CallMbaWs();

		//vastergard@swipnet.se
		//
		//Person p = new Person(true, true, "nils erik", "nilsson", "", "", "Vrams Bygata", "40", "", "", "", "29010", "tollarp", "", "nyyy@nyyy.se", "", "", "", "0705555555");
		//List<Person> pers = CirixDbHandler.FindCustomerByPerson(p);

		//Response.Write(pers.Count.ToString());

		//foreach (Person p1 in pers)
		//{
		//    long cusno = p1.Cusno;
		//}

		//CirixDbHandler.ChangeCusInvMode(3258679, "01", "02");


		//DIClassLib.GoldMember.GoldMemberHandler gh = new DIClassLib.GoldMember.GoldMemberHandler();
		//DataSet ds = gh.FindCustomer("197510177111");
		//int i = 0;

		//110519 - par har fixat saker...
		//DIClassLib.OneByOne.Obo.OboSubscribe(3454135, "200772534", "302009340", "402246120");
		//DIClassLib.OneByOne.Obo.OboUnsubscribe(3454135, "200772534", "302009340", "402246120");

		//DIClassLib.OneByOne.Obo.OboSubscribe(1476888, "200938419", "301302105", "401391921");
		//DIClassLib.OneByOne.Obo.OboUnsubscribe(1476888, "200938419", "301302105", "401391921");

		//List<CompanyEvent> evs = BusCalDbHandler.GetCompEvsMillistream();
		//List<Company> comps = BusCalDbHandler.GetCompanysFromMS();

		//DIClassLib.OneByOne.Obo.OboSubscribe(3454106, "200438643", "301744130", "401875734");

		//List<CustomerProp> cpAll = DIClassLib.Misc.CustomerPropertyHandler.GetAllCusProps(3163199);
		//List<CustomerProp> cpNew = new List<CustomerProp>();
		//List<CustomerProp> cpNew = DIClassLib.Misc.CustomerPropertyHandler.GetNewCusProps("a@a.se", "070123456", "19851025", true);
		//List<CustomerProp> cpNew = DIClassLib.Misc.CustomerPropertyHandler.GetNewCusProps("a2@a.se", "0270123456", "19851225", true);
		//List<CustomerProp> cpNew = DIClassLib.Misc.CustomerPropertyHandler.GetNewCusProps("", "", "", false);
		//List<CustomerProp> cpNew = DIClassLib.Misc.CustomerPropertyHandler.GetNewCusProps(null, null, null, null);

		//DIClassLib.Misc.CustomerPropertyHandler.InsertCusProps(3163199, cpAll, cpNew);

		//bool bo1 = DIClassLib.BonnierDigital.BonDigHandler.AddSubsToBonDig(3371693, "ok2@petter.it", "petteri");
		//bool bo2 = DIClassLib.BonnierDigital.BonDigHandler.AddSubsToBonDig(33716931, "ok2@petter.it", "petteri");



		//string[] s = MdbDbHandler.GetCustomerInfo(2036214);
		//string[] s1 = MdbDbHandler.GetCustomerInfo(1);
		//string[] s2 = MdbDbHandler.GetCustomerInfo(2301586);

		//GetAllExportWines();
		//XmlDocument doc1 = GetCiabs();
		//WineItem wi = AntipodesWines.GetWineById(767);
		//WineItem wi2 = AntipodesWines.GetWineById(-8);

		//AntipodesWineItem1.WineId = 767;

		//DIClassLib.WineTipSms.WineTipSmsHandler.SendWineTipSms(1);

		//bool bo = CirixDbHandler.SubscriptionIsPersonal(1);
		//Response.Write(bo.ToString());


		//DIClassLib.EPiJobs.Apsis.ApsisWsHandler wh = new DIClassLib.EPiJobs.Apsis.ApsisWsHandler();
		//Response.Write("ID: " + wh.ApsisSendMailTEMP().ToString());

		//DataSet ds = CirixDbHandler.GetCustomer(1);


		//DataSet ds = CirixDbHandler.GetCustomer(3513116);
		//DataSet ds = CirixDbHandler.GetCustomer(3453968);



		//SyncSingleCust(3453979);

		//DIClassLib.EPiJobs.SyncSubs.SyncSubsHandler.SyncChangedCusts();

		//Response.Write(hs.Count);

		//Tools.Classes.Membership.DiMembershipProvider a = new Tools.Classes.Membership.DiMembershipProvider();
		//Response.Write(a.ValidateUser("3521667", "I576882").ToString());

		//DataSet ds = CirixDbHandler.GetCustomer(3371693);


		//3455079
		//Response.Write(DIClassLib.EPiJobs.SyncSubs.SyncSubsHandler.SyncChangedCusts());
		//DIClassLib.EPiJobs.SyncSubs.SyncSubsHandler.SyncSingleCust(3371693);    //.SyncChangedCusts();


		//List<Company> comps = new List<Company>();
		//comps.Add(new Company(1, "f1", "isin1"));
		//comps.Add(new Company(1, "f2", "isin2"));

		//CompanysInJsHandler ch = new CompanysInJsHandler(comps);

		#endregion

	}

}