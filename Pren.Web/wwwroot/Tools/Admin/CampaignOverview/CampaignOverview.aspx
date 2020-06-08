<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="CampaignOverview.aspx.cs" Inherits="Pren.Web.Tools.Admin.CampaignOverview.CampaignOverview" %>

<%@ Import Namespace="Pren.Web.Tools.Admin.CampaignOverview" %>
<%@ Import Namespace="EPiServer.Core" %>
<%@ Import Namespace="Pren.Web.Models.Pages" %>
<%@ Import Namespace="EPiServer.ServiceLocation" %>
<%@ Import Namespace="EPiServer" %>

<asp:content contentplaceholderid="FullRegion" runat="Server">
       
    <link href="<%:System.Web.Optimization.Styles.Url("~/bundles/css/global") %>" rel="stylesheet" />
    <%: System.Web.Optimization.Scripts.Render("~/bundles/js/global") %>

    <style type="text/css">
        .campaign-ov{border: 1px solid black; border-radius: 4px;margin-bottom: 40px;}
        .campaign-ov-head{ padding: 15px;color: white;background-color: black;text-align: center;font-size: 30px;margin-bottom: 10px;}
        .campaign-ov-head.notpublished{ background-color: red !important; }
        .campaign-ov-head.published{ background-color: green; }
        .campaign-ov-head.Shortcut, .campaign-ov-head.External{ background-color: yellow;color: black; }
        .campaign-ov-block { border: 1px solid blue; }
        .campaign-ov-block-head{padding: 15px;color: white;background-color: blue;text-align: center;font-size: 20px;margin-bottom: 10px;}
    </style>

    <div class="epi-contentContainer epi-padding">
        <div class="epi-contentArea">
            <h1 class="EP-prefix">Kampanjöversikt</h1>
            <div class="container" style="margin-bottom: 50px;">
                <div class="col-lg-4">
                    <div class="checkbox">
                        <asp:CheckBox runat="server" Text="Visa endast kampanjer som är publicerade" ID="cbOnlyPublished"/>
                    </div>
                    <div class="checkbox">
                        <asp:CheckBox runat="server" Text="Visa endast kampanjer som inte redirectas" ID="cbOnlyNormal"/>
                    </div>
                    <div class="form-group">
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCampaignPageTypes" DataTextField="Description" DataValueField="ID"/>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="tbKayakCampaign">Filtrera på Kayak kampanj</asp:Label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="tbKayakCampaign"></asp:TextBox> 
                    </div>

                    <asp:Button runat="server" CssClass="btn btn-default" Text="Visa kampanjöversikt" OnClick="ShowCampaignOverview"/>
                </div>
            </div>                                    
            <asp:Repeater runat="server" ID="repCampaignOverview">
                <HeaderTemplate>
                    <div class="container">
                        <h3><%=repCampaignOverview.Items.Count %> kampanjer</h3>
                        <hr/>
                </HeaderTemplate>
                <ItemTemplate>                   
                    <div class="row campaign-ov" id="campaign_<%#((CampaignOverviewData)Container.DataItem).CampaignPage.ContentLink.ID %>">
                        <div class="campaign-ov-head 
                            <%#((CampaignOverviewData)Container.DataItem).CampaignPage.CheckPublishedStatus(PagePublishedStatus.Published) ? "published" : "notpublished" %> 
                            <%# (PageShortcutType)Enum.Parse(typeof(PageShortcutType), ((CampaignOverviewData)Container.DataItem).CampaignPage["PageShortCutType"].ToString()) %> 
                            col-lg-12">
                            [<%#GetNicePageTypeName(((CampaignOverviewData)Container.DataItem).CampaignPage) %>] <%#((CampaignOverviewData)Container.DataItem).CampaignPage.Name %>
                            (<%#((CampaignOverviewData)Container.DataItem).CampaignPage.ContentLink.ID %>)
                        </div>
                        <div class="col-lg-4">
                            <strong>Sidtyp: </strong><%#((CampaignOverviewData)Container.DataItem).CampaignPage.PageTypeName %><br/>
                            <strong>Sidans namn: </strong><%#((CampaignOverviewData)Container.DataItem).CampaignPage.Name %><br/>
                            <strong>Sidans id: </strong><%#((CampaignOverviewData)Container.DataItem).CampaignPage.ContentLink.ID %><br/>
                            <strong>Målgrupp: </strong><%# ((CampaignOverviewData)Container.DataItem).CampaignPage.TargetGroup%> (<%# ((CampaignOverviewData)Container.DataItem).CampaignPage.TargetGroupMobile%>)
                        </div>
                        <div class="col-lg-4">
                            <strong>Publicerad: </strong><%#((CampaignOverviewData)Container.DataItem).CampaignPage.CheckPublishedStatus(PagePublishedStatus.Published) ? "JA" : "NEJ" %><br />
                            <strong>Ändrad senast av: </strong><%#((CampaignOverviewData)Container.DataItem).CampaignPage.ChangedBy%><br />
                            <strong>Datum: </strong><%#((CampaignOverviewData)Container.DataItem).CampaignPage.Changed.ToString("yyyy-MM-dd HH:mm")%><br />
                        </div>
                        <div class="col-lg-2">
                            <strong>Genvägstyp: </strong><%# (PageShortcutType)Enum.Parse(typeof(PageShortcutType), ((CampaignOverviewData)Container.DataItem).CampaignPage["PageShortCutType"].ToString()) %>
                        </div>
                        <div class="col-lg-2">
                            <strong>Länk: </strong><a href="<%#((CampaignOverviewData)Container.DataItem).CampaignPage.LinkURL%>" target="_blank"><%# ((CampaignOverviewData)Container.DataItem).CampaignPage.PageName%></a>
                        </div>
                            <asp:Repeater runat="server" DataSource="<%#((CampaignOverviewData)Container.DataItem).BlockOverviewDatas %>">
                                <HeaderTemplate>
                                    <div class="col-lg-12" style="text-align: center; margin-top: 20px;">
                                        <h4>Kampanjblock</h4>
                                    </div>                                        
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="col-lg-4" style="margin-bottom: 10px;">                                                            
                                        <div class="col-lg-12" style="border: 1px solid blue; border-radius: 4px; padding-bottom: 20px;">
                                            <div class="row campaign-ov-block-head">
                                            <%#((IContent)((BlockOverviewData)Container.DataItem).CampaignBlock).Name %> 
                                            (<%#((IContent)((BlockOverviewData)Container.DataItem).CampaignBlock).ContentLink.ID %>)
                                        </div>   
                                            <div class="col-lg-9">
                                                <strong>Namn: </strong><%#((IContent)((BlockOverviewData)Container.DataItem).CampaignBlock).Name %><br/>
                                                <strong>Blockmapp: </strong><%#ServiceLocator.Current.GetInstance<IContentRepository>().Get<IContent>(((IContent)((BlockOverviewData)Container.DataItem).CampaignBlock).ParentLink).Name %><br/>
                                            </div>
                                            <div class="col-lg-3">
                                                <strong>Id: </strong><%#((IContent)((BlockOverviewData)Container.DataItem).CampaignBlock).ContentLink.ID %>
                                            </div>
                                            <asp:PlaceHolder runat="server" Visible="<%#!string.IsNullOrEmpty(((BlockOverviewData)Container.DataItem).CampaignBlock.FirstCampaignPeriod.CampaignCardAndInvoice) || !string.IsNullOrEmpty(((BlockOverviewData)Container.DataItem).CampaignBlock.FirstCampaignPeriod.CampaignAutogiro) %>">
                                                <div class="col-lg-12">
                                                    <br/>
                                                    <strong style="font-size: 15px;">PERIOD 1:</strong><br/>
                                                    <strong>Kort och faktura: </strong><%#((BlockOverviewData)Container.DataItem).CampaignBlock.FirstCampaignPeriod.CampaignCardAndInvoice %><br/>
                                                    <strong>Autogiro: </strong><%#((BlockOverviewData)Container.DataItem).CampaignBlock.FirstCampaignPeriod.CampaignAutogiro %><br/>
                                                </div>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder runat="server" Visible="<%#!string.IsNullOrEmpty(((BlockOverviewData)Container.DataItem).CampaignBlock.SecondCampaignPeriod.CampaignCardAndInvoice) || !string.IsNullOrEmpty(((BlockOverviewData)Container.DataItem).CampaignBlock.SecondCampaignPeriod.CampaignAutogiro) %>">
                                                <div class="col-lg-12">
                                                    <br/>
                                                    <strong style="font-size: 15px;">PERIOD 2:</strong><br/>
                                                    <strong>Kort och faktura: </strong><%#((BlockOverviewData)Container.DataItem).CampaignBlock.SecondCampaignPeriod.CampaignCardAndInvoice %><br/>
                                                    <strong>Autogiro: </strong><%#((BlockOverviewData)Container.DataItem).CampaignBlock.SecondCampaignPeriod.CampaignAutogiro %><br/>
                                                </div>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder runat="server" Visible="<%#!string.IsNullOrEmpty(((BlockOverviewData)Container.DataItem).CampaignBlock.ThirdCampaignPeriod.CampaignCardAndInvoice) || !string.IsNullOrEmpty(((BlockOverviewData)Container.DataItem).CampaignBlock.ThirdCampaignPeriod.CampaignAutogiro) %>">
                                                <div class="col-lg-12">
                                                    <br/>
                                                    <strong style="font-size: 15px;">PERIOD 3:</strong><br/>
                                                    <strong>Kort och faktura: </strong><%#((BlockOverviewData)Container.DataItem).CampaignBlock.ThirdCampaignPeriod.CampaignCardAndInvoice %><br/>
                                                    <strong>Autogiro: </strong><%#((BlockOverviewData)Container.DataItem).CampaignBlock.ThirdCampaignPeriod.CampaignAutogiro %><br/>
                                                </div>
                                            </asp:PlaceHolder>
                                      
                                            <asp:PlaceHolder runat="server" Visible="<%#((BlockOverviewData)Container.DataItem).UsedOn.Any() %>">
                                                <asp:Repeater runat="server" DataSource="<%#((BlockOverviewData)Container.DataItem).UsedOn %>">
                                                    <HeaderTemplate>                                                   
                                                            <div class="col-lg-12">
                                                                <hr/>
                                                                <h4>Används också på</h4>
                                                            </div>    
                                                    </HeaderTemplate>
                                                    <ItemTemplate>                                                
                                                            <div class="col-lg-12">
                                                                [<%#((CampaignPage)Container.DataItem).PageTypeName%>]
                                                                <a href="#campaign_<%#((CampaignPage)Container.DataItem).ContentLink.ID %>"><%# ((CampaignPage)Container.DataItem).PageName%></a> (<%#((CampaignPage)Container.DataItem).ContentLink.ID %>)
                                                                <%#((CampaignPage)Container.DataItem).CheckPublishedStatus(PagePublishedStatus.Published) ? "" : "AVPUBLICERAD" %>
                                                                <%# (PageShortcutType)Enum.Parse(typeof(PageShortcutType), ((CampaignPage)Container.DataItem)["PageShortCutType"].ToString()) == PageShortcutType.Normal ? "" : ((PageShortcutType)Enum.Parse(typeof(PageShortcutType), ((CampaignPage)Container.DataItem)["PageShortCutType"].ToString())).ToString() %>
                                                            </div>                                                
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                
                                                    </FooterTemplate>
                                                </asp:Repeater>      
                                            </asp:PlaceHolder>                                                      
                                        </div>                                                                                
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                    </div>                                        
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>        
        </div>
    </div>
</asp:content>
