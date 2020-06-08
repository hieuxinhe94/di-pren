<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiGoldSearch.aspx.cs" Inherits="DagensIndustri.Tools.Admin.DiGold.DiGoldSearch" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dagens Industri</title> 
    <link media="all" rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Tools/Admin/DiGold/Styles/reset.css")%>" />
    <link media="all" rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Tools/Admin/DiGold/Styles/shared.css")%>" />
    <meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" />
</head>
<body>
    <form runat="server">		
        <div id="content">
            <div class="digoldsearch_header">
                <a href="/Tools/Admin/DiGold/DigoldSearch.aspx">
                    <img src="Images/logo.png" />
                </a>
            </div>
            <asp:PlaceHolder ID="SearchFormPlaceHolder" runat="server">
  		    <div class="form-box">  
				<div class="section"> 
					<div class="row"> 
						<div class="col"> 
							<DI:Input ID="SocialSecurityInput" Title="Personummer" CssClass="text" Name="personalnum" TypeOfInput="SocialSecurityNumber" StripHtml="true" runat="server" />
						</div> 
					</div>				
					
					<div class="divider"><hr /></div> 
					
					<div class="row"> 
						<div class="col">
                            <DI:Input ID="FirstNameInput" Title="Förnamn" Name="firstname" CssClass="text" TypeOfInput="Text" StripHtml="true" runat="server" />
						</div> 
						<div class="col">
                             <DI:Input ID="LastNameInput" Title="Efternamn" Name="lastname" CssClass="text" TypeOfInput="Text" StripHtml="true" runat="server" />
						</div>						
					</div> 
					
					<div class="row"> 
						<div class="col">
                            <DI:Input ID="StreetInput" Title="Gata" Name="street" CssClass="text" TypeOfInput="Text" StripHtml="true" runat="server" />
						</div> 
						<div class="col"> 
							<div class="small">
                                <DI:Input ID="StreetNoInput" Title="Gatunummer" Name="number" CssClass="text" TypeOfInput="Text" StripHtml="true" runat="server" />
							</div> 
							<div class="small"> 
								 <DI:Input ID="ZipInput" Title="Postnummer" Name="zip" CssClass="text" TypeOfInput="ZipCode" StripHtml="true" runat="server" />
							</div>							
						</div>						
					</div> 
					
					<div class="row">				
						<div class="col"> 
							<DI:Input ID="TelephoneInput" Title="Telephone" Name="mobile" CssClass="text" TypeOfInput="Telephone" StripHtml="true" runat="server" />
						</div>
                        <div class="col">
                            <DI:Input ID="EmailInput" Title="E-post" Name="email" CssClass="text" TypeOfInput="Email" StripHtml="true" runat="server" />
                        </div>					
					</div>						
	
					<div class="button-wrapper">
                        <asp:Button ID="SearchButton" CssClass="" Text="Sök" OnClick="SearchButton_Click" runat="server" />
                         
					</div> 
				</div> 
			</div>
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="DiGoldFormPlaceHolder" Visible="false" runat="server">

            <div class="form-box">  
				<div class="section"> 
					<div class="row"> 
						<div class="col">
                            <DI:Input ID="DiGoldFirstnameInput" Title="Förnanmn" Name="firstname" Required="true" CssClass="text" TypeOfInput="Text" StripHtml="true" runat="server" />
                        </div>
                        <div class="col"> 
                            <DI:Input ID="DiGoldLastnameInput" Title="Efternamn" Name="lastname" Required="true" CssClass="text" TypeOfInput="Text" StripHtml="true" runat="server" />
                        </div>
                    </div>
                    <div class="row"> 
						<div class="col">
                            <DI:Input ID="DiGoldEmailInput" Title="E-post *" Name="email" Required="true" CssClass="text" TypeOfInput="Email" StripHtml="true" runat="server" />
                        </div>
                        <div class="col">
                            <DI:Input ID="DiGoldTelephoneInput" Title="Mobilnummer *" Required="true" Name="mobile" CssClass="text" TypeOfInput="Telephone" StripHtml="true" runat="server" />
                        </div>
                    </div>
                    <div class="row"> 
						<div class="col">
                            <DI:Input ID="DiGoldSocialInput" Title="Personummer *" Required="true" CssClass="text" Name="personalnum" TypeOfInput="SocialSecurityNumber" StripHtml="true" runat="server" />
                        </div>
                    </div>
                    <div class="row row-checkbox">
                        <span class="checkbox">
                            <di:Input ID="TermsAcceptedInput" TypeOfInput="CheckBox" Required="true" Name="terms" Title="<%# GetTermsAndConditions() %>" runat="server" />
	                    </span>
                    </div>
                    <div class="row row-checkbox">
                        <span class="checkbox">
                            <di:Input ID="TicketAcceptInput" TypeOfInput="CheckBox" Name="ticket" Title="Vinbiljett" runat="server" />
	                    </span>
                    </div>
                    <div class="button-wrapper">
                        <asp:Button ID="DiGoldSaveButton" Text="Spara" OnClick="DiGoldSaveButton_Click" runat="server" /> 
					</div> 

                    <div class="row"> 
                        <div class="col">
                            <asp:Literal ID="LiteralErr" Text="Alla fält måste anges med giltig information.<br>Tel ska vara ett mobilnummer<br>Personnr format YYYYMMDDXXXX<br>Var god försök igen" Visible="false" runat="server"></asp:Literal>
                        </div>
                    </div>

                </div>
                
            </div>
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="SocialSecurityResultPlaceHolder" Visible="false" runat="server">           
                <div class="result-box">
                    <div style="border-bottom:2px solid #E2DFDF;">  
                        <div class="result_wrapper">          
						    <div class="result_firstline">
                                <asp:LinkButton ID="ResultsLinkButton3" OnClick="ResultsLinkButton_Click"  runat="server">
                                    <%= SubsCustomerNumber %> - <%= SubsName%>
                                </asp:LinkButton>
                            </div>
                            <div class="result_otherlines">
                                <%= SubsAddress %> <%= SubsZipCode %>
                            </div>                                        
                            <div class="result_otherlines">
                                    <%= SubsEmail %>
                            </div>
                        </div>
                        <div class="result_image_holder">
                            <asp:PlaceHolder ID="DiWeekendPlaceHolder" Visible='<%# IsDiWeekend(Subscriber.Cusno) %>' runat="server">
                                <div class="di_weekend_holder">
                                    Di Helg
                                </div>
                            </asp:PlaceHolder>
                            <asp:Image ImageUrl="Images/wine.jpg" Visible='<%# HasWineTicket(Subscriber.Cusno)%>' runat="server" />
                            <asp:Image ImageUrl="Images/DiGold.jpg" Visible='<%# UserIsDIGoldMemberByCusNo(Subscriber.Cusno.ToString())%>' runat="server" /> 
                        </div>
                     </div>
                    <div class="clear"></div>
                </div>
            </asp:PlaceHolder>
                      
            <asp:Repeater ID="SearchResults" Visible="false" runat="server">
                <HeaderTemplate>
                    <div class="result-box"> 
                </HeaderTemplate>
                <ItemTemplate>
                    <div style="border-bottom:2px solid #E2DFDF;">
                        <div class="result_wrapper">
						    <div class="result_firstline">
                            <asp:LinkButton ID="ResultsLinkButton" OnClick="ResultsLinkButton_Click" CommandArgument='<%# string.Format("{0}|{1}", DataBinder.Eval(Container.DataItem, "CUSNO").ToString(), UserIsDIGoldMemberByCusNo(DataBinder.Eval(Container.DataItem, "CUSNO").ToString()).ToString()) %>' runat="server">
                                <%# DataBinder.Eval(Container.DataItem, "CUSNO")%> - <%# DataBinder.Eval(Container.DataItem, "ROWTEXT1")%>
                            </asp:LinkButton>
                        </div>
                            <div class="result_otherlines">
                            <%# DataBinder.Eval(Container.DataItem, "STREET1")%> <%# DataBinder.Eval(Container.DataItem, "ZIPCODE")%>
                        </div>                                        
                            <div class="result_otherlines">
                                <%# DataBinder.Eval(Container.DataItem, "EMAILADDRESS")%>
                            </div> 
                        </div>
                        <div class="result_image_holder">
                        <asp:PlaceHolder ID="DiWeekendPlaceHolder2"  Visible='<%# IsDiWeekend(long.Parse(DataBinder.Eval(Container.DataItem, "CUSNO").ToString())) %>' runat="server">
                                <div class="di_weekend_holder">
                                    Di Helg
                                </div>
                            </asp:PlaceHolder>
                        <asp:Image ImageUrl="Images/wine.jpg" Visible='<%# HasWineTicket(long.Parse(DataBinder.Eval(Container.DataItem, "CUSNO").ToString()))%>' runat="server" />    
                        <asp:Image ImageUrl="Images/DiGold.jpg" Visible='<%# UserIsDIGoldMemberByCusNo(DataBinder.Eval(Container.DataItem, "CUSNO").ToString())%>' runat="server" />                
                    </div>
                    </div>
                    <div class="clear"></div>        
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <div style="border-bottom:2px solid #E2DFDF;">
                        <div class="result_wrapper">       
						    <div class="result_firstline">
                            <asp:LinkButton ID="ResultsLinkButton2" OnClick="ResultsLinkButton_Click" CommandArgument='<%# string.Format("{0}|{1}", DataBinder.Eval(Container.DataItem, "CUSNO").ToString(), UserIsDIGoldMemberByCusNo(DataBinder.Eval(Container.DataItem, "CUSNO").ToString()).ToString()) %>' runat="server">
                                <%# DataBinder.Eval(Container.DataItem, "CUSNO")%> - <%# DataBinder.Eval(Container.DataItem, "ROWTEXT1")%>
                            </asp:LinkButton>
                        </div>
                            <div class="result_otherlines">
                            <%# DataBinder.Eval(Container.DataItem, "STREET1")%> <%# DataBinder.Eval(Container.DataItem, "ZIPCODE")%>
                        </div>                                        
                            <div class="result_otherlines">
                            <%# DataBinder.Eval(Container.DataItem, "EMAILADDRESS")%>
                        </div>
                        </div>
                        <div class="result_image_holder">
                            <asp:PlaceHolder ID="DiWeekendPlaceHolder3" Visible='<%# IsDiWeekend(long.Parse(DataBinder.Eval(Container.DataItem, "CUSNO").ToString())) %>' runat="server">
                                <div class="di_weekend_holder">
                                    Di Helg
                                </div>
                            </asp:PlaceHolder>
                            <asp:Image ID="Image4" ImageUrl="Images/wine.jpg" Visible='<%# HasWineTicket(long.Parse(DataBinder.Eval(Container.DataItem, "CUSNO").ToString()))%>' runat="server" />    
                            <asp:Image ImageUrl="Images/DiGold.jpg" Visible='<%# UserIsDIGoldMemberByCusNo(DataBinder.Eval(Container.DataItem, "CUSNO").ToString())%>' runat="server" /> 
                        </div>
                    </div>
                    <div class="clear"></div>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>

            <asp:PlaceHolder ID="TicketFormPlaceHolder" Visible="false" runat="server">
                <div class="result-box"> 
                    <div class="result_wrapper_alt">
                        <div class="result_firstline" style="text-align:center;font-size:26px;">
                            <asp:LinkButton ID="TicketLinkButton" OnClick="TicketLinkButton_Click" runat="server">
                                <strong>Klicka här för att få din vinbiljett.</strong>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>     
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="TicketPlaceHolder" Visible="false" runat="server">
                 <div class="result-box"> 
                    <div class="result_wrapper_alt">
                        <div class="result_firstline" style="text-align:center;font-size:26px;">
                            <asp:HyperLink ID="ReturnToStartHyperLink" NavigateUrl="/Tools/Admin/DiGold/DigoldSearch.aspx" runat="server">
                                <strong>Här är din vinbiljett</strong>
                            </asp:HyperLink>
                        </div>
                    </div>
                </div> 
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="HasTicketPlaceHolder" Visible="false" runat="server">
                 <div class="result-box"> 
                    <div class="result_wrapper_alt">
                        <div class="result_firstline" style="text-align:center;font-size:26px;">
                            <asp:HyperLink ID="ReturnToStartHyperLink2" NavigateUrl="/Tools/Admin/DiGold/DigoldSearch.aspx" runat="server">
                                <strong>Användaren har redan en vinbiljett</strong>
                            </asp:HyperLink>
                        </div>
                    </div>
                </div> 
            </asp:PlaceHolder>
        </div>
    </form>
</body>
</html>
