using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.DbHelpers;
using EPiServer.Core;

namespace DIClassLib.SignUp
{
    public class SignUpPerson
    {
        public int Id { get; set; }
        public int Cusno { get; set; }
        public int EpiPageId { get; set; }
        public int PayerId { get; set; }
        public String PayMethod { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Address { get; set; }
        public String Zip { get; set; }
        public String City { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public bool Canceled { get; private set; }
        public DateTime DateSaved { get; private set; }
        public Guid Code { get; set; }

        public List<SignUpPerson> Friends { get; set; }

        public SignUpPerson()
        {
            Friends = new List<SignUpPerson>();
        }

        public void AddFriend(int id, String firstname, String lastName)
        {
            SignUpPerson friend = new SignUpPerson()
            {
                Id = id,
                FirstName = firstname,
                LastName = lastName
            };
            Friends.Add(friend);
        }

        /// <summary>
        /// Get signed up person by unique code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Found person or null if not found OR if the registration has been canceled</returns>
        public static SignUpPerson GetRegisteredSignUpPerson(Guid code)
        {
            DataSet ds = MsSqlHandler.GetSignUpPerson(code);
            return GetPersonFromDataSet(ds);
        }

        /// <summary>
        /// Get signed up person by cusno
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Found person or null if not found OR if the registration has been canceled</returns>
        public static SignUpPerson GetRegisteredSignUpPerson(long cusno)
        {
            DataSet ds = MsSqlHandler.GetSignUpPerson(cusno);
            return GetPersonFromDataSet(ds);
        }

        private static SignUpPerson GetPersonFromDataSet(DataSet ds){
            // Get the person data
            //
            SignUpPerson person = null;

            if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                person = new SignUpPerson();
                DataRow dr = ds.Tables[0].Rows[0];
                person.Id = Int32.Parse(dr["Id"].ToString());
                if(dr["Cusno"] != null)
                    person.Cusno = Int32.Parse(dr["Cusno"].ToString());

                person.EpiPageId = Int32.Parse(dr["EpiPageId"].ToString());  

                person.PayerId = Int32.Parse(dr["PayerId"].ToString());
                person.PayMethod = dr["PayMethod"].ToString();
                person.FirstName = (String)dr["FirstName"];
                person.LastName = (String)dr["LastName"];
                person.Address = (String)dr["Address"];
                person.Zip = (String)dr["Zip"];
                person.City = (String)dr["City"];
                person.Phone = (String)dr["Phone"];
                person.Email = (String)dr["Email"];
                person.Canceled = Boolean.Parse(dr["Canceled"].ToString());
                person.DateSaved = (DateTime)dr["DateSaved"];
                
                if (dr["Code"] != null)
                {
                    String code = dr["Code"].ToString();
                    if (code != String.Empty)
                    {
                        person.Code = new Guid(code);
                    }
                } 

                //long cusno = long.Parse(dr["cusno"].ToString());
                //string userId = MembershipDbHandler.GetUserid(cusno);
                //SubUser = new SubscriptionUser(userId);
            }


            // See if the person has registered friends
            //
            if (person != null)
            {
                DataSet dsFriends = MsSqlHandler.GetSignUpPersonFriends(person.PayerId);
                if (dsFriends != null && dsFriends.Tables != null && dsFriends.Tables[0].Rows != null)
                {
                    foreach (DataRow dr in dsFriends.Tables[0].Rows)
                    {
                        person.AddFriend((int)dr["Id"], (String)dr["FirstName"], (String)dr["LastName"]);
                    }
                }
            }



            return person;
        }

        public static long GetCusnoFromCodeTable(Guid g)
        {
            long cusno = 0;
            try
            {
                DataSet ds = MsSqlHandler.GetSignUpPersonCust(g);
                if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    cusno = long.Parse(dr["cusno"].ToString());
                }
            }
            catch(Exception ex)
            {
                new Logger("SignUpPerson.GetCusnoFromCodeTable(code) failed for code:" + g.ToString(), ex.ToString());
            }
            return cusno;
        }

        public static int InsertSignUpPerson(long cusno, int epiPageId, int payerId, string payMethod, string firstname, string lastname, string address, string zip, string city, string phone, string email,Guid code)
        {
        
            int id = 0;
            DataSet ds = MsSqlHandler.InsertSignUpUserGuid(cusno, epiPageId, payerId, payMethod, firstname, lastname, address, zip, city, phone, email,code);
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                id = Int32.Parse(dr["Id"].ToString());
                code = (Guid)dr["code"];
            }
            return id;
        }

        public static void RemoveSignUpPerson(Guid code)
        {
            SignUpPerson person = GetRegisteredSignUpPerson(code);
            // first delete friends
            //
            foreach (SignUpPerson friend in person.Friends) 
            {
                MsSqlHandler.DeleteSignUpPerson(friend.Id);
            }

            // the delete main person
            //
            MsSqlHandler.DeleteSignUpPerson(person.Id);
        }

        public static int GetPlacesLeft(PageData pd)
        {
            int i = 500;
            if (pd["MaxParticipants"] != null)
            {
                int.TryParse(pd["MaxParticipants"].ToString(), out i);
            }

            return i - MsSqlHandler.GetSignUpNumParticipants(pd.PageLink.ID);
        }

        public static void CancelSignUpPerson(int payerId)
        {
            MsSqlHandler.CancelSignUpPerson(payerId);
        }
    }
}
