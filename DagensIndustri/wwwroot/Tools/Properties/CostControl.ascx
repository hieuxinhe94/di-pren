<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CostControl.ascx.cs" Inherits="DagensIndustri.Tools.Properties.CostControl" %>

<asp:UpdatePanel runat="server" ID="updPanelCosts" UpdateMode="Conditional">
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <strong>Beskrivning:</strong>
                </td>
                <td>
                    <strong>Belopp:</strong>
                </td>
            </tr>      
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="TxtDescription" Width="200"></asp:TextBox>                    
                </td> 
                <td>
                    <asp:TextBox runat="server" ID="TxtAmount" Width="50"></asp:TextBox>    
                </td>
                <td>
                    <asp:Button runat="server"  ID="BtnAdd" OnClick="btnAddOnClick" Text="Lägg till" />
                </td>   
            </tr>
            <tr>
                <td colspan="3" style="color:Red;">
                    <asp:Label runat="server" ID="LblError" EnableViewState="false"></asp:Label>
                </td>
            </tr>              
            <tr>
                <td>
                    <asp:ListBox runat="server" ID="ListCosts" Width="200" Rows="5"></asp:ListBox>                    
                </td>
                <td colspan="2">
                    <strong>Summa: </strong>
                    <asp:Label runat="server" ID="LblSum"></asp:Label><br />
                    <asp:Button runat="server" ID="BtnDelete" OnClick="btnDeleteOnClick" Text="Ta bort" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>