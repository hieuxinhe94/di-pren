<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wine.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Antipodes.Wine" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
<div class="infobox"> 
	<div class="wrapper"> 
		<h2>Erbjudanden</h2> 
		<div class="content"> 
			<ul class="winelist"> 
				<li> 
					<div class="image-wrapper"> 
						<img src='<%= imageURL %>' /> 
					</div> 
					<h3>
                        <asp:Literal ID="NameLiteral" runat="server" />
                    </h3> 
					<p class="region">
                        <asp:Literal ID="CountryRegionLiteral" runat="server" />
                    </p> 
					<p class="price">
                        <asp:Literal ID="PriceLiteral" runat="server" />
                    </p> 
					<p class="quantity">
                        <asp:Literal ID="QuantityLiteral" runat="server" />
                    </p>
					<a class="btn" href="#">
                        <span>
                            <EPiServer:Translate Text="/common/readmore" runat="server" />
                        </span>
                    </a> 
				</li> 												
			</ul> 
		</div> 
	</div> 
</div> 

</asp:Content>