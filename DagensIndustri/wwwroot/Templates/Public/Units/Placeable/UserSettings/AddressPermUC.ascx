<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressPermUC.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.UserSettings.AddressPermUC" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register tagprefix="di" tagname="Address" src="~/Templates/Public/Units/Placeable/AddressForm/Address.ascx" %>

<style type="text/css">
    .gvStyle { text-align:left; padding-right:10px; line-height:1.5em; }
</style>

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
    </p>
    
    <asp:PlaceHolder ID="PlaceHolderFutureAdresses" runat="server">
        <h3>Framtida permanent adress</h3>
        <asp:GridView ID="gvAddresses" DataKeyNames="Id" OnSelectedIndexChanged="gvAddresses_SelectedIndexChanged" OnRowCommand="gvAddresses_OnRowCommand" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField DataField="Street1" HeaderText="Adress" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
                <asp:BoundField DataField="StartDate" HeaderText="Från och med" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
            
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

    <br />
    <di:Address ID="Address1" ShowNames="false" ShowCompany="false" ShowDate1="true" Visible="false" runat="server" />

</asp:PlaceHolder>