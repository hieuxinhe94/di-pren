<%@ Page Language="C#" AutoEventWireup="true" Codebehind="LinkedImageCreator.aspx.cs"
    Inherits="EPiCode.ImageList.Dialogs.LinkedImageCreator" %>

<%@ Register TagPrefix="customcontrol" Namespace="EPiCode.ImageList.Dialogs" Assembly="EPiCode.ImageListProperty" %>
<%@ Register TagPrefix="EPiServer" Namespace="EPiServer.Web.WebControls" Assembly="EPiServer.Web.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <base target="_self" />
    <title>Linked Image Wizard</title>
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/App_Themes/Default/styles/system.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/App_Themes/Default/styles/ToolButton.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/styles/ImageListStyle.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/EPiServer/Shell/ClientResources/Shell.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/EPiServer/Shell/ClientResources/ShellLightTheme.css") %>" />

    <script type='text/javascript' src="<%= EPiServer.UriSupport.ResolveUrlFromUtilBySettings("javascript/episerverscriptmanager.js") %>"></script>
    <script type='text/javascript' src="<%= EPiServer.UriSupport.ResolveUrlFromUIBySettings("javascript/system.js") %>"></script>
    <script type='text/javascript' src="<%= EPiServer.UriSupport.ResolveUrlFromUIBySettings("javascript/system.aspx") %>"></script>

    <script type="text/javascript">
    
    // Pass the page or parent page id to the popup. Will be used in page browser dialog
    var pageId                      = '<%= Page.Request.QueryString["pageid"] ?? "0" %>';
    var parentPageId                = '<%= Page.Request.QueryString["parentId"] ?? "0" %>';
    
    
    // The file manager needs the folder id in order to show or create the "Page Files" directory. 
    var folderId = '<%= Page.Request.QueryString["folderId"] ?? Page.Request.QueryString["pageid"] %>';
    
    function SelectImageLink(postbackCtrlId)
    {   
        var url = '<%= EPiServer.UriSupport.ResolveUrlFromUIBySettings("edit/FileManagerBrowser.aspx") %>?id=' + pageId + '&parent=' + parentPageId + '&folderid=' + folderId;
        var selectedfile = '<%= LinkItem.ImageLink == null ? "" : LinkItem.ImageLink.URL %>';
        url += '&selectedfile=' + selectedfile;
        
        var linkAttributes = new Object();
	    
	    var dialogArguments = linkAttributes;
	    var features = {width: 600, height: 412};
	    var callbackArguments = new Object();
	    callbackArguments.postbackCtrlId = postbackCtrlId;
	    
	    /* opens the filemanagerbrowser dialog */       
	    EPi.CreateDialog(
	            url,
	            OnFileDialogClosed,
	            callbackArguments,
	            dialogArguments,
	            features);               
	    
    }
    
    /* function called after file browser dialog has returned and closed*/
    function OnFileDialogClosed(returnValue, callbackArguments)
    {
        //there is no postback if user clicks "Cancel - 0" or "X - undefined"
        // returnValue.closeAction: "cancel"
        // returnValue.closeAction: "clear"
        //if user clicks "Update" or "Clear - -1" postback event is raised in ImageSelector
        var test = undefined;
        if (returnValue != undefined && returnValue.closeAction != 'cancel') 
        {
            var linkTag;
            if (returnValue.items != undefined && returnValue.items.length > 0) {
                linkTag = '<image>' + returnValue.items[0].path + '</image>';
            }
            else if (returnValue.closeAction == 'clear') {
                linkTag = '<image>' + '-1' + '</image>';// the old way of representing the close action 'clear'
            }
	        __doPostBack(callbackArguments.postbackCtrlId, linkTag);
	    }
    }
    
    function SelectPageLink(postbackCtrlId)
    {
        var callingLanguageBranch  = '<%= Page.Request.QueryString["epspagelanguagebranch"] ?? "en" %>';
        var url = '<%= EPiServer.UriSupport.ResolveUrlFromUIBySettings("editor/dialogs/HyperlinkProperties.aspx") %>';
        var existingLink = '<%= LinkItem.PageLink == null ? "" : LinkItem.PageLink.URL %>';
        var existingTitle = '<%= LinkItem.PageLink == null ? "" : LinkItem.PageLink.AltText %>';
        var existingTarget = '<%= LinkItem.PageLink == null ? "" : LinkItem.PageLink.Target %>';
        var existingText = '<%= LinkItem.PageLink == null ? "" : LinkItem.PageLink.Name %>';
        url += '?url='+ existingLink;
        var linkAttributes			            = new Object();
		linkAttributes.href			            = existingLink;
		linkAttributes.target		            = existingTarget;
        linkAttributes.language                 = callingLanguageBranch;
        linkAttributes.title		            = existingTitle;
 	    linkAttributes.text			            = existingText;
	    linkAttributes.imageDialogUrl			= "";
	    linkAttributes.resizeDialogUrl			= "";
	    linkAttributes.parentWindow				= window;
	    linkAttributes.fileManagerBrowserUrl    = '<%= EPiServer.UriSupport.ResolveUrlFromUIBySettings("edit/FileManagerBrowser.aspx") %>?id=' + pageId + '&parent=' + parentPageId + '&folderid=' + folderId;

	    var dialogArguments = linkAttributes;
	    var features = {width: 460, height: 412};
	    var callbackArguments = new Object();
	    callbackArguments.postbackCtrlId = postbackCtrlId;
	    
	    /* opens the hyperlinkproperty dialog */       
	    EPi.CreateDialog(
	            url,
	            OnLinkDialogClosed,
	            callbackArguments,
	            dialogArguments,
	            features);   
    }
    
     /* function called after file browser dialog has returned and closed*/
    function OnLinkDialogClosed(returnValue, callbackArguments) {
        debugger;	
        //there is no postback if user clicks "Cancel - 0" or "X - undefined"
	    //if user clicks "Update" or "Clear - -1" postback event is raised in ImageSelector
	    if (returnValue != undefined && returnValue != 0)
	    { 	    
	       var linkTag = '<page name="' + returnValue.text + '" target="' + returnValue.target + '" alt="' + returnValue.title + '">' + returnValue.href + '</page>';
           __doPostBack(callbackArguments.postbackCtrlId, linkTag);
	    }
    }

    function CustomOnClose(xmltext)
    {
        EPi.GetDialog().Close(xmltext);
    }
    </script>

