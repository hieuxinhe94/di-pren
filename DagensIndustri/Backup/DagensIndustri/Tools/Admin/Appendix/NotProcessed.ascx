<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotProcessed.ascx.cs" Inherits="DagensIndustri.Tools.Admin.Appendix.NotProcessed" %>

<div class="left" style="margin:10px 0 0 50px;" >	                
    <asp:Repeater runat="server" ID="RepFiles">
        <HeaderTemplate>
            <table>   
                <tr>
                    <td colspan="3">
                        <strong>Filer i mappen NotProcessed</strong>
                    </td>
                </tr>                     
        </HeaderTemplate>
        <ItemTemplate>
            <tr class='<%# Container.ItemIndex % 2 == 0 ? "evenrow" : "oddrow" %>'>
                <td style="text-align:center;">
                    <img src="\Tools\Admin\Styles\Images\<%# ((string)Container.DataItem).Substring(((string)Container.DataItem).LastIndexOf(".") + 1,3) %>.png" />
                </td>
                <td style="padding-left:10px;">
                    <%# GetLink((string)Container.DataItem) %>
                </td>
                <td style="text-align:center;">
                    <asp:ImageButton runat="server" ID="IbDeleteFile" OnClick="IbDeleteFileOnClick" OnClientClick="javascript:return confirm('Är du säker att du vill ta bort denna fil?');"  CommandArgument="<%# (string)Container.DataItem %>" ImageUrl="/Tools/Admin/Styles/Images/Delete.png" />
                </td>
            </tr>	                        
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>   
    
    <p>
        <asp:FileUpload runat="server" ID="FileInput" /><br />
        <asp:Button runat="server" ID="BtnUpload" OnClick="BtnUploadOnClick" Text="Ladda upp fil" /><br />
        <asp:Label runat="server" ID="LbError" CssClass="error" EnableViewState="false"></asp:Label>
    </p>
    
</div>	  