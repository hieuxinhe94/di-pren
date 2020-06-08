using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.PlugIn;
using DIClassLib.Wine;

namespace DagensIndustri.Tools.Admin.Wine
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Vinadmin", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Vinadmin", UrlFromUi = "/Tools/Admin/Wine/WineAdmin.aspx", SortIndex = 2050)]
    public partial class WineAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetSuccessMessage("");
            SetErrorMessage("");

            if (!IsPostBack)
            {
                DataBind();
                if (Request.QueryString["new"] != null && Request.QueryString["new"] == "1")
                {
                    SetSuccessMessage("Nytt vin sparat");
                }
            }
        }

        protected void SetSuccessMessage(string msg)
        {

            lbSuccess.Visible = !String.IsNullOrEmpty(msg);
            lbSuccess.Text = msg;

        }

        protected void SetErrorMessage(string msg)
        {
            lbError.Visible = !String.IsNullOrEmpty(msg);
            lbError.Text = msg;
        }


        protected void gvWines_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteWine")
            {
                int id = Int32.Parse(e.CommandArgument.ToString());
                WineHandler.DeleteWine(id);
                gvWines.DataBind();

            }
 
        }

        protected void btnDeleteCharacter_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (Int32.TryParse(ddlCharacters.SelectedValue, out id))
            {
                try{
                    WineHandler.DeleteWineCharacter(id);
                }catch(Exception ex){
                    SetErrorMessage(ex.Message);
                    return;
                }
                
                SetSuccessMessage("Karaktären har tagits bort");
                dsCharacters.DataBind();
                ddlCharacters.DataBind();
                
            }
        }

        protected void btnEditCharacter_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (Int32.TryParse(ddlCharacters.SelectedValue, out id))
            {
                WineCharacter character = WineHandler.GetWineCharacter(id);
                multiCharacters.SetActiveView(viewCharacterEdit);
                
                tbCharacterName.Text = character.Name;
            }
        }

        protected void btnUpdateCharacter_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (Int32.TryParse(ddlCharacters.SelectedValue, out id))
            {
                if (tbCharacterName.Text.Trim() == "")
                {
                    SetErrorMessage("Ange karaktär");
                    return;
                }

                try
                {
                    WineHandler.UpdateWineCharacter(id, tbCharacterName.Text);
                }
                catch (Exception ex)
                {
                    SetErrorMessage(ex.Message);
                    return;
                }
                tbCharacterName.Text = "";
                SetSuccessMessage("Karaktären har sparats");
                dsCharacters.DataBind();
                ddlCharacters.DataBind();
                multiCharacters.SetActiveView(viewCharacterDefault);
            }
        }

        protected void btnCancelEditCharacter_Click(object sender, EventArgs e)
        {
            tbCharacterName.Text = "";
            multiCharacters.SetActiveView(viewCharacterDefault);
        }
    }
}