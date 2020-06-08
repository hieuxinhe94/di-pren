<%@ Page Title="" Language="C#" MasterPageFile="~/DiIpad/Master.Master" AutoEventWireup="true" CodeBehind="RssBig.aspx.cs" Inherits="WS.DiIpad.RssBig" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
    <td valign="top">
        <img src="Images/dise.gif" alt="" border="0" style="margin-bottom:7px;" /><br />
        <div id="selectedItem"></div>
    </td>
    <td width="30"></td>
    <td width="300" valign="top">
        <a href="RssBig.aspx">Uppdatera listan</a><hr />
        <div id="newsList"></div>
    </td>
    </tr>
    </table>

        <script type="text/javascript">
            
            var xmlDoc = GetXmlDoc();
            document.getElementById("newsList").innerHTML = GetLinkListAsHtml(xmlDoc, true);

            function PrintSelectedItem(id) {
                var html = "";
                var itm = xmlDoc.getElementsByTagName("NewsItem");
                
                for (i = 0; i < itm.length; i++) {
                    
                    if (id == itm[i].getElementsByTagName("NewsItemId")[0].childNodes[0].nodeValue || id == null) {
                        
                        html = "<div class='date'>" + FormatDate(itm[i].getElementsByTagName("DateId")[0].childNodes[0].nodeValue) + "</div>" +
                               "<div class='headlineItem'>" + itm[i].getElementsByTagName("HeadLine")[0].childNodes[0].nodeValue + "</div>";

                        var intro = itm[i].getElementsByTagName("block")[0].childNodes[0];
                        if (intro != null)
                            html += "<div class='introItem'>" + intro.nodeValue + "</div>";

                        var main = itm[i].getElementsByTagName("block")[1].childNodes[0];
                        if (main != null)
                            html += "<div class='bread'>" + main.nodeValue + "</div>";

                        var person = itm[i].getElementsByTagName("person")[0].childNodes[0];
                        if (person != null)
                            html += "<div class='person'>" + person.nodeValue + "</div>";
                        
                        document.getElementById("selectedItem").innerHTML = html;
                        scroll(0, 0);   //go to top of page

                        return;
                    }
                }
            }

            PrintSelectedItem(<%=Id%>);

        </script>
</asp:Content>
