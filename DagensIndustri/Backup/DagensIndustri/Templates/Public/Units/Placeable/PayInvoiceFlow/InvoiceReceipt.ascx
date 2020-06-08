<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceReceipt.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.PayInvoiceFlow.InvoiceReceipt" %>

<p><asp:Literal ID="ReceiptLiteral" runat="server" /></p>
<div class="divider"><hr /></div>

<p>
    <a href="#" class="btn print">
        <span><EPiServer:Translate Text="/common/print" runat="server" /></span>
    </a>
</p>