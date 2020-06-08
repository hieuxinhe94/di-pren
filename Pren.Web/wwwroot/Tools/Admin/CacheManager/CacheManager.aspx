<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="CacheManager.aspx.cs" Inherits="Pren.Web.Tools.Admin.CacheManager.CacheManager" %>
<%@ Import Namespace="Pren.Web.Business.Cache" %>

<asp:content ContentPlaceHolderID="FullRegion" runat="Server">
    
    <div class="epi-contentContainer epi-padding">
        
        <div class="epi-contentArea">
            <h1 class="EP-prefix">Cache Manager</h1>
    
            <p class="EP-systemInfo">Rensa cache. Endast cachenycklar med prefix "Pren.Web_" visas.</p>
            
            <asp:Button runat="server" ID="BtnRefresh" CssClass="btn btn-primary" Text="Refresh" OnClick="BtnRefreshClick"/>    

            <div style="margin-top: 20px;">
                <asp:Repeater runat="server" ID="RepCacheItems">
                    <HeaderTemplate>
                        <table>
                            <thead>
                                <tr>
                                    <th>Denna server</th>
                                    <th>Nyckel</th>
                                    <th>Gäller till</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Button runat="server" CommandArgument='<%# Eval("key")%>' Text="Rensa" OnClick="BtnRemoveClick"/>
                            <td>
                                <%# Eval("key")%>
                            </td>
                            <td>
                                <%# ((CacheKeyInfo) Eval("value")).Expires.ToLocalTime().ToString("yyyy-MM-dd HH:mm")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>

    </div>

</asp:content>