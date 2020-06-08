<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellAd.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Gasellerna.GasellAd" %>

<%--Add pagelink to id on div. Id must be unique for swfobject--%>
<div id="flasharea<%=ActualCurrentPage.PageLink%>" class="flasharea" >    
    <asp:PlaceHolder runat="server" ID="PhImage">            
        <a href='<%=ActualCurrentPage["AdImageLink"]%>' target="_blank">
            <img src='<%=ActualCurrentPage["AdImage"] as string ?? "missing.jpg"%>' alt='<%=ActualCurrentPage["AdImageAltText"]%>' />
        </a>
    </asp:PlaceHolder>
</div>            

     
<asp:PlaceHolder runat="server" ID="PhFlashScript" Visible="false">
    <script type="text/javascript">
        // <![CDATA[
        var so = new SWFObject('<%=ActualCurrentPage["AdFlashMovie"]%>', 'adflash', '<%=ActualCurrentPage["AdFlashWidth"] ?? 160%>px', '<%=ActualCurrentPage["AdFlashHeight"] ?? 300%>px', '<%=ActualCurrentPage["AdFlashVersion"] ?? 7%>', '#FFFFFF');
        so.addParam("scale", "exactfit");
        so.write("flasharea<%=ActualCurrentPage.PageLink%>");
        // ]]>
    </script> 
</asp:PlaceHolder> 