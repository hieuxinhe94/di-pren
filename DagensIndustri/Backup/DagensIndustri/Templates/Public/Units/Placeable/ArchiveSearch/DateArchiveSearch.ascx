<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateArchiveSearch.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ArchiveSearch.DateArchiveSearch" %>
<%@ Register TagPrefix="di" TagName="input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class="section form-date" id="archive-search_date">
	<div class="row">
		<div class="col">          
			<%--<div class="input date-inline">
            </div>
			<input type="hidden" id="input-datepicker" name="search" value="2011-02-25" />
			<input type="submit" value="Sök" />--%>
                <p>
                <DI:Input ID="SearchOnDateInput" Title="<%$ Resources: EPiServer, archive.paper.datesearch.publicationdate %>" CssClass="text date" Name="date-to" TypeOfInput="Date" Text="<%$ Resources: EPiServer, archive.paper.datesearch.enterdate %>" runat="server" />
			</p>
            <div class="button-wrapper">
                <p class="description">
                    <EPiServer:Translate Text="/archive/paper/datesearch/searchondatedescription" runat="server" /><br />
                </p>
                <asp:Button ID="SearchButton" CssClass="btn" OnClick="SearchButton_Click" Text="<%$ Resources: EPiServer, common.search %>" runat="server" />

                <asp:PlaceHolder ID="PlaceHolderWeekendSubscriberText" Visible="false" runat="server">
                    <p class="description" style="margin-top:20px;">Obs! Helgprenumeranter kan endast läsa helgutgåvor.</p>
                </asp:PlaceHolder>

            </div>                
		</div>
						
		<div class="col paper">
                
			<img src='<%= GetPapersImage()%>' />
			<h3>
                <EPiServer:Translate Text="/common/dagensindustri" runat="server" />
            </h3>
			<p class="date">
                <%= GetPapersDate() %>
            </p>

            <asp:PlaceHolder ID="HiddenButton" runat="server">
                
                <asp:PlaceHolder ID="PlaceHolderTexTalkBtn" runat="server">
                    <a href='<%= GetPapersLink() %>' class="btn" target="_blank">
                        <span>
                            <EPiServer:Translate Text="/common/read" runat="server" />
                        </span>
                    </a>
                </asp:PlaceHolder>

                <a href="javascript:void(0);" onclick="<%= GetDownloadLink() %>" class="btn" target="_blank">
                    <span>
                        <EPiServer:Translate ID="Translate3" Text="/common/download" runat="server" />
                    </span>
                </a>
            </asp:PlaceHolder>					
		</div>
	</div>
</div>