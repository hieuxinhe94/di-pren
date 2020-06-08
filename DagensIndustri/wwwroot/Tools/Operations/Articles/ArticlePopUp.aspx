<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ArticlePopUp.aspx.cs" Inherits="EPiServer.Functions.Articles.ArticlePopUp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" runat="server" id="HtmlElement">

    <head runat="server">
	    <title><%=p_Title %></title>
    </head>
    
   
    <frameset rows="30,*" framespacing="0" frameborder="0" border="0">
        <frame name="navigation" src="ArticlePopUpNav.aspx?art=<%=p_artlink %>" scrolling="no" style="border-bottom: 2px solid #FEEDE4" />
        <frame name="article" src="" scrolling="auto"/>
    </frameset>
   
    
</html>

