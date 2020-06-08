<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ArticlePopUpNav.aspx.cs" Inherits="EPiServer.Functions.Articles.ArticlePopUpNav" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" runat="server" id="HtmlElement">

  <head>
    <title><%=p_Title %></title>
    <script language="JavaScript" type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/popup.js")%>"></script>
    <script language="JavaScript" type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/rollover.js")%>"></script>
    <link rel="stylesheet" href="<%=ResolveUrl("~/Templates/Public/Styles/paper.css")%>" type="text/css" />

    <script type="text/javascript">
      function setElement(elemName, status)
      {
        img = document.getElementById('btn_' + elemName);
        img.src = "/Templates/Public/Images/paper/btn_" + status + ".gif";        
      }

      function toggleContent(type)
      {
        if (type == 'text')
        {
          setElement('text', 'down');
          setElement('pdf',  'right');

          window.parent.article.location = "ArticlePopUpBody.aspx?art=<%=p_artlink%>";
        }
        else if (type == 'pdf')
        {
          setElement('text', 'right');
          setElement('pdf',  'down');

          window.parent.article.location = "/PDFTidning/<%=p_Datepath %>/<%=p_artlink%>.pdf";
        }
      }

      function framePrint( frame )
      {
        if( window.print )
        {
          window.parent[frame].focus();
          window.parent[frame].print();
        }
        else
        {
          alert('Använd ustkriftsfunktionen i huvudmenyn');
        }
      }

      function init()
      {
        toggleContent( 'text' );
      }
    </script>
  </head>

  <body class="mg00" onload="init();">

    <table width="100%" cellspacing="0" cellpadding="0">
      <tr>
        <td id="text" class="artnavigator" width="1" align="right" onclick="toggleContent('text')">
          <nobr><img id="btn_text" src="<%=ResolveUrl("~/Templates/Public/Images/paper/btn_right.gif") %>" border="0"> <a href="#">Textversion</a></nobr></td>

        <td id="pdf" class="artnavigator" width="1" onClick="toggleContent('pdf')">
          <nobr><img id="btn_pdf" src="<%=ResolveUrl("~/Templates/Public/Images/paper/btn_right.gif") %>" border="0"> <a href="#">PDF version</a></nobr></td>

        <td id="print" class="artnavigator" width="1">
          <nobr><img src="<%=ResolveUrl("~/Templates/Public/Images/paper/btn_right.gif") %>" border="0"> <a href="javascript:framePrint('article')">Skriv ut</a></nobr></td>

        <td id="sendArt" class="artnavigator" width="1">
          <nobr><img src="<%=ResolveUrl("~/Templates/Public/Images/paper/btn_right.gif") %>" border="0"> <a href="javascript:void(0);" onclick="sendDIArticle('NS<%=p_artlink.ToUpper() %>')">Skicka artikel</a></nobr></td>


        <td>&nbsp;</td>

      </tr>
    </table>

  </body>

</html>

