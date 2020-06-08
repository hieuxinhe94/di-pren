﻿<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddressTemp.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.AddressTemp" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="MySettingsMenu" src="~/Templates/Public/Pages/UserSettings/MySettingsMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register tagprefix="di" tagname="Address" src="~/Templates/Public/Units/Placeable/AddressForm/Address.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>


<asp:Content ContentPlaceHolderID="TopContentPlaceHolder" runat="server" ></asp:Content>
<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:MySettingsMenu ID="MySettingsMenu1" runat="server" />

    <style type="text/css">
        .gvStyle { text-align:left; padding-right:10px; line-height:1.5em; }
    </style>


    <h2><%= CurrentPage["Heading"] ?? CurrentPage.PageName %></h2>
    <di:Mainbody ID="Mainbody1" runat="server" />

    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:PlaceHolder ID="PlaceHolderCurrAddrAndForm" runat="server">

        <asp:PlaceHolder ID="PlaceHolderFutureAdresses" runat="server">
            <h3>Framtida tillfälliga adresser</h3>
            <%--DataSource='<%#FuturePermAddresses %>'  SelectedRowStyle-BackColor="Silver"--%>
            <asp:GridView ID="gvAddresses" DataKeyNames="Id" OnSelectedIndexChanged="gvAddresses_SelectedIndexChanged" OnRowCommand="gvAddresses_OnRowCommand" SelectedRowStyle-BackColor="#cccccc" AutoGenerateColumns="false" runat="server">
                <Columns>
                    <asp:BoundField DataField="Street1" HeaderText="Adress" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
                    <asp:BoundField DataField="StartDate" HeaderText="Från och med" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
                    <asp:BoundField DataField="EndDate" HeaderText="Till och med" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
            
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%--<asp:LinkButton ID="lbtnEdit" runat="server" Text="Redigera" CommandName="Select" CommandArgument='<%# Eval("Id") %>' Visible='<%#(bool)Eval("CanBeEdited") %>' CssClass="gvStyle" />--%>
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
            <br />
        </asp:PlaceHolder>

        <h3>Ny tillfällig adressändring</h3>
        
        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
        <td>
            <asp:DropDownList ID="DropDownListAddresses" OnSelectedIndexChanged="DropDownListAddresses_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
        </td>
        <td width="10"></td>
        <td>eller</td>
        <td width="10"></td>
        <td>
            <asp:LinkButton ID="LinkButtonNewAddress" runat="server" Text="Skapa ny adress" onclick="LinkButtonNewAddress_Click" />
        </td>
        </tr>
        </table>
            
        <br />
        <br />
        <di:Address ID="Address1" ShowNames="false" ShowCompany="false" ShowDate1="true" ShowDate2="true" Visible="false" runat="server" />

    </asp:PlaceHolder>


</asp:Content>
