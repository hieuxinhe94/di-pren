<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="Papers.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Papers" %>
<%@ Import Namespace="DagensIndustri.Templates.Public.Units.Placeable" %>
<%@ Register TagPrefix="di" TagName="ConcUsers" Src="~/Templates/Public/Units/Placeable/ConcurrentUsers.ascx" %>

<di:ConcUsers ID="ConcUsers1" runat="server" />

<asp:HiddenField ID="SelectedIssueHiddenField" runat="server" />
    <asp:HiddenField ID="SelectedPaperDateHiddenField" runat="server" />

    <%--UserInRoleAllowedToReadPdfs: <%=UserInRoleAllowedToReadPdfs%><br />
    RoleDiSms24Hour: <%=HttpContext.Current.User.IsInRole(DIClassLib.Membership.DiRoleHandler.RoleDiSms24Hour)%><br />--%>
    
    <!-- Online-magazines -->
    <asp:Repeater ID="PapersCurrentRepeater" OnItemDataBound="PapersCurrentRepeater_ItemDataBound" runat="server">
        <HeaderTemplate>
            <%=EditionsWrapperStartElement%>
        </HeaderTemplate>
        <ItemTemplate>
            <%=EditionsItemStartElement%>   <%--class="<%# ((Paper)Container.DataItem).CSSClass %>"--%>

                <asp:PlaceHolder ID="PaperPlaceHolder" runat="server">
                    <img src='<%# GetImageUrl((Paper)Container.DataItem) %>' alt="<%# ((Paper)Container.DataItem).Name %>" />
				            <p class="date"><%# ((Paper)Container.DataItem).GetPaperDate() %></p>

                    <asp:HyperLink ID="ReadPaperHyperLink" NavigateUrl="#" CssClass="btn login-required" runat="server">
                        <%--<span><EPiServer:Translate Text="/onlinepapers/read" runat="server" /></span>--%>
                        <span><EPiServer:Translate ID="Translate1" Text="Logga in" runat="server" /></span>
                    </asp:HyperLink>
                </asp:PlaceHolder>

                <asp:PlaceHolder ID="TomorrowsPaperPlaceHolder" runat="server">
                    <div class="img-wrapper">
                        <asp:Image ID="TomorrowsPaperImage" Alternate="<%# ((Paper)Container.DataItem).Name %>" runat="server" /> 
					              <%--<p class="exclusive"><EPiServer:Translate Text="/onlinepapers/exclusive" runat="server" /></p>--%>
					          </div>

                        <%--<h2>--%><%# ((Paper)Container.DataItem).Name %><%--</h2>--%>
                        <p class="date"><asp:Literal ID="DateCountDownLiteral" runat="server" /></p>

                    <asp:PlaceHolder ID="LinkPlaceHolder" runat="server">
                        <asp:HyperLink ID="ReadTomorrowsPaperHyperLink" runat="server">
                            <%--<span><EPiServer:Translate Text="/onlinepapers/read" runat="server" /></span>--%>
                            <%--<span><EPiServer:Translate Text="Logga in" runat="server" /></span>--%>
                            <span><asp:Literal ID="LiteralReadTomorrowBtn" runat="server"/></span>
                        </asp:HyperLink>

                        <div style="margin-top:40px;">
                             <a id="DownloadPaperAnchor" target="_blank" runat="server">
                                <EPiServer:Translate ID="Translate2" Text="/common/download" runat="server" />
                            </a>
                        </div>
                    </asp:PlaceHolder>
                </asp:PlaceHolder>
            <%=EditionsItemEndElement%>
        </ItemTemplate>
        <FooterTemplate>
            <%=EditionsWrapperEndElement%>
        </FooterTemplate>
    </asp:Repeater>
    <!-- // Online-magazines -->
    
    
    <!-- Previous magazines -->	
    <asp:Repeater ID="PapersPreviousRepeater" OnItemDataBound="PapersPreviousRepeater_ItemDataBound" runat="server">
        <HeaderTemplate>
            <h2><EPiServer:Translate ID="Translate3" Text="/onlinepapers/previousnumbers" runat="server" /></h2>
            <%=EditionsWrapperStartElement%>
        </HeaderTemplate>
        <ItemTemplate>
            <%=EditionsItemStartElement%>
                
                <%--<asp:PlaceHolder ID="PlaceHolder1" Visible='<%# UserInRoleAllowedToReadPdfs && !HttpContext.Current.User.IsInRole(DIClassLib.Membership.DiRoleHandler.RoleDiSms24Hour) %>' runat="server">--%>
                <asp:PlaceHolder ID="PaperAccessiblePlaceHolder" Visible="false" runat="server">
                    <%--<a href='<%# Paper.GetLinkHref(((Paper)Container.DataItem).Issue, ((Paper)Container.DataItem).PaperDate)%>' target="<%# ((Paper)Container.DataItem).LinkTarget %>">--%>
                        <img src='<%# ((Paper)Container.DataItem).GetImgSrc(110) %>' title="<%# ((Paper)Container.DataItem).PaperDate.ToString("yyyy-MM-dd") %>" alt="<%# ((Paper)Container.DataItem).PaperDate.ToString("yyyy-MM-dd") %>" />
                    <%--</a>--%>
                    <p class="date"><%# ((Paper)Container.DataItem).GetPaperDate() %></p>
                    <% if (MobileView)
                       { %>
                      <a href="/Tools/Operations/Stream/DownloadPDF.aspx?appendix=false&issueid=<%#((Paper)Container.DataItem).PaperDate.ToString("yyyyMMdd") %>" target="<%# ((Paper)Container.DataItem).LinkTarget %>" class="btn">
                          <span>PDF</span>
                      </a>

                    <% }else{%>
                      <a href='<%# Paper.GetLinkHref(((Paper)Container.DataItem).Issue, ((Paper)Container.DataItem).PaperDate)%>' target="<%# ((Paper)Container.DataItem).LinkTarget %>" class="btn">
                          <span>Läs</span>
                      </a>
                    <%} %>
                </asp:PlaceHolder>

                <%--<asp:PlaceHolder ID="PlaceHolder1" Visible='<%# !UserInRoleAllowedToReadPdfs && !HttpContext.Current.User.IsInRole(DIClassLib.Membership.DiRoleHandler.RoleDiSms24Hour) %>' runat="server">--%>
                <asp:PlaceHolder ID="PaperNotAccessiblePlaceHolder" Visible="false" runat="server">
                    <%--<asp:HyperLink ID="ReadPaperHyperLink" NavigateUrl="#" CssClass="login-required" runat="server">--%>
                        <img src='<%# ((Paper)Container.DataItem).GetImgSrc(110) %>' title="<%# ((Paper)Container.DataItem).PaperDate.ToString("yyyy-MM-dd") %>" alt="<%# ((Paper)Container.DataItem).PaperDate.ToString("yyyy-MM-dd") %>" />
                    <%--</asp:HyperLink>--%>
                    <p class="date"><%# ((Paper)Container.DataItem).GetPaperDate() %></p>
                </asp:PlaceHolder>
			    
                <%--<asp:PlaceHolder ID="PlaceHolder1" Visible='<%# HttpContext.Current.User.IsInRole(DIClassLib.Membership.DiRoleHandler.RoleDiSms24Hour) %>' runat="server">--%>
                <asp:PlaceHolder ID="SMSGroupPlaceHolder" Visible="false" runat="server">
                    <img src='<%# ((Paper)Container.DataItem).GetImgSrc(110) %>' title="<%# ((Paper)Container.DataItem).PaperDate.ToString("yyyy-MM-dd") %>" alt="<%# ((Paper)Container.DataItem).PaperDate.ToString("yyyy-MM-dd") %>" />
                    <p class="date"><%# ((Paper)Container.DataItem).GetPaperDate() %></p>
                </asp:PlaceHolder>

			    
		    <%=EditionsItemEndElement%>
        </ItemTemplate>
        <FooterTemplate>
            <%=EditionsWrapperEndElement%>
        </FooterTemplate>
    </asp:Repeater>
	<!-- // Previous magazines -->
    <asp:PlaceHolder ID="HideArchiveLinkPlaceHolder" runat="server">
        <p>
		    <a href='<%= ArchiveURL %>' class="more">
                <EPiServer:Translate ID="Translate4" Text="/archive/toarchive" runat="server" />
            </a>
	    </p>
    </asp:PlaceHolder>
