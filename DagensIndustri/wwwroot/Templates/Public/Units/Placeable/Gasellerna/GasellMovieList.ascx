<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellMovieList.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Gasellerna.GasellMovieList" %>
<%@ Register TagPrefix="DI" TagName="GasellMovie" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellMovie.ascx" %>

<EPiServer:PageList runat="server" ID="PlMovieList">

    <HeaderTemplate>
        <div class="ColumnBox movielist">
            <table cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
	                    <td class="heading">
	                        <%= HttpUtility.HtmlEncode(ActualCurrentPage.PageName)%>
	                    </td>
                    </tr>    
    </HeaderTemplate>
    
    <ItemTemplate>
           <tr>
                <td class="movie">	                
                    <DI:GasellMovie runat="server" />	                
                </td>
            </tr>    
    </ItemTemplate>
    
    <FooterTemplate>
                </tbody>
            </table>      
        </div>
    </FooterTemplate>

</EPiServer:PageList>