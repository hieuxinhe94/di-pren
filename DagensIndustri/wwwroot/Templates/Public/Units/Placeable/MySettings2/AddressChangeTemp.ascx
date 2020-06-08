<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressChangeTemp.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.MySettings2.AddressChangeTemp" %>
<%@ Register tagprefix="di" tagname="Address" src="~/Templates/Public/Units/Placeable/AddressForm/Address.ascx" %>


<style type="text/css">
.gvStyle { text-align:left; padding-right:10px; line-height:1.5em; }
</style>


<h2>Tillfällig adressändring</h2>

<p>
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed id nulla molestie massa lacinia aliquet rutrum sed felis. Duis eleifend dolor non nibh sollicitudin vulputate. Sed suscipit ultrices urna, id feugiat sem eleifend ac. Ut et purus nulla.
</p>

<asp:PlaceHolder ID="PlaceHolderCurrAddrAndForm" runat="server">

    <asp:PlaceHolder ID="PlaceHolderFutureAdresses" runat="server">
        <h3>Framtida tillfälliga adresser</h3>
        <%--DataSource='<%#FuturePermAddresses %>'  SelectedRowStyle-BackColor="Silver"--%>
        <asp:GridView ID="gvAddresses" DataKeyNames="Id" OnSelectedIndexChanged="gvAddresses_SelectedIndexChanged" OnRowCommand="gvAddresses_OnRowCommand" SelectedRowStyle-BackColor="#cccccc" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField DataField="Street1" HeaderText="Adress" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
                <asp:BoundField DataField="StartDate" HeaderText="Från" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
                <asp:BoundField DataField="EndDate" HeaderText="Till" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
            
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
    <asp:LinkButton ID="LinkButtonNewAddress" runat="server" onclick="LinkButtonNewAddress_Click">Lägg in ny tillfällig adress</asp:LinkButton>
    </p>

    <br />
    <di:Address ID="Address1" ShowNames="false" ShowCompany="false" ShowDate1="true" Visible="false" runat="server" />

</asp:PlaceHolder>



