<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellAdEditor.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Gasellerna.GasellAdEditor" %>

<div class="ColumnBox adarea">
    <table cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
	            <td class="heading">
	                <%= HttpUtility.HtmlEncode(ActualCurrentPage.PageName)%>	                
	            </td>
            </tr>
            <tr>
                <td class="ad">
                    <%= ActualCurrentPage["PuffEditor"] %>
                </td>
            </tr>                                             
        </tbody>
    </table>    
</div>