<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellMovie.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellMovie" %>
<%@ Register TagPrefix="DI" TagName="GasellMovie" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellMovie.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="ColumnBox movielist">
        <table cellspacing="0" cellpadding="0">
            <tbody>  
               <tr>
                    <td class="movie">	                
                        <DI:GasellMovie runat="server" />          
                    </td>
                </tr>    
            </tbody>
        </table>      
    </div>
</asp:Content>

