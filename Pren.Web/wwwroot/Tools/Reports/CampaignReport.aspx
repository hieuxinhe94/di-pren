<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="true" CodeBehind="CampaignReport.aspx.cs" Inherits="Pren.Web.Tools.Reports.CampaignReport" %>

<%@ Import Namespace="EPiServer" %>
<%@ Import Namespace="EPiServer.Core" %>
<%@ Import Namespace="Pren.Web.Tools.Reports" %>
<%@ Register TagPrefix="EPiServerUI" Namespace="EPiServer.UI.WebControls" Assembly="EPiServer.UI, Version=7.19.0.0, Culture=neutral, PublicKeyToken=8fe83dea738b45b7" %>



<asp:content ContentPlaceHolderID="FullRegion" runat="Server">
    <div class="epi-contentContainer epi-padding epi-contentArea">
        <h1 class="EP-prefix">
            Campaign report
        </h1>

        <asp:DropDownList ID="CampaignTypeSelector" runat="server" DataTextField="Description" DataValueField="ID" />

        <EPiServerUI:ToolButton Text="List campaigns" OnClick="BtnReportClick" ID="BtnReport" runat="server" />
        
        <div class="epi-marginVertical-small">
            
            <asp:GridView ID="GvReport" runat="server" AutoGenerateColumns="False" CssClass="epi-padding">
                <Columns>
                    <asp:TemplateField>                        
                        <ItemTemplate>
                           <%# Container.DataItemIndex + 1%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kampanj">                        
                        <ItemTemplate>
                            <a href="<%#((CampaignPageItem)Container.DataItem).CampaignPage.LinkURL%>" target="EPiServerMainUI"><%# ((CampaignPageItem)Container.DataItem).CampaignPage.PageName%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sidtyp">                        
                        <ItemTemplate>
                           <%# ((CampaignPageItem)Container.DataItem).CampaignPage.PageTypeName%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Målgrupp">                        
                        <ItemTemplate>
                           <%# ((CampaignPageItem)Container.DataItem).CampaignPage.TargetGroup%> (<%# ((CampaignPageItem)Container.DataItem).CampaignPage.TargetGroupMobile%>)
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kampanjer">                        
                        <ItemTemplate>
                           <%# 
                            "<h2>Kampanjperiod 1</h2><br>" +
                            string.Join("",((CampaignPageItem)Container.DataItem).CampaignBlocks
                                .Select(
                                    t => 
                                        "<div><strong>" + ((IContent)t).Name + "(" + ((IContent)t).ContentLink.ID + ")</strong></div>" + 
                                        "<div>Kort/faktura: " + t.FirstCampaignPeriod.CampaignCardAndInvoice + "</div>" +
                                        "<div>Autogiro: " + t.FirstCampaignPeriod.CampaignAutogiro + "</div><br>").ToArray()
                            )%>
                            <%# 
                            "<h2>Kampanjperiod 2</h2><br>" +
                            string.Join("",((CampaignPageItem)Container.DataItem).CampaignBlocks
                                .Select(
                                    t => 
                                        "<div><strong>" + ((IContent)t).Name + "(" + ((IContent)t).ContentLink.ID + ")</strong></div>" + 
                                        "<div>Kort/faktura: " + t.SecondCampaignPeriod.CampaignCardAndInvoice + "</div>" +
                                        "<div>Autogiro: " + t.SecondCampaignPeriod.CampaignAutogiro + "</div><br>").ToArray()
                            )%>
                            <%# 
                            "<h2>Kampanjperiod 3</h2><br>" +
                            string.Join("",((CampaignPageItem)Container.DataItem).CampaignBlocks
                                .Select(
                                    t => 
                                        "<div><strong>" + ((IContent)t).Name + "(" + ((IContent)t).ContentLink.ID + ")</strong></div>" + 
                                        "<div>Kort/faktura: " + t.ThirdCampaignPeriod.CampaignCardAndInvoice + "</div>" +
                                        "<div>Autogiro: " + t.ThirdCampaignPeriod.CampaignAutogiro + "</div><br>").ToArray()
                            )%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Senast ändrad">                        
                        <ItemTemplate>
                           <%# ((CampaignPageItem)Container.DataItem).CampaignPage.Changed.ToString("yyyy-MM-dd HH:mm")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ändrad av">                        
                        <ItemTemplate>
                           <%# ((CampaignPageItem)Container.DataItem).CampaignPage.ChangedBy%>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Script på tacksidan">                        
                        <ItemTemplate>
                            
                            <strong>Script</strong>
                            <%# Server.HtmlEncode(((CampaignPageItem)Container.DataItem).CampaignPage["ScriptThankyou"] as string)%>
                            <hr/>
                            <strong>Script in header</strong>
                            <%# Server.HtmlEncode(((CampaignPageItem)Container.DataItem).CampaignPage["ScriptThankyouInHeader"] as string)%>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <asp:TemplateField HeaderText="Genväng">                        
                        <ItemTemplate>
                           <%# (PageShortcutType)Enum.Parse(typeof(PageShortcutType), ((CampaignPageItem)Container.DataItem).CampaignPage["PageShortCutType"].ToString()) %>
                        </ItemTemplate>
                    </asp:TemplateField>   
                </Columns>
            </asp:GridView>
 
        </div>
    </div>
</asp:content>

