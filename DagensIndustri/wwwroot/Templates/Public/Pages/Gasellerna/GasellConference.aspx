<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellConference.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellConference" %>
<%@ Register TagPrefix="DI" TagName="GasellRoot" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellRoot.ascx" %>
<%@ Register TagPrefix="DI" TagName="MainBody" Src="~/Templates/Public/Units/Placeable/OldMainBody.ascx" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Templates/Public/Units/Placeable/InputWithValidation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <div id="ContainerMain">    
        <div id="MainContentArea">            
            <div id="MainBodyArea">
                <DI:MainBody runat="server" /> 
                
                <asp:PlaceHolder runat="server" ID="PhMessage" Visible="false">
                    <div class="paddingB10">
                        <asp:Label runat="server" ID="LblMessage" CssClass="EjBrodText"></asp:Label><br />
                        <asp:Label runat="server" ID="LblMailText" CssClass="EjBrodText"></asp:Label>    
                    </div>
                </asp:PlaceHolder>
                                               
                <asp:PlaceHolder runat="server" ID="PhSignUp" Visible="false">
                     			                                 
                    <div class="paddingB10"><asp:Label runat="server" ID="LblConfText"></asp:Label></div>
                                        
                    <table class="TblGForm EjBrodText paddingB10">
                        <tr>
                            <td>
                                <DI:Input runat="server" ID="InputFirstName" HeadingTranslateKey="/gasellerna/conference/form/firstname" Validate="true" ValidationGroup="form" MaxLength="150" />                                
                            </td>
                            <td>
                                <DI:Input runat="server" ID="InputLastName" HeadingTranslateKey="/gasellerna/conference/form/lastname" Validate="true" ValidationGroup="form" MaxLength="150" />
                            </td>
                            <td>
                                <DI:Input runat="server" ID="InputTitle" HeadingTranslateKey="/gasellerna/conference/form/title" Validate="true" ValidationGroup="form" MaxLength="150" />
                            </td>                              
                        </tr>
                        <tr>                      
                            <td colspan="3">
                                <DI:Input runat="server" ID="InputCompany" HeadingTranslateKey="/gasellerna/conference/form/company" Width="215" Validate="true" ValidationGroup="form" MaxLength="150" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <DI:Input runat="server" ID="InputCoAddress" Width="215" HeadingTranslateKey="/gasellerna/conference/form/coaddress" MaxLength="150" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <DI:Input runat="server" ID="InputAddress" HeadingTranslateKey="/gasellerna/conference/form/address" Width="215" Validate="true" ValidationGroup="form" MaxLength="150" />
                            </td>
                            <td>
                                <DI:Input runat="server" ID="InputStairs" HeadingTranslateKey="/gasellerna/conference/form/stairs" MaxLength="5" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <DI:Input runat="server" ID="InputZipCode" Validate="true" ValidationGroup="form" HeadingTranslateKey="/gasellerna/conference/form/zipcode" MaxLength="10" />
                            </td>
                            <td colspan="2">
                                <DI:Input runat="server" ID="InputCity" Validate="true" ValidationGroup="form" HeadingTranslateKey="/gasellerna/conference/form/city" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <DI:Input runat="server" ID="InputPhone" HeadingTranslateKey="/gasellerna/conference/form/phone" Width="215" Validate="true" ValidationGroup="form" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <DI:Input runat="server" ID="InputEmail" HeadingTranslateKey="/gasellerna/conference/form/email" Width="215" Validate="true" RegularExpressionValidate="true" ValidationGroup="form" MaxLength="150" />                                
                            </td>
                        </tr>                         
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="RblGasellCompany"><EPiServer:Translate ID="Translate1" runat="server" Text="/gasellerna/conference/form/gasellcompany" /></asp:Label><br />
                                <asp:RadioButtonList runat="server" ID="RblGasellCompany" CssClass="radio" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="yes" Selected="True">Ja</asp:ListItem>
                                    <asp:ListItem Value="no">Nej</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <DI:Input runat="server" ID="InputIndustry" HeadingTranslateKey="/gasellerna/conference/form/industry" Width="215" ValidationGroup="form" MaxLength="150" />                            
                            </td>
                            <td>
                                <DI:Input runat="server" ID="InputEmployees" HeadingTranslateKey="/gasellerna/conference/form/employees" ValidationGroup="form" MaxLength="150" />
                            </td>                            
                        </tr>                        
                        <tr>
                            <td colspan="3">
                                <DI:Input runat="server" ID="InputCode" HeadingTranslateKey="/gasellerna/conference/form/code" MaxLength="10" />
                            </td>
                        </tr>
                        <tr class="bold">
                            <td colspan="2">
                                <DI:Input runat="server" ID="InputInvoiceAddress" Width="215" HeadingTranslateKey="/gasellerna/conference/form/invoiceaddress" MaxLength="150" />
                            </td>      
                            <td>
                                <DI:Input runat="server" ID="InputInvoiceReference" HeadingTranslateKey="/gasellerna/conference/form/invoicereference" MaxLength="150" />
                            </td>
                            <td>
                                <DI:Input runat="server" ID="InputCorpIdNumber" HeadingTranslateKey="/gasellerna/conference/form/corpidnumber" MaxLength="50" />
                            </td>            
                        </tr>
                        <tr class="bold">
                            <td>
                                <DI:Input runat="server" ID="InputInvoiceZipCode" HeadingTranslateKey="/gasellerna/conference/form/invoicezipcode" MaxLength="10" />
                            </td>                        
                            <td>
                                <DI:Input runat="server" ID="InputInvoiceCity" HeadingTranslateKey="/gasellerna/conference/form/invoicecity" MaxLength="50" />
                            </td>                                                     
                        </tr>
                        <asp:PlaceHolder runat="server" ID="PhFreePren">
                            <tr>
                                <td colspan="3">
                                    <div class="freePren">
					                    <EPiServer:Property ID="Property1" runat="server" PropertyName="FreePrenText" />
                                        <asp:RadioButtonList runat="server" ID="RblSubscribe" CssClass="radio" RepeatDirection="Vertical">
                                            <asp:ListItem Value="yes" Selected="True">Ja tack</asp:ListItem>
                                            <asp:ListItem Value="no">Nej tack</asp:ListItem>
                                        </asp:RadioButtonList>				                    
				                        <a onclick="javascript:document.getElementById('Condition').style.display = 'block';" href="javascript:void(0);">Villkor</a>
				                        <div id="Condition" style="display:none;">
                                            <EPiServer:Property ID="Property2" runat="server" PropertyName="FreePrenCondition" />
                                            <a onclick="javascript:document.getElementById('Condition').style.display = 'none';" href="javascript:void(0);">[X] Stäng</a>								
				                        </div>                                
                                    </div>                            
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td colspan="3">
                                <asp:Button runat="server" ID="BtnSubmit" ValidationGroup="form" OnClick="OrderConferences" Text="Boka" />  
                                <asp:ValidationSummary runat="server" ID="ValSumForm" ValidationGroup="form" />                                                  
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <EPiServer:Translate ID="Translate2" runat="server" Text="/various/mandatorytext" />
                            </td>
                        </tr>
                    </table>                    		            
                </asp:PlaceHolder>
                
                <asp:Repeater runat="server" ID="GasellConfRepeater">
                    <HeaderTemplate>
	                    <table Cellspacing="0" Cellpadding="0" class="gasellConference EjBrodText redArr">
	                        <tr>
	                            <td class="heading" colspan="3">	                                
	                                <EPiServer:Translate ID="Translate3" runat="server" Text="/gasellerna/conference/listheading" />
	                            </td>
	                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
	                    <tr class='<%# Container.ItemIndex % 2 == 0 ? "evenrow" : "oddrow" %>'>
		                    <td>					                					                				                
		                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="ShowForm" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>'>
		                            <%# DataBinder.Eval(Container.DataItem, "city") %>
		                        </asp:LinkButton>
		                    </td>
		                    <td>
			                    <%# getDate((DateTime)DataBinder.Eval(Container.DataItem, "startDate")) %>
		                    </td>
		                    <td>					                
			                    <a href="javascript:sendGasellLinkMail(<%# DataBinder.Eval(Container.DataItem, "id")%>,<%= CurrentPage.PageLink %>);">Tipsa</a>
		                    </td>
	                    </tr>
                    </ItemTemplate>				
                    <FooterTemplate>
	                    </table>
                    </FooterTemplate>                 
                </asp:Repeater>
            </div>         
        </div>                                      
    </div>    
    
    <div id="ContainerRight">
        <DI:GasellRoot runat="server" />
    </div>
</asp:Content>

