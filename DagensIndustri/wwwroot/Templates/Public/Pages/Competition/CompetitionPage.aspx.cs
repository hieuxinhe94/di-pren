using System.Collections.Generic;
using System.Text;

using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions.AddCustAndSub;

using EPiServer.Core;
using System.Web.UI.WebControls;
using System.Data;
using DIClassLib.DbHandlers;
using System;
using DIClassLib.Subscriptions;
using EPiServer.Web.Hosting;
using System.Web.Hosting;
using System.IO;

namespace DagensIndustri.Templates.Public.Pages.Competition
{
    public partial class CompetitionPage : EPiServer.TemplatePage
    {
        #region Properties

        public string ParticipantId
        {
            get { return ViewState["ParticipantId"] as string; }
            set { ViewState["ParticipantId"] = value; }
        }

        public long CampNo
        {
            get { return (long)ViewState["CampNo"]; }
            set { ViewState["CampNo"] = value; }
        }

        public string AreaSelector
        {
            get { return ViewState["AreaSelector"] as string; }
            set { ViewState["AreaSelector"] = value; }
        }

        public string RightImageUrl { get; set; }

        #endregion

        #region Events

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                SetUpPage();
                SetUpStep1();
            }
        }

        protected void BtnSubmitClick(object sender, System.EventArgs e)
        {
            if (!Page.IsValid) return;

            var firstName = TxtFirstName.Text;
            var lastName = TxtLastName.Text;
            var email = TxtEmail.Text;
            var phone = TxtPhone.Text;
            var answers = string.Empty;
            var isCorrect = true;

            try
            {
                foreach (RepeaterItem item in RepQuestions.Items)
                {
                    var txtPageId = (TextBox)item.FindControl("TxtPageId");
                    var page = GetPage(new PageReference(int.Parse(txtPageId.Text)));

                    var question = page["Question"] as string ?? string.Empty;
                    var correctAnswer = page["CorrectAnswer"] as string ?? string.Empty;

                    //Question
                    if (!string.IsNullOrEmpty(question))
                        answers += question + ": ";

                    //Answer textbox
                    var txtAnswer = (TextBox)item.FindControl("TxtAnswer");
                    if (txtAnswer != null && !string.IsNullOrEmpty(txtAnswer.Text))
                        answers += txtAnswer.Text + Environment.NewLine;

                    //Answer radiobuttonlist
                    var rblAnswers = (RadioButtonList)item.FindControl("RblAnswers");
                    if (rblAnswers != null && !string.IsNullOrEmpty(rblAnswers.SelectedValue))
                    {
                        answers += rblAnswers.SelectedValue + Environment.NewLine;
                        if (rblAnswers.SelectedValue != correctAnswer)
                            isCorrect = false;
                    }
                }

                var readerid = RblRead.SelectedIndex;
                var receiveInfo = ChbAllowInfo.Checked;

                ParticipantId = MsSqlHandler.InsertParticipant(CurrentPage.PageLink.ID, firstName, lastName, email, phone, answers, isCorrect, readerid, false, receiveInfo);

                SetUpStep2();
            }
            catch (Exception ex)
            {
                var message = "pageId: " + CurrentPage.PageLink.ID + ", firstname: " + firstName + ", lastname: " + lastName + ", email: " + email + ", phone: " + phone + ", answers: " + answers;
                new DIClassLib.DbHelpers.Logger("Competition - BtnSubmitClick - failed for: " + message, ex.ToString());

                LblErrorStep1.Text = Translate("/common/errors/errorcustomerservice");
            }
        }

        protected void BtnSubmitPrenClick(object sender, System.EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var tg = Request.QueryString["tg"]; //dynamic target group
                //clean optional inputs (from default values)
                var co = TxtPrenCo.Text == TxtPrenCo.ToolTip ? string.Empty : TxtPrenCo.Text;
                var company = TxtPrenCompany.Text == TxtPrenCompany.ToolTip ? string.Empty : TxtPrenCompany.Text;
                var stairCase = TxtPrenStairCase.Text == TxtPrenStairCase.ToolTip ? string.Empty : TxtPrenStairCase.Text;
                var stairs = TxtPrenStairs.Text == TxtPrenStairs.ToolTip ? string.Empty : TxtPrenStairs.Text;
                var appNo = TxtPrenAppNo.Text == TxtPrenAppNo.ToolTip ? string.Empty : TxtPrenAppNo.Text;
                //create subscription and person objects
                var subscription = new Subscription(string.Empty, CampNo, PaymentMethod.TypeOfPaymentMethod.Invoice, DateTime.Now, false);
                subscription.TargetGroup = !string.IsNullOrEmpty(tg) ? tg : CurrentPage["TargetGroup"] as string;
                subscription.Subscriber = new Person(true, false, TxtPrenFirstName.Text, TxtPrenLastName.Text, co, company, TxtPrenStreetAddress.Text, TxtPrenHouseNo.Text, stairCase, stairs, appNo, TxtPrenZipCode.Text, string.Empty, TxtPrenPhone.Text, TxtPrenEmail.Text, string.Empty, string.Empty, string.Empty, string.Empty);

                var addCustAndSubHandler = new AddCustAndSubHandler();
                var addCustSubRet = addCustAndSubHandler.TryAddCustAndSub(subscription, null);
                //var err = SubscriptionController.TryInsertSubscription2(subscription);
                var returnMessages = new StringBuilder();
                if (addCustSubRet.HasMessages)
                {
                    foreach (var msg in addCustSubRet.Messages)
                    {
                        returnMessages.Append(msg.MessageCustomer);
                    }
                }
                if (string.IsNullOrEmpty(returnMessages.ToString()))
                {
                    //Success, good for you
                    MsSqlHandler.UpdateParticipant(ParticipantId, true);
                    //Hide form
                    PhPrenForm.Visible = false;
                    //Show message
                    PhPrenThankYouArea.Visible = true;
                    //Hide addthis if ThankYouArea is visible. This to avoid duplicate addthis controls
                    PrenAddThisControl.Visible = !PhThankYouArea.Visible;
                    //Thank you text
                    var msg = CurrentPage["WelcomeTextStep2"] as string;
                    LblStep2.Text = string.Format(msg, subscription.SubsStartDate.ToString("dddd dd MMMM"));
                    //Set up top image
                    SetUpTopImage(Step2, true);

                    if (IsValue("MandatoryPren"))
                        SetUpRightArea("ThankYouRightImageStep2"); //Show thankyou image in right column
                    else
                        PhRightArea.Visible = false; //Otherwise, hide right area (pren area in right area instead)
                }
                else
                {
                    //You're fucked
                    LblErrorStep2.Text = Translate("/common/errors/errorcustomerservice");
                }
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("Competition - BtnSubmitPrenClick - failed for: participantId: " + ParticipantId, ex.ToString());
                LblErrorStep2.Text = Translate("/common/errors/errorcustomerservice");
            }
        }

        #endregion

        #region Set uppers

        private void SetUpPage()
        {
            //Verify that competition is NOT out of date
            if (DateTime.Compare(DateTime.Now, DateTime.Parse(CurrentPage["CompetitionEndDate"].ToString())) > 0)
            {
                MvCompetition.SetActiveView(Off);
                return;
            }

            //Verify that correct campId is used
            var campNo = SubscriptionController.GetCampno(CurrentPage["CampId"].ToString());

            if (campNo > 0)
                CampNo = campNo;
            else
            {
                MvCompetition.SetActiveView(Off);
                LblOff.Text = "<div class='error'>" + Translate("/common/errors/errorcustomerservice") + "</div>";
                return;
            }

            SetUpTopImage(Step1, false);

            RepFooterBanners.DataSource = GetDataSource("BottomBanner", new string[] { "1", "2", "3", "4", "5" });
            RepFooterBanners.DataBind();
        }

        private void SetUpStep1()
        {
            RepQuestions.DataSource = GetChildren(CurrentPage.PageLink);
            RepQuestions.DataBind();
        }

        private void SetUpStep2()
        {
            MvSteps.SetActiveView(Step2);
            SetUpTopImage(Step2, false);

            var isPren = RblRead.SelectedIndex == 0;
            var isMandatoryPren = IsValue("MandatoryPren");
            var hideForm = IsValue("HidePrenForm");

            if (isPren || hideForm) //Already pren
            {
                PhPrenArea.Visible = false; //hide prenarea
                SetUpRightArea("ThankYouRightImageStep2"); //Show thankyou image in right column
            }
            else if (isMandatoryPren) //Not pren, and pren is mandatory
            {
                AreaSelector = "leftarea"; //Show prenarea to the left
                PhThankYouArea.Visible = false; //Hide thankyouarea
                SetUpRightArea("PrenAreaRightImageStep2"); //show pren image in right column
            }
            else
            { //No pren and pren is NOT mandatory
                AreaSelector = "rightarea"; //Show prenarea to the right
            }

            //Set already known values from first form to pren form
            if (!isPren)
            {
                TxtPrenFirstName.Text = TxtFirstName.Text;
                TxtPrenLastName.Text = TxtLastName.Text;
                TxtPrenEmail.Text = TxtEmail.Text;
                TxtPrenPhone.Text = TxtPhone.Text;
            }
        }

        private void SetUpRightArea(string propertyName)
        {
            //Show right area if there is anything to show in it
            PhRightArea.Visible = IsValue(propertyName);
            if (PhRightArea.Visible)
            {
                //Set prop for image in right column, used on code front
                RightImageUrl = CurrentPage[propertyName] as string;
            }
        }

        /// <summary>
        /// Set up the top image
        /// </summary>
        /// <param name="activeView">The active view</param>
        /// <param name="prenFormSubmitted">If user submitted prenu form</param>
        private void SetUpTopImage(View activeView, bool prenFormSubmitted)
        {
            if (activeView == Step1)
            {

                //Set up top image
                PhTopImage.Visible = CurrentPage["TopImageStep1_1"] != null;
                if (PhTopImage.Visible)
                {
                    var title = CurrentPage.PageName;
                    ImgTop.ToolTip = title;
                    ImgTop.AlternateText = title;
                    ImgTop.ImageUrl = CurrentPage["TopImageStep1_1"] as string;
                }

                //Bind repeaters
                RepShowCase.DataSource = GetDataSource("TopImageStep1_", new string[] { "1", "2", "3" });
                RepShowCase.DataBind();
            }
            else if (activeView == Step2)
            {
                var topImagePropertyName = string.Empty;

                //If already a subscriber (or just became a subscriber), check for specific top image
                if ((RblRead.SelectedIndex == 0 || prenFormSubmitted) && CurrentPage["TopImageStep2Pren_1"] != null)
                {
                    topImagePropertyName = "TopImageStep2Pren_";
                }
                //If not subscriber, check for another top image
                else if (CurrentPage["TopImageStep2_1"] != null)
                {
                    topImagePropertyName = "TopImageStep2_";
                }
                
                if (!string.IsNullOrEmpty(topImagePropertyName))
                {
                    var title = CurrentPage.PageName;
                    ImgTop.ToolTip = title;
                    ImgTop.AlternateText = title;
                    ImgTop.ImageUrl = CurrentPage[topImagePropertyName + "1"] as string; //Top image used in mobile devices instead of image slider

                    //Set up image slider
                    //For now, just one image in slider, but code is prepared for three. Just add som props and the images will slide.
                    RepShowCase.DataSource = GetDataSource(topImagePropertyName, new string[] { "1", "2", "3" });
                    RepShowCase.DataBind();
                }

                //otherwise, show the same top image/slider as in step 1
            }
        }

        #endregion

        /// <summary>
        /// Used in imageslider. To be able to set dynamic height on slider, the images need
        /// to have height and width attributes
        /// </summary>
        /// <param name="propertyName">The name of the property container the image</param>
        /// <returns>Width and height attributes in a string</returns>
        protected string GetFileSizeAttributes(string propertyName)
        {
            var filePath = CurrentPage[propertyName] as string;

            var file = System.Web.Hosting.HostingEnvironment.VirtualPathProvider.GetFile(Server.UrlDecode(filePath)) as UnifiedFile;            

            if (file == null) return string.Empty;

            var img = System.Drawing.Image.FromStream(file.Open());
            return "width='" + img.Width + "' height='" + img.Height + "'";
        }

        private List<string> GetDataSource(string propertyName, string[] indexes)
        {
            var propertiesWithValue = new List<string>();

            foreach (var index in indexes)
            {
                var propname = propertyName + index;
                if (CurrentPage[propname] != null)
                    propertiesWithValue.Add(propname);
            }

            return propertiesWithValue;
        }

    }
}