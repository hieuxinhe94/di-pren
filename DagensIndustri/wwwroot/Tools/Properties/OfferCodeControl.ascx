<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfferCodeControl.ascx.cs" Inherits="DagensIndustri.Tools.Properties.OfferCodeControl" %>

<asp:UpdatePanel runat="server" ID="updPanelOfferCodes" UpdateMode="Conditional">
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <strong>Tillgängliga:</strong><br />
                    <asp:ListBox runat="server" Width="200" SelectionMode="Single" ID="ListAvailable" Rows="10"></asp:ListBox>
                </td> 
                <td>
                    <br />
                    <asp:Button runat="server" ID="BtnAdd" OnClick="btnAddOnClick" Text=">>" /><br />
                    <asp:Button runat="server" ID="BtnRemove" OnClick="btnRemoveOnClick" Text="<<" />
                </td>   
                <td>
                    <strong>Valda:</strong><br />
                    <asp:ListBox runat="server" Width="200" SelectionMode="Single" ID="ListSelected" Rows="10"></asp:ListBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>