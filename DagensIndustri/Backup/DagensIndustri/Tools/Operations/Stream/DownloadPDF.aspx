<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadPDF.aspx.cs" Inherits="DagensIndustri.Tools.Operations.Stream.DownloadPDF" %>
<%@ OutputCache Duration="120" VaryByParam="appendix;issueid"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" runat="server" id="HtmlElement">
    <head id="Head1" runat="server">
	    <title>Ladda ned PDF</title>	    
	    <script type="text/javascript">
	        /*function resizeThis(){
	        window.resizeTo (300,200)
	        }*/
	    </script>
    </head>
    
    <body onload="resizeThis()" style="background-color:#feede4;">
        <asp:PlaceHolder ID="contentHolder" runat="server" visible="False">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="text-align:center">
                        <asp:Label runat="server" ID="LblMessage"></asp:Label>		                
                    </td>
                </tr>
                <tr>
                    <td style="text-align:center">
                        <button onclick="window.close()" type="button">Stäng</button>
                    </td>
                </tr>
            </table>
        </asp:PlaceHolder>
    </body>
</html>