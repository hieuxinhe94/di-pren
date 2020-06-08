<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CampaignForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Campaign.CampaignForm" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Templates/Public/Units/Placeable/InputWithValidation.ascx" %>


<div class="bggrey">

    <asp:PlaceHolder runat="server" ID="PhHeading">
        <h2 class="paddingT10 paddingL10"><%=FormHeading %></h2>
    </asp:PlaceHolder>

    <table class="formtable paddingT10B10">
        <asp:PlaceHolder runat="server" ID="PhAdvanced" Visible="false">    
            <tr>
                <td>
                    <DI:Input runat="server" ID="InputCompany" HeadingTranslateKey="/campaigns/campform/company" />      
                </td>        
                <td>
                    <DI:Input runat="server" ID="InputOrgNo" HeadingTranslateKey="/campaigns/campform/orgno" />      
                </td>
            </tr>
            <tr>
                <td>
                    <DI:Input runat="server" ID="InputAttention" HeadingTranslateKey="/campaigns/campform/attention" />      
                </td>            
            </tr>
        </asp:PlaceHolder>	    
        <tr>
            <td>
                <DI:Input runat="server" ID="InputFirstName" ValidationGroup="form" Validate="true" HeadingTranslateKey="/campaigns/campform/firstname" />      	        
            </td>
            <td> 
                <DI:Input runat="server" ID="InputLastName" ValidationGroup="form" Validate="true" HeadingTranslateKey="/campaigns/campform/lastname" />      
            </td>
        </tr>
    </table>    
    
    <ul id="tabs2">
	    <li>
	        <asp:LinkButton runat="server" CssClass="streetactive" ID="LbTab1" CommandArgument="tab1" OnClick="TabOnclick"><EPiServer:Translate ID="Translate1" runat="server" Text="/campaigns/tabs/street" /></asp:LinkButton>
	    </li>
	    <li>
            <asp:LinkButton runat="server" CssClass="co" ID="LbTab2" CommandArgument="tab2" OnClick="TabOnclick"><EPiServer:Translate ID="Translate2" runat="server" Text="/campaigns/tabs/co" /></asp:LinkButton>	    
	    </li>
	    <li>
            <asp:LinkButton runat="server" CssClass="box" ID="LbTab3" CommandArgument="tab3" OnClick="TabOnclick"><EPiServer:Translate ID="Translate3" runat="server" Text="/campaigns/tabs/box" /></asp:LinkButton>	    
	    </li>
    </ul>
</div>

<asp:PlaceHolder runat="server" ID="PhBirthNo" Visible="false">   
    <table class="formtable">
        <tr>
            <td>
                <DI:Input runat="server" ID="InputBirthNo" ValidationGroup="form" MaxLength="10" Validate="true" RegularExpressionValidate="true" ValidationExpression="\d{10}" HeadingTranslateKey="/campaigns/campform/birthno" />      
            </td>
        </tr>            
    </table>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="PhTab1" Visible="false">
    <table class="formtable">
        <tr>
            <td colspan="2">
                <DI:Input runat="server" ID="InputStreetName" ValidationGroup="form" Validate="true" HeadingTranslateKey="/campaigns/campform/streetname" />
            </td>
            <td>
                <DI:Input runat="server" ID="InputStreetNo" ValidationGroup="form" Validate="true" HeadingTranslateKey="/campaigns/campform/streetno" Width="50" />
            </td>
        </tr>	                            
        <tr>
            <td>
                <DI:Input runat="server" ID="InputEntrance" HeadingTranslateKey="/campaigns/campform/entrance" Width="50" /> 
            </td>
            <td>
                <DI:Input runat="server" ID="InputStairs" HeadingTranslateKey="/campaigns/campform/stairs" Width="50" />    
            </td>
            <td>
                <DI:Input runat="server" ID="InputApartment" HeadingTranslateKey="/campaigns/campform/apartment" Width="50" />
            </td>
        </tr>  
    </table>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="PhTab2" Visible="false">
    <table class="formtable">
        <tr>
            <td colspan="2">
                <DI:Input runat="server" ID="InputCo" ValidationGroup="form" Validate="true" HeadingTranslateKey="/campaigns/campform/co" /> 
            </td>
        </tr>                                    
        <tr>
            <td colspan="2">
                <DI:Input runat="server" ID="InputCoStreetName" ValidationGroup="form" Validate="true" HeadingTranslateKey="/campaigns/campform/streetname" />
            </td>
            <td>
                <DI:Input runat="server" ID="InputCoStreetNo" ValidationGroup="form" Validate="true" HeadingTranslateKey="/campaigns/campform/streetno" Width="50" />
            </td>
        </tr>	                            
        <tr>
            <td>
                <DI:Input runat="server" ID="InputCoEntrance" HeadingTranslateKey="/campaigns/campform/entrance" Width="50" /> 
            </td>
            <td>
                <DI:Input runat="server" ID="InputCoStairs" HeadingTranslateKey="/campaigns/campform/stairs" Width="50" />    
            </td>
            <td>
                <DI:Input runat="server" ID="InputCoApartment" HeadingTranslateKey="/campaigns/campform/apartment" Width="50" />
            </td>
        </tr>  
    </table>                                    
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="PhTab3" Visible="false">
    <table class="formtable">
        <tr>
            <td colspan="2">
                <DI:Input runat="server" ID="InputStopBox" ValidationGroup="form" Validate="true" HeadingTranslateKey="/campaigns/campform/stopbox" /> 
            </td>
            <td>
                <DI:Input runat="server" ID="InputStopBoxNo" ValidationGroup="form" Validate="true" HeadingTranslateKey="/campaigns/campform/stopboxno" Width="50" />    
            </td>
        </tr>                                     
    </table>
</asp:PlaceHolder>

<table class="formtable">
    <tr>
        <td>
            <DI:Input runat="server" ID="InputZip" ValidationGroup="form" Validate="true" MaxLength="5" RegularExpressionValidate="true" ValidationExpression="[0-9]+" HeadingTranslateKey="/campaigns/campform/zip" />         
        </td>
        <td>
            <DI:Input runat="server" ID="InputCity" ValidationGroup="form" Validate="true" HeadingTranslateKey="/campaigns/campform/city" /> 
        </td>
    </tr>
    <tr>
        <td>
            <DI:Input runat="server" ID="InputPhone" ValidationGroup="form" Validate="true" HeadingTranslateKey="/campaigns/campform/phone" />      
        </td>
        <td>
            <DI:Input runat="server" ID="InputEmail" ValidationGroup="form" Validate="true" RegularExpressionValidate="true" HeadingTranslateKey="/campaigns/campform/email" />      
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <EPiServer:Translate ID="Translate4" runat="server" Text="/various/mandatorytext" />
        </td>
    </tr>            
</table>