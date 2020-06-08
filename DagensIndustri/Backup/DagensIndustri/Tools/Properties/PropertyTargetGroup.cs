using System;
using EPiServer.PlugIn;
using System.Web.UI;
using System.Web;
using System.Data;
using EPiServer.Core;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.Campaign;
using System.Collections.Generic;
using DIClassLib.Misc;


namespace DagensIndustri.Tools.Properties
{
    /// <summary>
    /// Custom PropertyData implementation
    /// </summary>
    [Serializable]
    [PageDefinitionTypePlugIn(DisplayName = "Målgrupper")]
    public class PropertyTargetGroup : PropertyString
    {
        public override IPropertyControl CreatePropertyControl()
        {
            return new PropertyTargetGroupControl();
        }

    }

    /// <summary>
    /// PropertyControl implementation used for rendering PropertyDivider data.
    /// </summary>
    public class PropertyTargetGroupControl : EPiServer.Web.PropertyControls.PropertyStringControl
    {
        ScriptManager scriptManager = null;
        DropDownList ddlRegTargGr = new DropDownList();
        DropDownList ddlEmailTargGr = new DropDownList();
        DropDownList ddlPostalTargGr = new DropDownList();
        DropDownList ddlMobileTargGr = new DropDownList();

        /// <summary>
        /// Gets the PropertyGender instance for this IPropertyControl.
        /// </summary>
        /// <value>The property that is to be displayed or edited.</value>
        //public PropertyTargetGroup PropertyTargetGroup
        //{
        //    get
        //    {
        //        return PropertyData as PropertyTargetGroup;
        //    }
        //}


        public override void ApplyChanges()
        {
            //Save changes to db

            //If pagelink doesn't exist, don't save
            if (CurrentPage.PageLink.ID > 0)
            {
                if (ddlRegTargGr.SelectedIndex > 0)
                {
                    CurrentPage.SaveCamp(ddlRegTargGr.SelectedValue, ddlEmailTargGr.SelectedValue, ddlPostalTargGr.SelectedValue, ddlMobileTargGr.SelectedValue);
                    
                    //CurrentPage.DeleteTargetGroups();
                    //CurrentPage.InsertTargetGroup(Settings.TargetGroupType_Regular, ddlRegTargGr.SelectedValue);
                    //CurrentPage.InsertTargetGroup(Settings.TargetGroupType_Email, ddlEmailTargGr.SelectedValue);
                    //CurrentPage.InsertTargetGroup(Settings.TargetGroupType_Postal, ddlPostalTargGr.SelectedValue);
                }
                else
                    AddErrorValidator("Målgrupp för vanliga besökare kan inte vara tom.");
            }
        }

        public override void CreateEditControls()
        {

            if (scriptManager == null)
            {
                //first check to see if there is a ScriptManager loaded on the page already
                scriptManager = ScriptManager.GetCurrent(this.Page);
                if (scriptManager == null)
                {
                    //since there wasn't one on the page, we create a new one and add it to the controls collection
                    scriptManager = new ScriptManager();
                    this.Controls.Add(scriptManager);
                }
            }

            //The page must be save before you can choose targetgroup
            if (CurrentPage.PageLink.ID == 0)
            {
                Label lbl = new Label();
                lbl.Text = "Du måste spara sidan innan du kan ange målgrupp";
                this.Controls.Add(lbl);
            }
            else
            {
                string selReg = null;
                string selEmail = null;
                string selPostal = null;
                string selMobile = null;
                SetSelectedDdlItems(ref selReg, ref selEmail, ref selPostal, ref selMobile);

                List<StringPair> allTargGrs = CirixDbHandler.GetAllTargetGroups();
                AddDdl(ddlRegTargGr, allTargGrs, "Målgrupp vanliga besökare", selReg);
                AddDdl(ddlEmailTargGr, allTargGrs, "Målgrupp epost (PURL)", selEmail);
                AddDdl(ddlPostalTargGr, allTargGrs, "Målgrupp brevutskick (kod)", selPostal);
                AddDdl(ddlMobileTargGr, allTargGrs, "Målgrupp mobilsida", selMobile);
            }
        }

        private void SetSelectedDdlItems(ref string selReg, ref string selEmail, ref string selPostal, ref string selMobile)
        {
            DataSet ds = MsSqlHandler.GetCampaignTargetGroups(CurrentPage.PageLink.ID);
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int id = int.Parse(dr["targetGroupTypeId"].ToString());
                    string tg = dr["targetGroup"].ToString();

                    if (id == Settings.TargetGroupType_Regular)
                        selReg = tg;

                    if (id == Settings.TargetGroupType_Email)
                        selEmail = tg;

                    if (id == Settings.TargetGroupType_Postal)
                        selPostal = tg;

                    if (id == Settings.TargetGroupType_Mobile)
                        selMobile = tg;
                }
            }
        }

        private void AddDdl(DropDownList ddl, List<StringPair> targGrs, string heading, string selectedValue)
        {
            ddl.Items.Add(new ListItem(heading, ""));
            ddl.Items.Add(new ListItem("", ""));

            bool bo = false;
            foreach (StringPair sp in targGrs)
            {
                ListItem li = new ListItem(sp.S1, sp.S2);
                if (sp.S2 == selectedValue && bo == false)
                {
                    li.Selected = true;
                    bo = true;
                }
                ddl.Items.Add(li);
            }

            this.Controls.Add(ddl);
        }


        ///// <summary>
        ///// Gets the current page
        ///// </summary>
        //public static PageData CurrentPage
        //{
        //    get { return ((PageBase)HttpContext.Current.Handler).CurrentPage; }
        //}

    }
}