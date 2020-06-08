using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.Wine;

namespace DagensIndustri.Tools.Admin.Wine
{
    public partial class Edit : System.Web.UI.Page
    {
        private int wineId = 0;
        public bool EditMode = false;
        public String ModeString { get; set; }
        private DIClassLib.Wine.Wine _currentWine = null;
        public DIClassLib.Wine.Wine CurrentWine
        {
            get
            {
                if (_currentWine == null)
                {

                    if (Int32.TryParse(Request.QueryString["wine"], out wineId))
                    {
                        _currentWine = WineHandler.GetWine(wineId);
                    }
                }
                return _currentWine;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetErrorMessage("");
            SetSuccessMessage("");
            if (Int32.TryParse(Request.QueryString["wine"], out wineId))
            {
                EditMode = true;
                ModeString = "Redigera";
            }
            else
            {
                ModeString = "Lägg till";
            }

            if (!IsPostBack)
            {
                DataBind();
            }



            if (!IsPostBack)
                DataBind();
        }



        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            if (wineId > 0)
            {
                DIClassLib.Wine.Wine wine = CurrentWine;
                if (wine == null)
                {
                    SetErrorMessage("Ogiltigt vin.");
                    return;
                }
                tbAbout.Text = wine.About;
                tbDate.Text = wine.Date.ToString("yyyy-MM-dd");
                tbVarnummer.Text = wine.Varnummer.ToString();
            }
        }


        private void SetErrorMessage(string msg)
        {
            lbError.Text = msg;
        }
        private void SetSuccessMessage(string msg)
        {
            lbSuccess.Text = msg;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<int> characterIds = new List<int>();

            DateTime date = DateTime.MinValue;
            if (!DateTime.TryParse(tbDate.Text, out date))
            {
                SetErrorMessage("Felaktigt datum.");
                return;
            }

            int varnummer = 0;

            if (!Int32.TryParse(tbVarnummer.Text, out varnummer))
            {
                SetErrorMessage("Ange först ett giltigt varnummer");
                return;
            }

            SystembolagetArticle article = WineHandler.GetSystembolagetArticleByVarnummer(varnummer);


            if (article == null)
            {
                SetErrorMessage("Kunde inte hitta artikeln");
                return;
            }

            foreach (ListItem li in cblCharacter.Items)
            {
                if (li.Selected)
                {
                    characterIds.Add(Int32.Parse(li.Value));
                }
            }

            String longitude = tbLongitude.Text;
            String latitude = tbLatitude.Text;

            if (!EditMode)
            {
                int wineId = WineHandler.InsertWine(varnummer, tbAbout.Text, date, characterIds.ToArray(),longitude,latitude);
                Response.Redirect("WineAdmin.aspx?new=1&wine=" + wineId);
            }
            else
            {
                WineHandler.UpdateWine(wineId, varnummer, tbAbout.Text, date, characterIds.ToArray(), longitude, latitude);
                SetSuccessMessage("Vinet har uppdaterats");
            }
        }

        protected void btnAddCharacter_Click(object sender, EventArgs e)
        {
            String characterName = tbAddCharacter.Text;
            characterName = characterName.Trim();

            if (characterName == "")
            {
                SetErrorMessage("Ange karaktär");
                return;
            }

            foreach (ListItem li in cblCharacter.Items)
            {
                if (characterName.ToLower() == li.Text.ToLower())
                {
                    SetErrorMessage("Karaktären finns redan");
                    return;
                }
            }

            try
            {
                int id = WineHandler.InsertWineCharacter(tbAddCharacter.Text);
                cblCharacter.DataBind();

                foreach (ListItem li in cblCharacter.Items)
                {
                    if (id.ToString() == li.Value)
                    {
                        li.Selected = true;
                        return;
                    }
                }
                tbAddCharacter.Text = "";
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
            }

        }

        protected void btnGetArticle_Click(object sender, EventArgs e)
        {
            int varnummer = 0;

            if (!Int32.TryParse(tbVarnummer.Text, out varnummer))
            {
                SetErrorMessage("Ange först ett giltigt varnummer");
                return;
            }

            SystembolagetArticle article = WineHandler.GetSystembolagetArticleByVarnummer(varnummer);

            if (article == null)
            {
                SetErrorMessage("Kunde inte hitta artikeln");
                return;
            }

            divArticleInfo.InnerHtml = article.Namn + "<br/>";
            if (!String.IsNullOrEmpty(article.Namn2))
                divArticleInfo.InnerHtml += article.Namn2;



        }

        protected void cblCharacter_DataBound(object sender, EventArgs e)
        {
            if (CurrentWine != null && CurrentWine.CharacterIds != null)
            {

                foreach (ListItem li in cblCharacter.Items)
                {
                    int id = Int32.Parse(li.Value);
                    if (CurrentWine.CharacterIds.Contains(id))
                    {
                        li.Selected = true;
                    }
                    else
                    {
                        li.Selected = false;
                    }

                }
            }
        }
    }
}