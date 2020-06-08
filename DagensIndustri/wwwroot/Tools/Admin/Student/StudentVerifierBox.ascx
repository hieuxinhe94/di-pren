<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StudentVerifierBox.ascx.cs" Inherits="DagensIndustri.Tools.Admin.Student.StudentVerifierBox" %>
<div>

    
    <b>Verifiera student</b> (ååmmddxxxx)<br />
    <table border="0" cellpadding="0" cellspacing="3">
    <tr>
    <td><asp:TextBox ID="tbSocialSecNo" Width="100" runat="server" /></td>
    <td><asp:Button ID="btnVerifyStudent" runat="server" Text="Verifiera student" OnClick="btnVerifyStudent_Click" /></td>
    </tr>
    </table>

    

    
    <%--<asp:Label ID="lbSocialSecNo" runat="server" Text="" />--%>
    
    
</div>
<div id="divResult" runat="server" visible="false">
    <b>Resultat: </b>
    <asp:Label ID="lbResult" runat="server" />
</div>