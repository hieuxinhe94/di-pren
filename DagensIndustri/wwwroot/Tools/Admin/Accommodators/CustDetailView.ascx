<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustDetailView.ascx.cs" Inherits="DagensIndustri.Tools.Admin.Accommodators.CustDetailView" %>


<asp:PlaceHolder ID="PlaceHolderCust" runat="server">
    <b>Kund</b><br />
    <asp:Label ID="LabelCustName" runat="server"></asp:Label>
    <br />
    <br />
    <b>Permanent adress</b><br />
    <asp:Label ID="LabelAddress" runat="server"></asp:Label>
    <br />
    <br />

    <b>Tillfällig adress</b><br />
    <asp:Label ID="LabelTempAddress" runat="server"></asp:Label>
    <br />
    <br />

    <b>Prenumerationsstatus</b><br />
    <asp:Label ID="LabelSubs" runat="server"></asp:Label>
    <br />
    <b>Fakturor</b><br />
    <asp:Label ID="LabelInvoices" runat="server"></asp:Label>
</asp:PlaceHolder>