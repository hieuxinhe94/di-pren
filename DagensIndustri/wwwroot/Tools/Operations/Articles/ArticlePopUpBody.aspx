<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ArticlePopUpBody.aspx.cs" Inherits="EPiServer.Functions.Articles.ArticlePopUpBody" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" runat="server" id="HtmlElement">

  <head>
    <title><%=p_Title %></title>
    <meta name="PrenLevel" content="1">
    <meta name="PublishDate" content="2009-10-06">
    <meta http-equiv="Content-Type" content="text/html; charset=CP1252">

    <link rel="stylesheet" href="<%=ResolveUrl("~/Templates/Public/Styles/paper.css")%>" type="text/css" />
  </head>

  <body>

    <table class="body" width="100%" cellspacing="0" cellpadding="0">
      <tr>
        <td class="artheader"><%=p_Headline %></td>
      </tr>

      <tr>
        <td class="artbody"><%=p_PublishDate%>:&nbsp; <%=p_Authors %></td>
      </tr>
      <tr>
        <td class="artbody">
            <%=p_ArticleText %>
        </td></tr>
                
       <tr> <td class="artbody">&copy; di.se</td>
      </tr>
    </table>

 </body>

</html>

