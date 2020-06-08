<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminSubscriptionPrices.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Subscription.AdminSubscriptionPrices" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Prenumerationsadmin, prisintervall</title>
    <link href="/Templates/DI/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />    
    <link href="/Templates/DI/Styles/StyleCampaign.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainBodyArea">
        
        <asp:Panel ID="PanelPrices" runat="server">
        
            <div class="Header">Ordinarie erbjudanden</div>
            <div class="SpaceBottomBig">(Synliga som standard)</div>
            
            <asp:RadioButtonList ID="RadioButtonListPrices1" runat="server"></asp:RadioButtonList>
            <br />
            <div class="Indent">
                <asp:DropDownList 
                    ID="DropDownListCamp1" 
                    AutoPostBack="true"
                    runat="server" 
                    onselectedindexchanged="DropDownListCamp_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            
            <br />
            <div class="DottedDivider"></div>
            <br />
            
            <div class="Header">Övriga ordinarie erbjudanden</div>
            <div class="SpaceBottomBig">(Synliga efter klick på "visa alla")</div>
                        
            <asp:RadioButtonList ID="RadioButtonListPrices2" runat="server"></asp:RadioButtonList>
            <br />
            <div class="Indent">
                <asp:DropDownList 
                    ID="DropDownListCamp2" 
                    AutoPostBack="true"
                    runat="server" 
                    onselectedindexchanged="DropDownListCamp_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            
            <br />
            <div class="DottedDivider"></div>
            <br />
            
            <div class="Header">DI helg erbjudanden</div>
            <div class="SpaceBottomBig">(Synliga efter klick på "visa alla")</div>
            
            <asp:RadioButtonList ID="RadioButtonListPrices3" runat="server"></asp:RadioButtonList>
            <br />
            <div class="Indent">
                <asp:DropDownList 
                    ID="DropDownListCamp3" 
                    AutoPostBack="true"
                    runat="server" 
                    onselectedindexchanged="DropDownListCamp_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        
        </asp:Panel>
        
        
        
        <asp:Panel ID="PanelEdit" Visible="true" runat="server">
            
            <b>Erbjudande</b>
            <br />
            <br />
            Beskrivning<br />
            <asp:TextBox ID="TextBoxOfferTxt" Width="300" Height="35" TextMode="MultiLine" MaxLength="500" runat="server"></asp:TextBox>
            
            <br />
            <br />
            Sortering<br />
            <asp:TextBox ID="TextBoxSorting" Width="40" MaxLength="3" runat="server"></asp:TextBox>
            
            <asp:RequiredFieldValidator 
                ID="RequiredFieldValidator2" 
                runat="server" 
                ControlToValidate="TextBoxSorting" 
                ErrorMessage="Ange sortering (0-999)" 
                Display="None">
            </asp:RequiredFieldValidator>
            
            <asp:RangeValidator 
                ID="RangeValidator1" 
                runat="server" 
                Type="Integer" 
                MaximumValue="999"
                MinimumValue="0"
                ErrorMessage="Ange sortering (tal 0-999)"
                ControlToValidate="TextBoxSorting" 
                Display="None">
            </asp:RangeValidator>
            
            <br />
            <br />
            <asp:CheckBox ID="CheckBoxIsAutogiro" Text="Autogiro" runat="server" />
            <br />
            <br />
            <asp:CheckBox ID="CheckBoxIsActive" Text="Aktiv" runat="server" />
            <br />
            <br />
            <asp:HiddenField ID="HiddenFieldId" runat="server" />
            <asp:HiddenField ID="HiddenFieldCampNo" runat="server" />
            <asp:Button ID="ButtonEditSave" runat="server" Text="Spara" onclick="ButtonEditSave_Click" />
            
            <br />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            
        </asp:Panel>
        
        
    </div>
    </form>
</body>
</html>
