<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellPuff.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Gasellerna.GasellPuff" %>

<div class="ColumnBox puff">
    <table cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
	            <td class="heading">
	                <%= HttpUtility.HtmlEncode(ActualCurrentPage.PageName)%>	               
	            </td>
            </tr>
            <tr>
                <td class="image">	                                
                    <a href='<%= linkURL %>' target="_blank">
                        <img alt='<%= ActualCurrentPage["PuffImageAltText"]%>' src='<%= ActualCurrentPage["PuffImage"] %>'/>
                    </a>	                
                </td>
            </tr>
            <tr>
                <td class="link redArr">
                    <a href='<%= linkURL %>' target="_blank"><%= HttpUtility.HtmlEncode(ActualCurrentPage["PuffLinkText"] as string)%></a>
                </td>
            </tr>
        </tbody>
    </table>    
</div>