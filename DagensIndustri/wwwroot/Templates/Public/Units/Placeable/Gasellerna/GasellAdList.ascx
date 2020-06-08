<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellAdList.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Gasellerna.GasellAdList" %>
<%@ Register TagPrefix="DI" TagName="GasellAd" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellAd.ascx" %>

<EPiServer:PageList runat="server" ID="PlAdList">
    <HeaderTemplate>
        <div class="ColumnBox adarea pink">
            <table cellspacing="0" cellpadding="0">
                <tbody>  
    </HeaderTemplate>
    <ItemTemplate>
                    <tr>
                        <td class="ad">                        
                            <DI:GasellAd runat="server" />
                        </td>
                    </tr>     
    </ItemTemplate>
    <FooterTemplate>
               </tbody>
            </table>    
        </div>    
    </FooterTemplate>
</EPiServer:PageList> 