<%@ Page Language="C#" AutoEventWireup="False" CodeBehind="CompetitionPage.aspx.cs"
    Inherits="DagensIndustri.Templates.Public.Pages.Competition.CompetitionPage" MaintainScrollPositionOnPostback="true"  MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master"%>

<%@ Register TagPrefix="di" TagName="addthis" Src="~/Templates/Public/Pages/Competition/CompetitionAddThis.ascx" %>

<%@ Import Namespace="EPiServer.Core" %>
<%@ Import Namespace="DIClassLib.Competition" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta property="og:image" content="<%= EPiServer.Configuration.Settings.Instance.SiteUrl.ToString() + CurrentPage["FaceBookImage"] %>" />
    <meta property="og:title" content="<%= CurrentPage["FaceBookTitle"] %>" />
    <link rel="stylesheet" type="text/css" href="/Templates/Public/Styles/competition/imageslider.css"/>
    <link rel="stylesheet" type="text/css" href="/Templates/Public/Styles/competition/competition.css?v=1"  />    
    <link rel="stylesheet" type="text/css" href="/Templates/Public/Styles/competition/jquery-ui.css"  />        
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Templates/Public/js/jquery/jquery-1.8.2.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Templates/Public/js/jquery/jquery.ui.1.9.1.js") %>"></script>       
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Templates/Public/js/respond.js") %>"></script> 
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Templates/Public/js/jquery.aw-showcase.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Templates/Public/js/competitioninit.js?v=1") %>"></script>

    <%if (CurrentPage["BackgroundImage"] != null ){ %>
        <style type="text/css">
            body
            {
                background: url('<%=CurrentPage["BackgroundImage"]%>')  no-repeat scroll 50% 0;
                background-size: 100% auto;
                background-color: <%=CurrentPage["BackgroundColor"] as string ?? "#FFFFFF"%>;
            }
        </style>
    <%} %>

    <style type="text/css">
        img{max-width:100%;}
    </style>
</asp:Content>

