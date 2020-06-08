<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Campaign.aspx.cs" Inherits="PrenDiSe.Templates.Public.Pages.CampaignTemplates.CampaignDigital.Campaign" %>
<%@ Register TagPrefix="di" TagName="form" Src="~/Templates/Public/Units/CampaignTemplates/CampaignForm.ascx" %>
<%@ Register TagPrefix="di" TagName="header" Src="~/Templates/Public/Units/CampaignTemplates/CampaignHeader.ascx" %>
<%@ Register TagPrefix="di" TagName="papersidebar" Src="~/Templates/Public/Units/CampaignTemplates/CampaignPaperSideBar.ascx" %>
<%@ Register TagPrefix="di" TagName="googleanalytics" Src="~/Templates/Public/Units/Static/GoogleAnalytics.ascx" %>
<%@ Register src="~/Templates/Public/Units/CampaignTemplates/AddThisFieldset.ascx" tagname="AddThisFieldset" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <di:googleanalytics WriteTrackPageview="False" runat="server" />
    <di:header runat="server"></di:header>
    <asp:PlaceHolder ID="phAgendaCss" Visible="false" runat="server">
      <link href="/bootstrapDi/css/agenda.css?v=2" rel="stylesheet" media="screen" />
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phTpnCss" Visible="false" runat="server">
      <link href="/bootstrapDi/css/tpn.css?v=1" rel="stylesheet" media="screen" />
    </asp:PlaceHolder>
    <title>
        <asp:Literal ID="LitPageTitle" runat="server" />
    </title>
</head>

