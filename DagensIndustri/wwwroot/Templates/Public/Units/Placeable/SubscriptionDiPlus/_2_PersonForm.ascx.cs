using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.DiPlus;
using System.Text;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using System.Data;


namespace DagensIndustri.Templates.Public.Units.Placeable.SubscriptionDiPlus
{
    public partial class _2_PersonForm : EPiServer.UserControlBase
    {

        #region members
        
        DagensIndustri.Templates.Public.Pages.SubscriptionDiPlus _page;
        long _cusno = 0;
        string _company;
        string _firstName;
        string _lastName;
        string _phoneMob;
        string _email;
        string _passwd;
        string _passwd2;

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            _page = (DagensIndustri.Templates.Public.Pages.SubscriptionDiPlus)Page;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (_page.PlusSub.IsNewSubs)
                PlaceHolderCusno.Visible = false;
            else
                PlaceHolderPersonFields.Visible = false;


            SetPriceInfo();
        }

        private void SetPriceInfo()
        {
            LiteralHeader.Text = (_page.PlusSub.IsNewSubs) ? "Ny prenumeration" : "Uppgradera prenumeration";
            LiteralPriceCardBig.Text = _page.SelectedCamp.PriceIncVat.ToString();
            LiteralPriceCardSmall.Text = _page.SelectedCamp.PriceExVat.ToString();
        }



        public void HandleFormVisibility(bool phCusno, bool phRegFields, bool phButCalcPrice, bool phButPayCard)
        {
            PlaceHolderCusno.Visible = phCusno;
            PlaceHolderRegularFields.Visible = phRegFields;
            PlaceHolderButtonCalcPrice.Visible = phButCalcPrice;
            PlaceHolderButtonPayCreditCard.Visible = phButPayCard;
        }



        protected void ButtonCusno_Click(object sender, EventArgs e)
        {
            long cusno = 0;
            long.TryParse(InputCusno.Text, out cusno);
            if (cusno <= 0)
            {
                LiteralErr.Text = "Kundnumret måste vara ett positivt tal";
                return;
            }

            DIClassLib.Subscriptions.Subscription subUpg = GetMostWantedUpgSub(cusno);

            string stdErr = "Ingen uppgraderingsbar prenumeration hittades för angivet kundnummer. " +
                            "Var god kontakta kundtjänst på tel 08-573 651 00 så hjälper vi dig att skapa din prenumeration.";

            //no upgradable subs found
            if (subUpg == null)
            {
                LiteralErr.Text = stdErr;
                return;
            }

            //was payer last subs (not subscriber)
            if (subUpg.Subscriber.Cusno != cusno)
            {
                LiteralErr.Text = stdErr;
                return;
            }

            PersistPlusSub(cusno, subUpg);
            InputCusno.Disabled = true;
            HandleFormVisibility(true, true, false, true);
        }

        private DIClassLib.Subscriptions.Subscription GetMostWantedUpgSub(long cusno)
        {
            List<DIClassLib.Subscriptions.Subscription> upgSubs = GetAllUpgSubs(cusno);

            if (upgSubs.Count == 0)
                return null;

            if (upgSubs.Count == 1)
                return upgSubs[0];

            //upgrade regular 6 days sub if available
            foreach (DIClassLib.Subscriptions.Subscription sub in upgSubs)
            {
                if (sub.ProductNo == Settings.ProductNo_Regular)
                    return sub;
            }

            //else upgrade weekend sub
            foreach (DIClassLib.Subscriptions.Subscription sub in upgSubs)
            {
                if (sub.ProductNo == Settings.ProductNo_Weekend && sub.PaperCode == Settings.PaperCode_DI)
                    return sub;
            }

            return upgSubs[0];
        }

        private List<DIClassLib.Subscriptions.Subscription> GetAllUpgSubs(long cusno)
        {
            List<DIClassLib.Subscriptions.Subscription> ret = new List<DIClassLib.Subscriptions.Subscription>();
            List<DIClassLib.Subscriptions.Subscription> allSubs = SubscriptionController.GetSubscriptions2(cusno);
            allSubs.Sort();

            foreach (DIClassLib.Subscriptions.Subscription sub in allSubs)
            {
                if (sub.SubsRealEndDate > DateTime.Now.Date &&
                    Settings.SubsStateActiveValues.Contains(sub.SubsState) &&
                    sub.PaperCode == Settings.PaperCode_DI &&
                    (sub.ProductNo == Settings.ProductNo_Regular || (sub.ProductNo == Settings.ProductNo_Weekend && sub.PaperCode == Settings.PaperCode_DI)) 
                    //&& sub.SubsLenMons >= 6
                    //&& sub.SubsKind == Settings.SubsKind_tillsvidare
                    )
                {
                    ret.Add(sub);
                }
            }

            return ret;
        }

        private void PersistPlusSub(long cusno, DIClassLib.Subscriptions.Subscription subUpg)
        {
            if (subUpg != null)
            {
                SetMembersFromCirix(_cusno);

                if (subUpg.ProductNo == Settings.ProductNo_Regular)
                    _page.PlusSub.SubsType = DiPlusSubscriptionType.PlusSubsType.UpgradeDi6DaySubs;

                if (subUpg.ProductNo == Settings.ProductNo_Weekend && subUpg.PaperCode == Settings.PaperCode_DI)
                    _page.PlusSub.SubsType = DiPlusSubscriptionType.PlusSubsType.UpgradeWeekendSubs;
            }

            _page.PlusSub.Cusno = cusno;
            _page.PlusSub.SubToUpgrade = subUpg;
            _page.SavePlusSubViewState();
        }

        

        
        protected void ButtonPayCreditCard_Click(object sender, EventArgs e)
        {
            SetMembers();
            
            string err = ValidateMembers();
            if (!string.IsNullOrEmpty(err))
            {
                LiteralErr.Text = err;
                return;
            }

            _page.PersonFormButtonClicked(_cusno, _company, _firstName, _lastName, _phoneMob, _email, _passwd);
        }

        
        private void SetMembers()
        {
            long.TryParse(InputCusno.Text, out _cusno);
            _company = InputCompany.Text;
            _firstName = InputFirstName.Text;
            _lastName = InputLastName.Text;
            _phoneMob = InputPhone.Text;
            _email = InputEmail.Text;
            _passwd = InputPasswd1.Text;
            _passwd2 = InputPasswd2.Text;
        }

        private void SetMembersFromCirix(long _cusno)
        {
            DataSet ds = SubscriptionController.GetCustomer(_cusno);
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                _firstName = dr["FIRSTNAME"].ToString();
                _lastName = dr["LASTNAME"].ToString();
                _phoneMob = dr["O_PHONE"].ToString();
            }
        }

        private string ValidateMembers()
        {
            StringBuilder err = new StringBuilder();

            if (InputCusno.Visible && _cusno <= 0)
                err.Append("Ange kundnummer<br>");

            if (_passwd != _passwd2)
                err.Append("Angivna lösenord är inte identiska<br>");

            if (!string.IsNullOrEmpty(err.ToString()))
                return "<p>" + err.ToString() + "</p>";

            return string.Empty;
        }

    }
}