</head>
<body>
    <form id="LinkedImageWizard" enctype="multipart/form-data" method="post" runat="server">
        <div class="epi-padding" style="padding-left: 5px; overflow: hidden;">
            <div class="epi-contentArea">  
            <h1>
                <EPiServer:Translate ID="epiTranslateTitle" runat="server" text="/imagelistselector/imagecreator/title" />
            </h1>
            <EPiServer:Translate ID="epiTranslateIntro" runat="server" text="/imagelistselector/imagecreator/introduction" />            
            </div>
            <div class="epi-formArea">
            
            <fieldset style="margin-top:20px;">
                <legend><EPiServer:Translate runat="server" Text="/imagelistselector/creatorcommon/urlheading" /></legend>
                <table   style="width: 330px; " border="0">
                    <tr>
                        <td style="width: 250px; ">
                            <%# GetImage() %>
                        </td>
                        <td  valign="bottom">
                            <!-- adding span tag around buttons to simulate episerver toolbutton look -->
                            <span class="epitoolbutton" >
                                <customcontrol:ImageSelector ID="ImageSelectorControl" OnClick="ImageSelect_Click" runat="server" />
                            </span>
                        </td>
                    </tr>
                </table>
            </fieldset>
            </div>
            
            <asp:PlaceHolder runat="server" ID="flashPanel" Visible="false">
            <div class="epi-formArea">
                <fieldset>
                    <legend><EPiServer:Translate runat="server" Text="/imagelistselector/flashcreator/heading" /></legend>
                    <table  border="0" style="width: 330px; ">
                        <tr>
                            <td colspan="2">
                                <EPiServer:Translate ID="epiTrans1" runat="server" text="/imagelistselector/flashcreator/height" />:&nbsp;
                                <asp:TextBox runat="server" Width="25" ID="txtHeight" />&nbsp;px &nbsp;
                                <EPiServer:Translate ID="epiTrans2" runat="server" text="/imagelistselector/flashcreator/width" />:&nbsp;
                                <asp:TextBox runat="server" Width="25" ID="txtWidth" />&nbsp;px
                            </td>
                        </tr>
                    </table>
                </fieldset>
                </div>
            </asp:PlaceHolder>
            <div class="epi-formArea">
            <fieldset>
                <legend><EPiServer:Translate runat="server" Text="/imagelistselector/imagecreator/heading" /></legend>
                <table  border="0" style="width: 330px; ">
                    <tr>
                        <td style="width: 250px;">
                            <asp:TextBox ID="txtPageName" runat="server" Text="" ReadOnly="true" BackColor="#dddddd"
                                Width="150" />
                        </td>
                        <td >
                            <!-- adding span tag around buttons to simulate episerver toolbutton look -->
                            <span class="epitoolbutton">
                                <customcontrol:PageSelectorEx ID="PageSelectorEx1" OnClick="PageSelect_Click" runat="server" />
                            </span>
                        </td>
                    </tr>
                </table>
                </fieldset>
                </div>
                <asp:PlaceHolder runat="server" ID="linkPanel" Visible="true">
                <div class="epi-formArea">
                    <fieldset>
                        <legend><EPiServer:Translate runat="server" Text="/imagelistselector/imagecreator/alttext" /></legend>
                        <asp:TextBox runat="server" ID="txtAltText" Width="150" />
                    </fieldset>
                    </div>
                </asp:PlaceHolder>
                <asp:HiddenField ID="ReturnValueXml" runat="server" />
                <div id="LinkTool" class="epi-buttonDefault">
                    <!-- adding span tag around buttons to simulate episerver toolbutton look -->
                    <span class="epi-cmsButton">
                        <img alt="" src='<%= ResolveUrl("~/App_Themes/Default/Images/Tools/Save.gif") %>'/>
                        <asp:Button ID="InsertButton" CssClass="accept" Translate="/imagelistselector/button/insert"
                            runat="server" OnClick="InsertButton_Click" />
                    </span>&nbsp;
                    <!-- adding span tag around buttons to simulate episerver toolbutton look -->
                    <span class="epi-cmsButton">
                        <img alt="" src='<%= ResolveUrl("~/App_Themes/Default/Images/Tools/Cancel.gif") %>'/>
                        <asp:Button ID="CancelButton" CssClass="decline" Translate="/imagelistselector/button/cancel"
                            runat="server" OnClick="CancelButton_Click" />
                    </span>
                </div>
        </div>
    </form>
</body>
</html>