<body id="mainBody" runat="server">
    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    
    <div class="container">
      <div class="row">
        <div class="span12 template-top-bar">
          <div class="template-top-bar-content">
            <img src="/bootstrapDi/img/Di-small.jpg" style="float:right;" />
            <asp:Literal ID="LitPageSubTitle" runat="server" />
          </div>
        </div>
      </div>
       <!-- <div class="row template-top-bar">
            
        </div>-->

        <asp:MultiView runat="server" ID="MvSteps" ActiveViewIndex="0">            
            <asp:View ID="Step2" runat="server">
                <%--Virtual page track for GA--%>
                <script type="text/javascript">
                  TrackPageview('<%=GetGaTrackingString("")%>');
                </script>                
                <div class="row template-bg2">
                  
                    <!-- ########## PRODUCT TEXT ###################################################### -->
                    <div class="span4">
                        <div class="template-left">
                          <div class="template-margin">
                            <div class="template-centered">
                            <asp:HyperLink ID="lnkLogotype" runat="server">
                              <asp:Image ID="imgLogotype" runat="server"/>
                            </asp:HyperLink>
                            </div>
                            <EPiServer:Property ID="epiPropProductInfo" PropertyName="ProductInfo" runat="server"/>
                            <span class="hidden-phone">
                              <asp:Image ID="imgProductInfoImage" Visible="False" runat="server"/>
                            </span>
                            <span class="visible-phone">
                              <asp:Image ID="imgProductInfoImageMobile" Visible="False" runat="server"/>
                            </span>
                          </div>
                        </div>
                    </div>
                    <!-- ########## END PRODUCT TEXT ################################################## -->
                    
                    <div class="span8 template-bg1">
                      <!-- ###### OFFERS ###################################################### -->
                        <h4 class="diindent">Välj erbjudande</h4>
                        <div class="diwrap">                                                        
                                <div class="dicheckbox">  
                                    <asp:RadioButton ID="RbPrimaryCampaignStep2" onclick="<%#SetGobackAndGetTrackScript(true) %>" AutoPostBack="true" GroupName="CampaignGroup" runat="server" />                                
                                    <asp:Label runat="server" AssociatedControlID="RbPrimaryCampaignStep2">
                                        <div class="size3">
                                          <%= CurrentPage[PrimaryCampaign + "PrimHeading"]%>
                                          <span class="size5"><%= CurrentPage[PrimaryCampaign + "Price"]%></span>
                                        </div>
                                        
                                    </asp:Label>
                                </div>
                            <div class="sidebarpapers">
                                <di:papersidebar runat="server" ID="SideBarPrimary"></di:papersidebar>
                            </div>
                        </div>

                        <asp:PlaceHolder runat="server" ID="PhSecondaryArea2">
                            <div class="diwrap">
                                    <div class="dicheckbox">
                                        <asp:RadioButton ID="RbSecondaryCampaignStep2" onclick="<%#SetGobackAndGetTrackScript(false) %>" AutoPostBack="true" GroupName="CampaignGroup" runat="server" />                                         
                                        <asp:Label runat="server" AssociatedControlID="RbSecondaryCampaignStep2">
                                            <div class="size3">
                                              <%= CurrentPage[SecondaryCampaign + "SecHeading"] %>
                                              <span class="size5"><%= CurrentPage[SecondaryCampaign + "Price"]%></span>
                                            </div>
                                            
                                        </asp:Label>
                                    </div>
                                <div class="sidebarpapers">
                                    <di:papersidebar runat="server" ID="SideBarSecondary"></di:papersidebar>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                      <!-- ###### END OFFERS ################################################## -->

                      <asp:PlaceHolder ID="phForms" runat="server">
                        <div class="diwrap">
                            <di:form runat="server" ID="UcCampaignForm" ValidationGroup="form" FormHeading="Kontrollera dina uppgifter"></di:form>
                            <di:form runat="server" ID="UcCampaignFormOtherPayer" ValidationGroup="otherform" IsOtherPayerForm="true" Visible="false"  FormHeading="Ange fakturamottagarens uppgifter"></di:form>
                        
                        <EPiServer:Property runat="server" ID="PropFormEditor" CssClass="formeditor" />
                        </div>
                      </asp:PlaceHolder>

                      <asp:PlaceHolder ID="phNotLoggedInMessage" Visible="False" runat="server">
                        <div class="template-button-wrapper">
                          <asp:Button ID="btnLogin" CssClass="btn btn-large btn-success" OnClick="btnLogin_OnClick" Text="GÅ VIDARE" runat="server" />
                          <!--<div class="row">
                            
                            <div class="span5 template-box2">
                              <EPiServer:Property ID="PropLoginButtonMessage" PropertyName="LoginButtonMessage" CssClass="notloggedin-error" runat="server"/>
                            </div>
                            
                            <div class="span8">
                              
                            </div>
                            
                            <div class="span5 template-box2">
                              <EPiServer:Property ID="PropBackButtonMessage" PropertyName="BackButtonMessage" CssClass="notloggedin-error" runat="server"/>
                            </div>
                            <div class="span3 template-box1">
                              <asp:Button ID="BtnBack" CssClass="btn btn-large btn-block" OnClick="BtnBack_OnClick" runat="server" />
                            </div>

                          </div>-->
                        </div>
                      </asp:PlaceHolder>
                    
                    </div>
                </div>
            </asp:View>
            

            <asp:View ID="Step3" runat="server">
                <div class="row template-bg2">
                  <!-- ########## PRODUCT TEXT ###################################################### -->
                  <div class="span4">
                        <div class="template-centered">
                          <div class="template-margin">
                            <asp:HyperLink ID="lnkLogotypeEnd" runat="server">
                              <asp:Image ID="imgLogotypeEnd" runat="server"/>
                            </asp:HyperLink>
                            <EPiServer:Property ID="Property1" PropertyName="ProductInfo" runat="server"/>
                            <span class="hidden-phone">
                              <asp:Image ID="imgProductInfoImageEnd" Visible="False" runat="server"/>
                            </span>
                            <span class="visible-phone">
                              <asp:Image ID="imgProductInfoImageMobileEnd" Visible="False" runat="server"/>
                            </span>
                          </div>
                        </div>

                    </div>
                    <!-- ########## END PRODUCT TEXT ################################################## -->
                    
                    <div class="span8 template-bg1">      
                      <asp:PlaceHolder ID="PlaceHolderDigForm" runat="server">
                          <div class="diwrap">
                              <div class="size1"><EPiServer:Property ID="Property3" runat="server" PropertyName="ThankYouHeading" /></div>
                              <EPiServer:Property runat="server" ID="PropThankYouText" />
                              
                              <br />
                              <asp:Button ID="btnBackStep3" CssClass="btn btn-large btn-success" OnClick="BtnBack_OnClick" runat="server" />&nbsp;
                          </div>
                                        
                      </asp:PlaceHolder>
                        <br />
                        <div class="diwrap">
                            <uc1:AddThisFieldset ID="AddThisFieldset2" runat="server" />
                        </div>
                    </div>

                </div>
            </asp:View>
            
            <asp:View ID="ErrorView" runat="server">
                <div class="size3 error">
                    <asp:Label runat="server" ID="LblError" EnableViewState="false"></asp:Label>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
        
    <asp:Literal ID="LiteralAdWordsScriptOnLoad" EnableViewState="false" runat="server"></asp:Literal>
    <asp:Literal ID="LiteralAdWordsScriptOnThankYou" EnableViewState="false" runat="server"></asp:Literal>
    <asp:Literal ID="litAdditionalScript" EnableViewState="false" runat="server"></asp:Literal>
    </form>
    <asp:PlaceHolder runat="server" ID="plhMaintenanceScript" Visible='<%#ConfigurationManager.AppSettings.Get("ShowMaintenanceMessage") == "true" %>'>
        <script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/maintenanceHandler/maintenanceHandler.js")%>"></script>
        <script type="text/javascript">
            maintenanceHandler.init();
        </script>
    </asp:PlaceHolder>
</body>

</html>
