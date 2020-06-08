using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.EPiJobs.SyncSubs
{
    public class Customer
    {
        public int Cusno;
        public string Email;
        public string BirthNo;
        public List<Subscription> Subs = new List<Subscription>();

        public bool IsPopulated
        {
            get
            {
                return Cusno > 0;
            }
        }

        public bool ShouldBeSaved
        {
            get
            {
                var subsShouldBeSaved = false;
                foreach (Subscription s in Subs)
                {
                    if (s.ShouldBeSaved)
                    {
                        subsShouldBeSaved = true;
                        break;
                    }
                }

                if (IsPopulated && subsShouldBeSaved)
                    return true;

                return false;
            }
        }


        public Customer(int cusno, string email, string birthNo, List<Subscription> subs)
        {
            Cusno = cusno;
            Email = email;
            BirthNo = birthNo;
            Subs = subs;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Cusno: " + Cusno + Environment.NewLine);
            sb.Append("Email: " + Email + Environment.NewLine);
            sb.Append("BirthNo: " + BirthNo + Environment.NewLine);
            sb.Append("IsPopulated: " + IsPopulated + Environment.NewLine);
            sb.Append("ShouldBeSaved: " + ShouldBeSaved + Environment.NewLine);
            foreach (var sub in Subs)
            {
                sb.Append("-----" + Environment.NewLine);
                sb.Append(sub);
            }
            return sb.ToString();
        }

    }
}