<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventArea.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Campaign.EventArea" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Templates/Public/Units/Placeable/InputWithValidation.ascx" %>

<script language="javascript" type="text/javascript">
    function ValidateChkBox(source, arguments) {
        var checkbox = document.getElementById('<%=CheckBoxIsSubscriber.ClientID%>');
        arguments.IsValid = checkbox.checked;
    }
</script>


<img alt='<%=CurrentPage["TopImageAltText"] %>' src='<%=CurrentPage["TopImageUrl"] %>' /> 
	    
<div id="MainBodyArea">

    <asp:PlaceHolder runat="server" ID="PhForm">

        <EPiServer:Property ID="Property1" runat="server" PropertyName="TopBody" />
        
	    <EPiServer:Property ID="Property2" runat="server" PropertyName="EventHeading" />
        <%--<asp:RequiredFieldValidator runat="server" ValidationGroup="form" ControlToValidate="RblOptions" ErrorMessage="Seminarium:* Obligatorisk uppgift">--%>
            <%--<img src="/Templates/DI/Images/mandatory.png" title='<= Translate("/various/validators/mandatory") >' alt='<= Translate("/various/validators/mandatory") >' />--%>
        <%--</asp:RequiredFieldValidator>--%>
        
        
        <asp:Label ID="LabelEventFull" runat="server" Text="<br /><b>Slutsålt</b>" Visible="false"></asp:Label>
        
        
        <asp:Panel ID="PanelEventForm" runat="server">
            
            <asp:RadioButtonList runat="server" ID="RblOptions"></asp:RadioButtonList>
            <br />

            <table cellpadding="5">
                <tr>
                    <td><DI:Input runat="server" ID="InputFirstName" HeadingTranslateKey="/campaigns/form/firstname" Validate="true" ValidationGroup="form" MaxLength="50" /></td>
                    <td><DI:Input runat="server" ID="InputLastName" HeadingTranslateKey="/campaigns/form/lastname" Validate="true" ValidationGroup="form" MaxLength="50" /></td>
                    <td><DI:Input runat="server" ID="InputCompany" HeadingTranslateKey="/campaigns/form/company"  MaxLength="50" /></td>                
                </tr>
                <tr>
                    <td><DI:Input runat="server" ID="InputAddress" HeadingTranslateKey="/campaigns/form/address"  MaxLength="100" /></td>
                    <td><DI:Input runat="server" ID="InputZip" HeadingTranslateKey="/campaigns/form/zip"  MaxLength="10" /></td>
                    <td><DI:Input runat="server" ID="InputCity" HeadingTranslateKey="/campaigns/form/city"  MaxLength="50" /></td>
                </tr>	        
                <tr>
                    <td><DI:Input runat="server" ID="InputPhone" HeadingTranslateKey="/campaigns/form/phone"  MaxLength="50" /></td>
                    <td><DI:Input runat="server" ID="InputEmail" HeadingTranslateKey="/campaigns/form/email" Validate="true" ValidationGroup="form" RegularExpressionValidate="true" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td colspan="3"> 
                    
                        <asp:CheckBox ID="CheckBoxIsSubscriber" Text="Jag intygar att jag prenumererar på Dagens industri*" runat="server" />
                        
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="form" ClientValidationFunction="ValidateChkBox" ErrorMessage="Endast prenumeranter kan ta del av erbjudandet" Display="Dynamic">
                            <img src="/Templates/DI/Images/mandatory.png" title='<%= Translate("/various/validators/mandatory") %>' alt='<%= Translate("/various/validators/mandatory") %>' />
                        </asp:CustomValidator>
                        <br />
                        <br />
                    
                    
                        <%--<EPiServer:Translate runat="server" Text="/campaigns/form/subscription" />--%>
                        <%--<asp:RequiredFieldValidator runat="server" ValidationGroup="form" ControlToValidate="RblSubscriber" ErrorMessage="Prenumerationsuppgifter:* Obligatorisk uppgift">--%>
                            <%--<img src="/Templates/DI/Images/mandatory.png" title='<= Translate("/various/validators/mandatory") >' alt='<= Translate("/various/validators/mandatory") >' />--%>
                        <%--</asp:RequiredFieldValidator>--%>
                        <asp:RadioButtonList runat="server" ID="RblSubscriber">
                        </asp:RadioButtonList>	                        
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <EPiServer:Translate ID="Translate1" runat="server" Text="/campaigns/form/acceptedcards" /><br />
                        <img src="/Templates/DI/Images/Subscribe/Kort.gif" alt="Vi accepterar följande kort" />
                    </td>
                </tr>	                
                <tr>
                    <td colspan="3">	                        
                        <asp:Button runat="server" ValidationGroup="form" ID="BtnSubmit" OnClick="BtnSubmitOnClick" Text="Gå vidare till betalning" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="form" />    
                    </td>
                </tr>
            </table>
        
        </asp:Panel>
    
    </asp:PlaceHolder>   
    
    <asp:Label runat="server" ID="LblMessage" EnableViewState="false"></asp:Label>
    <asp:Label runat="server" ID="LblError" CssClass="error" EnableViewState="false"></asp:Label>
    
	<EPiServer:Property ID="Property3" runat="server" PropertyName="BottomBody" />
	
</div>