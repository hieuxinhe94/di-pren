<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateArchiveSearch.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DateArchiveSearch" %>
<%@ Register TagPrefix="di" TagName="input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class="form-nav">
  	<ul>
  		<li class="current">
            <a href="#archive-search_date">
                <EPiServer:Translate Text="/archive/paper/datesearch/searchondate" runat="server" />
            </a>
        </li>
  	</ul>
  		
</div>			
			
<div class="form-box">
	<div class="section form-date" id="archive-search_date">
		<div class="row">
			<div class="col">          
				<%--<div class="input date-inline">
                </div>
				<input type="hidden" id="input-datepicker" name="search" value="2011-02-25" />
				<input type="submit" value="Sök" />--%>
                 <p>
                    <DI:Input ID="SearchOnDateInput" Title="Utgivningsdatum <i>(YYYY-MM-DD)" CssClass="text date" Name="date-to" TypeOfInput="Date" Text="Fyll i ett datum." runat="server" />
				</p>
                <div class="button-wrapper">
                    <p class="description">
                        <EPiServer:Translate ID="Translate1" Text="/archive/paper/datesearch/searchondatedescription" runat="server" />
                    </p>
                    <asp:Button ID="SearchButton" CssClass="btn"  OnClick="SearchButton_Click" Text="Sök" runat="server" />
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
				    <a href='<%= GetPapersLink() %>' class="btn" target="_blank">
                        <span>
                            <EPiServer:Translate Text="/common/read" runat="server" />
                        </span>
                    </a>

                    <a href="javascript:void(0);" onclick="<%= GetDownloadLink() %>" class="btn" target="_blank">
                        <span>
                            <EPiServer:Translate Text="/common/download" runat="server" />
                        </span>
                    </a>
                </asp:PlaceHolder>					
			</div>
		</div>
	</div>
</div>