<asp:Content  ContentPlaceHolderID="FullRegion" runat="server">

    <div id="aboutdialog" class="hidden">
        <%if (CurrentPage["AboutImage"] != null){ %>
            <img src='<%=CurrentPage["AboutImage"] %>'alt='Om tävlingen' />
        <%} %>
        <EPiServer:Property runat="server" PropertyName="AboutText" />
    </div>
    <div id="rulesdialog" class="hidden">
        <%if (CurrentPage["RulesImage"] != null){ %>
            <img src='<%=CurrentPage["RulesImage"] %>'alt='Regler' />
        <%} %>
        <EPiServer:Property runat="server" PropertyName="RulesText" />
    </div>
    <div id="progress" class="hidden">        
        <img id="progressimg" src="../../Styles/competition/images/progress.gif" alt="Skickar ..." style="float:left;" />
        <span style="margin-left:20px;margin-top:15px;float:left;">Skickar ...</span>
    </div>	
    <div id="validform" class="hidden">
        <asp:ValidationSummary runat="server" CssClass="valsummary" ValidationGroup="competition" />
        <div class="btn" onclick="$('#validform').dialog('close');"><span>OK</span></div>
    </div>
    <div id="validpren" class="hidden">
        <asp:ValidationSummary runat="server" CssClass="valsummary" ValidationGroup="pren" />
        <div class="btn" onclick="$('#validpren').dialog('close');"><span>OK</span></div>
    </div>         

    <div id="compwrapper" class="clearfix">
        
        <asp:MultiView ID="MvCompetition" runat="server" ActiveViewIndex="0">
            <asp:View id="On" runat="server">

                <div id="functions">
                    <img src="/templates/public/images/logo.png" alt="Dagens industri" />                       
                    <ul>
                        <li class="about">Om tävlingen</li>
                        <li class="rules">Regler</li>
                    </ul>                 
                </div>

                <asp:PlaceHolder runat="server" ID="PhTopImage">
                    <div id="imagearea">
                        <div id="topimage">
                            <asp:Image runat="server" ID="ImgTop" CssClass="toppen" />                
                        </div>
                        <asp:Repeater runat="server" ID="RepShowCase">
                            <HeaderTemplate><div id="showcase" class="showcase showcase-load"></HeaderTemplate>
                            <ItemTemplate>
                                <div class="showcase-slide">
			                        <div class="showcase-content">
				                        <img src='<%# CurrentPage[(string)Container.DataItem] %>' alt="" <%#GetFileSizeAttributes((string)Container.DataItem) %> />
			                        </div>
		                        </div>
                            </ItemTemplate>
                            <FooterTemplate></div></FooterTemplate>
                        </asp:Repeater>	                    
                    </div>
                </asp:PlaceHolder>        

                <asp:MultiView id="MvSteps" runat="server" ActiveViewIndex="0">
                    <asp:View id="Step1" runat="server">
                        <div id="leftarea">
                            <div class="header"><EPiServer:Property runat="server" PropertyName="HeaderLeftStep1" /></div>                            
                            <div class="questionwrapper">                        
                                <span class="inputfield">
                                    Hur läser du Dagens industri idag?
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="competition" ControlToValidate="RblRead" ErrorMessage="Du måste ange hur du läser Di" CssClass="val">
                                        <span class="status">&nbsp;</span>
                                    </asp:RequiredFieldValidator>
                                    <asp:RadioButtonList ID="RblRead" runat="server">
                                        <asp:ListItem Text="Jag har en prenumeration hem"></asp:ListItem>
                                        <asp:ListItem Text="När jag kommer över den"></asp:ListItem>
                                        <asp:ListItem Text="Inte alls"></asp:ListItem>
                                    </asp:RadioButtonList>  
                                </span>                      
                            </div> 
                            <div class="inputwrapper">
                                <span class="inputfield">
                                    <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" ID="TxtFirstName" Text="Förnamn" ToolTip="Förnamn"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="competition" ControlToValidate="TxtFirstName" ErrorMessage="Du måste ange förnamn" InitialValue="Förnamn" CssClass="val">
                                        <span class="status">&nbsp;</span>
                                    </asp:RequiredFieldValidator>
                                </span>
                            </div>
                            <div class="inputwrapper">
                                <span class="inputfield">
                                    <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" ID="TxtLastName" Text="Efternamn" ToolTip="Efternamn"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="competition" ControlToValidate="TxtLastName" ErrorMessage="Du måste ange efternamn" InitialValue="Efternamn" CssClass="val">
                                        <span class="status">&nbsp;</span>
                                    </asp:RequiredFieldValidator>
                                </span>
                            </div>
                            <div class="inputwrapper">
                                <span class="inputfield">
                                    <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" ID="TxtEmail" Text="E-postadress" ToolTip="E-postadress"></asp:TextBox>
                                    <asp:RegularExpressionValidator runat="server" ValidationGroup="competition" ControlToValidate="TxtEmail" ErrorMessage="Du måste ange en korrekt e-postadress" CssClass="val" ValidationExpression="^([_A-Za-z0-9-])+(\.[_A-Za-z0-9-]+)*@(([A-Za-z0-9]){1,}([-.])?([A-Za-z0-9]){1,})+((\.)?([A-Za-z0-9]){1,}([-])?([A-Za-z0-9]){1,})*(\.[A-Za-z]{2,4})$">
                                        <span class="status">&nbsp;</span>
                                    </asp:RegularExpressionValidator>
                                </span>
                            </div>
                            <div class="inputwrapper">
                                <span class="inputfield">
                                    <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" ID="TxtPhone" Text="Mobilnummer" ToolTip="Mobilnummer"></asp:TextBox>   
                                    <asp:RegularExpressionValidator runat="server" ValidationGroup="competition" ControlToValidate="TxtPhone" ErrorMessage="Du måste ange ett korrekt mobilnummer" CssClass="val" ValidationExpression="(^([0-9-\s\+]){5,}$)">
                                        <span class="status">&nbsp;</span>
                                    </asp:RegularExpressionValidator>                                      
                                </span>
                            </div>
                            <asp:CheckBox runat="server" Visible="false" CssClass="block" ID="ChbAllowInfo" Text="Ja, jag vill ta emot information från Dagens industri" Checked="true" />            
                        </div>
                        <div id="rightarea">
                            <div class="header"><EPiServer:Property runat="server" PropertyName="HeaderRightStep1" /></div>
                            <asp:Repeater runat="server" ID="RepQuestions">
                                <ItemTemplate>
                                    <div class="questionwrapper">                                            
                                        <asp:TextBox runat="server" ID="TxtPageId" Text="<%# ((PageData)Container.DataItem).PageLink.ID%>" CssClass="hidden"></asp:TextBox>                             
                                        <strong><EPiServer:Property runat="server" PropertyName="Question" /></strong>
                                        <asp:PlaceHolder runat="server" Visible='<%# ((PageData)Container.DataItem) ["Answers"] != null %>'>
                                            <asp:RadioButtonList runat="server" ID="RblAnswers" RepeatDirection="Horizontal" DataSource="<%# CompetitionUtility.GetAnswers(((PageData)Container.DataItem) ) %>" ></asp:RadioButtonList>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="RblAnswers" ValidationGroup="competition" ErrorMessage='<%# ((PageData)Container.DataItem) ["Question"] %>' Display="None">&nbsp;</asp:RequiredFieldValidator>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" Visible='<%# ((PageData)Container.DataItem) ["Answers"] == null %>'>
                                            <asp:TextBox runat="server" ID="TxtAnswer"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAnswer" ValidationGroup="competition" ErrorMessage='<%# ((PageData)Container.DataItem) ["Question"] %>' Display="None">&nbsp;</asp:RequiredFieldValidator>
                                        </asp:PlaceHolder>                                                                     
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div class="text small">
                                <EPiServer:Property runat="server" PropertyName="InfoTextRightStep1" />
                            </div>

                            <%--<div onclick="javascript:showProgress('#validform');">test</div>--%>
                            <asp:LinkButton runat="server" CssClass="submitbtn btn" OnClick="BtnSubmitClick" OnClientClick="javascript:showProgress('#validform');" ValidationGroup="competition"><span>Skicka</span></asp:LinkButton>
                            <p class="required"> obligatoriska uppgifter</p>
                            <asp:Label runat="server" ID="LblErrorStep1" CssClass="error block" EnableViewState="false"></asp:Label>
                        </div>
                    </asp:View>

                    <asp:View id="Step2" runat="server">
                        <asp:PlaceHolder runat="server" ID="PhThankYouArea">
                            <div id="leftarea">                
                                <div class="header"><EPiServer:Property runat="server" PropertyName="ThankYouMessageHeaderStep2" /></div>
                                <div class="text">
                                    <EPiServer:Property runat="server" PropertyName="ThankYouMessageStep2" />
                                </div>
                                <di:addthis runat="server"></di:addthis>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="PhPrenArea">
                            <div id="<%=AreaSelector %>">
                                <asp:PlaceHolder runat="server" ID="PhPrenForm">
                                    <div id="prenform">
                                        <div class="header small checkred"><EPiServer:Property runat="server" PropertyName="HeaderRightStep2" /></div>  
                                        <div class="inputwrapper">
                                            <span class="inputfield">
                                                <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input50"  ID="TxtPrenFirstName" Text="Förnamn" ToolTip="Förnamn"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ValidationGroup="pren" ControlToValidate="TxtPrenFirstName" ErrorMessage="Du måste ange förnamn" InitialValue="Förnamn" CssClass="val">
                                                    <span class="status">&nbsp;</span>
                                                </asp:RequiredFieldValidator>
                                            </span>
                                            <span class="inputfield">
                                                <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input50"  ID="TxtPrenLastName" Text="Efternamn" ToolTip="Efternamn"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ValidationGroup="pren" ControlToValidate="TxtPrenLastName" ErrorMessage="Du måste ange efternamn" InitialValue="Efternamn" CssClass="val">
                                                    <span class="status">&nbsp;</span>
                                                </asp:RequiredFieldValidator>
                                            </span>
                                        </div>
                                        <div class="inputwrapper">
                                            <span class="inputfield optional">
                                                <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input50"  ID="TxtPrenCo" Text="C/O" ToolTip="C/O"></asp:TextBox>
                                            </span>
                                            <span class="inputfield optional">
                                                <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input50" ID="TxtPrenCompany" Text="Företag" ToolTip="Företag"></asp:TextBox>
                                            </span>
                                        </div>
                                        <div class="inputwrapper">
                                            <span class="inputfield">
                                                <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input50" ID="TxtPrenStreetAddress" Text="Gatuadress" ToolTip="Gatuadress"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ValidationGroup="pren" ControlToValidate="TxtPrenStreetAddress" ErrorMessage="Du måste ange gatuadress" InitialValue="Gatuadress" CssClass="val">
                                                    <span class="status">&nbsp;</span>
                                                </asp:RequiredFieldValidator>
                                            </span>
                                            <span class="inputfield">
                                                <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input25" ID="TxtPrenHouseNo" Text="Nr" ToolTip="Nr"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ValidationGroup="pren" ControlToValidate="TxtPrenHouseNo" ErrorMessage="Du måste ange nr" InitialValue="Nr" CssClass="val">
                                                    <span class="status">&nbsp;</span>
                                                </asp:RequiredFieldValidator>
                                            </span>
                                        </div>
                                        <div class="inputwrapper">
                                            <span class="inputfield optional">
                                                <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input25" ID="TxtPrenStairCase" Text="Uppg" ToolTip="Uppg"></asp:TextBox>
                                            </span>
                                            <span class="inputfield optional">
                                                <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input25 nomargin" ID="TxtPrenStairs" Text="Tr" ToolTip="Tr"></asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" ValidationGroup="pren" ControlToValidate="TxtPrenStairs" ErrorMessage="Du har angett för många tecken i Tr" CssClass="val" ValidationExpression="^.{0,3}$">
                                                    <span class="status">&nbsp;</span>
                                                </asp:RegularExpressionValidator>
                                            </span>
                                            <span class="inputfield optional">
                                                <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input25" ID="TxtPrenAppNo" Text="Lgh nr" ToolTip="Lgh nr"></asp:TextBox>
                                            </span>
                                        </div>
                                        <div class="inputwrapper">
                                            <span class="inputfield">
                                                <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input50" ID="TxtPrenZipCode" Text="Postnr" ToolTip="Postnr" MaxLength="5"></asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" ValidationGroup="pren" ControlToValidate="TxtPrenZipCode" ErrorMessage="Du måste ange postnummer (xxxxxx)" CssClass="val" ValidationExpression="[0-9]+">
                                                    <span class="status">&nbsp;</span>
                                                </asp:RegularExpressionValidator>
                                            </span>
                                            <span class="inputfield">
                                                <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input50" ID="TxtPrenEmail" Text="E-postadress" ToolTip="E-postadress"></asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" ValidationGroup="pren" ControlToValidate="TxtPrenEmail" ErrorMessage="Du måste ange en korrekt e-postadress" CssClass="val" ValidationExpression="^([_A-Za-z0-9-])+(\.[_A-Za-z0-9-]+)*@(([A-Za-z0-9]){1,}([-.])?([A-Za-z0-9]){1,})+((\.)?([A-Za-z0-9]){1,}([-])?([A-Za-z0-9]){1,})*(\.[A-Za-z]{2,4})$">
                                                    <span class="status">&nbsp;</span>
                                                </asp:RegularExpressionValidator>
                                            </span>
                                        </div>
                                
                                        <span class="inputwrapper inputfield">
                                            <asp:TextBox runat="server" onfocus="check(this);" onblur="check(this);" CssClass="input50" ID="TxtPrenPhone" Text="Mobilnummer" ToolTip="Mobilnummer"></asp:TextBox>
                                            <asp:RegularExpressionValidator runat="server" ValidationGroup="pren" ControlToValidate="TxtPrenPhone" ErrorMessage="Du måste ange ett korrekt mobilnummer" CssClass="val" ValidationExpression="(^([0-9-\s\+]){5,}$)">
                                                <span class="status">&nbsp;</span>
                                            </asp:RegularExpressionValidator>
                                        </span>
                                
                                        <div class="text small">
                                            <EPiServer:Property runat="server" PropertyName="InfoTextRightStep2" />
                                        </div>
                                        <asp:LinkButton runat="server" CssClass="submitbtn btn" OnClick="BtnSubmitPrenClick" OnClientClick="javascript:showProgress('#validpren');" ValidationGroup="pren"><span>Skicka</span></asp:LinkButton>
                                        <p class="required"> obligatoriska uppgifter</p>
                                    </div>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder runat="server" ID="PhPrenThankYouArea" Visible="false">
                                    <div class="header"><EPiServer:Property runat="server" PropertyName="WelcomeTextHeaderStep2" /></div>
                                    <div class="text">
                                        <asp:Label runat="server" ID="LblStep2" CssClass="block" EnableViewState="false"></asp:Label>
                                    </div>
                                    <di:addthis runat="server" ID="PrenAddThisControl"></di:addthis>
                                </asp:PlaceHolder>                              
                                <asp:Label runat="server" ID="LblErrorStep2" CssClass="error block" EnableViewState="false"></asp:Label>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="PhRightArea" Visible="false">
                            <div id="rightarea">                                                                        
                                <img src='<%= RightImageUrl %>' alt='' />
                            </div>                            
                        </asp:PlaceHolder>
                    </asp:View>
                </asp:MultiView>

                <div id="footerbanners">
                    <asp:Repeater runat="server" ID="RepFooterBanners" >
                        <ItemTemplate>
                            <img src='<%#CurrentPage[(string)Container.DataItem] %>' title='<%#CurrentPage[(string)Container.DataItem + "Title"] %>' alt='<%#CurrentPage[(string)Container.DataItem + "Title"] %>' />
                        </ItemTemplate>
                    </asp:Repeater>  
                </div>
      
            </asp:View>
            <asp:View id="Off" runat="server">
                <div id="off">
                    <img src="/templates/public/images/logo.png" alt="Dagens industri" />       
                    
                    <asp:Label runat="server" ID="LblOff" >
                        <div class="header" style="margin-top:20px;">Tävlingen är avslutad</div>     
                        <EPiServer:Property runat="server" PropertyName="ClosedText" />
                    </asp:Label>             
                </div>
            </asp:View>
        </asp:MultiView>
        
    </div>
    
    
</asp:Content>
