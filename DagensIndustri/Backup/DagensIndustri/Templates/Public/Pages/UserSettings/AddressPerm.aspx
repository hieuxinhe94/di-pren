<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddressPerm.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.AddressPerm" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="MySettingsMenu" src="~/Templates/Public/Pages/UserSettings/MySettingsMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register tagprefix="di" tagname="Address" src="~/Templates/Public/Units/Placeable/AddressForm/Address.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>


<asp:Content ContentPlaceHolderID="TopContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:MySettingsMenu ID="MySettingsMenu1" runat="server" />

    <style type="text/css">
        .gvStyle { text-align:left; padding-right:10px; line-height:1.5em; }
    </style>


    <h2><%= CurrentPage["Heading"] ?? CurrentPage.PageName %></h2>
    <di:Mainbody ID="Mainbody1" runat="server" />
    
    <%--<h2>Permanent adress</h2>
    <p>
    Ändra permanent adress vid flytt. Om du tillfälligt vill ha tidningen till en annan adress kan du göra en tillfällig adressändring (se flik 'prenumeration' i menyn).<br />
    Var god kontakta kundtjänst på tel 08-573 651 00 för att göra en permanent adressändring utanför Sverige eller om du har andra frågor.
    </p>--%>


    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:PlaceHolder ID="PlaceHolderCurrAddrAndForm" runat="server">
        <h3>Nuvarande permanent adress</h3>
        <p>
        <asp:Literal ID="LiteralName1Now" runat="server" /><asp:Literal ID="LiteralName2Now" runat="server" /><br />
        <asp:Literal ID="LiteralStreetAddrNow" runat="server" />&nbsp;
        <asp:Literal ID="LiteralStreetNumNow" runat="server" />&nbsp;
        <asp:Literal ID="LiteralEntranceNow" runat="server" />&nbsp;
        <asp:Literal ID="LiteralStarisNow" runat="server" />&nbsp;
        <br />
        <asp:Literal ID="Literal_Co_ApartmentNo_Now" runat="server" />
        <asp:Literal ID="LiteralZipNow" runat="server" />&nbsp;<asp:Literal ID="LiteralCityNow" runat="server" />
        <br />
        <%--<asp:LinkButton ID="LinkButtonEditCurrentPermAddress" runat="server" onclick="LinkButtonEditCurrentPermAddress_Click">Redigera permanent adress</asp:LinkButton>--%>
        </p>


        <asp:PlaceHolder ID="PlaceHolderFutureAdresses" runat="server">
            <h3>Framtida permanent adress</h3>
            <%--DataSource='<%#FuturePermAddresses %>'  SelectedRowStyle-BackColor="Silver"--%>
            <asp:GridView ID="gvAddresses" DataKeyNames="Id" OnSelectedIndexChanged="gvAddresses_SelectedIndexChanged" OnRowCommand="gvAddresses_OnRowCommand" AutoGenerateColumns="false" runat="server">
                <Columns>
                    <asp:BoundField DataField="Street1" HeaderText="Adress" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
                    <asp:BoundField DataField="StartDate" HeaderText="Från och med" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
                    <%--<asp:BoundField DataField="EndDate" HeaderText="Till" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />--%>
            
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnEdit" runat="server" Text="Redigera" CommandName="Select" CommandArgument='<%# Eval("Id") %>' Visible='<%#(bool)Eval("CanBeEdited") %>' CssClass="gvStyle" />
                            <asp:LinkButton ID="lbtnDelete" runat="server" Text="Radera" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' Visible='<%#(bool)Eval("CanBeDeleted") %>' CssClass="gvStyle" OnClientClick="return confirm ('Är du säker på att du vill radera adressen?')" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Literal ID="Literal1" runat="server" Text='<%#Eval("Comment") %>' Visible='<%#(bool)Eval("HasComment") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </asp:PlaceHolder>

        <p>
            <asp:LinkButton ID="LinkButtonNewPermAddress" runat="server" onclick="LinkButtonNewPermAddress_Click">Ny permanent adress</asp:LinkButton>
        </p>

        <%--<asp:Panel ID="panEdit" runat="server" Visible='<%#gvAddresses.SelectedIndex > -1 %>'>
            <h2>Redigera</h2>
            <%-<asp:TextBox ID="tbStreetAddress" runat="server" Text='<%--#SelectedAddress.Id -%>' Width="300" />-%>
        </asp:Panel>--%>

        <br />
        <di:Address ID="Address1" ShowNames="false" ShowCompany="false" ShowDate1="true" Visible="false" runat="server" />

    </asp:PlaceHolder>


</asp:Content>
