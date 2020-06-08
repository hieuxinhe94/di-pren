<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellAd.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellAd" %>
<%@ Register TagPrefix="DI" TagName="GasellAd" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellAd.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="ColumnBox adarea pink">
        <table cellspacing="0" cellpadding="0">
            <tbody> 
                <tr>
                    <td class="ad">  
                        <DI:GasellAd runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>  
</asp:Content>
