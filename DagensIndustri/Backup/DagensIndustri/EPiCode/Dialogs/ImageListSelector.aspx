<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ImageListSelector.aspx.cs" Inherits="EPiCode.ImageList.Dialogs.ImageListSelector" %>
<%@ Register TagPrefix="customcontrol" Namespace="EPiCode.ImageList.Dialogs" Assembly="EPiCode.ImageListProperty" %>
<%@ Register TagPrefix="EPiServer" Namespace="EPiServer.Web.WebControls" Assembly="EPiServer.Web.WebControls" %>
<%@ Import Namespace="EPiServer.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <base target="_self" />
    <title>Image List Selection</title>
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/App_Themes/Default/styles/system.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/App_Themes/Default/styles/ToolButton.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/styles/ImageListStyle.css") %>" />
<%--    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/EPiServer/Shell/ClientResources/Epi/Shell/Light/Shell.css") %>" />--%>
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/EPiServer/Shell/ClientResources/Shell.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/EPiServer/Shell/ClientResources/ShellLightTheme.css") %>" />
<%--    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/EPiServer/Shell/ClientResources/Epi/Shell/Shell.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/EPiServer/CMS/ClientResources/Epi/Base/CMS.css") %>" />--%>
    <script type='text/javascript' src="<%= EPiServer.UriSupport.ResolveUrlFromUtilBySettings("javascript/episerverscriptmanager.js") %>"></script>
    <script type='text/javascript' src="<%= EPiServer.UriSupport.ResolveUrlFromUIBySettings("javascript/system.js") %>" ></script>
    <script type='text/javascript' src="<%= EPiServer.UriSupport.ResolveUrlFromUIBySettings("javascript/system.aspx") %>" ></script>

    <script type="text/javascript">
    
   // Global variables
    var isPostBack = false;
    var showDebugMessages = false;
    var hiddenEditedLinkTag;
    var hiddenEditedLinkTagIndex;
    
    function debugWrite(msg)
    {
        if (showDebugMessages)
            alert(msg);
    }
    
    // Safe way to have many onload functions. EPiServer might add
    // it's own, we don't want to overwrite those
    function addLoadEvent(func) {
      var oldonload = window.onload;
      if (typeof window.onload != 'function') {
        window.onload = func;
      } else {
        window.onload = function() {
          if (oldonload) {
            oldonload();
          }
          func();
        }
      }
    }

    // Called when the page is loaded, we
    // only want to do this the first time, 
    // not on subsequent post back. 
    // During the first load (not a postback)
    // we'll set the global javascript var
    // isPostBack to false, and true for all
    // postbacks after that
    function LoadedPage()
    {
		hiddenEditedLinkTag = document.getElementById("EditedLinkTag");    
        hiddenEditedLinkTagIndex = document.getElementById("EditedLinkTagIndex");
		if (!isPostBack)
        {
            var InitialXmlCtrl = document.getElementById("InitialXml");
            if(EPi.GetDialog().dialogArguments)
                InitialXmlCtrl.value = EPi.GetDialog().dialogArguments;
            else
                InitialXmlCtrl.value = '';
            __doPostBack("", "");
        }        
    }   
    
    function SelectLink(postbackCtrlId, type)
    {
        EditLink(postbackCtrlId, "" ,"", "", "", "", "", "", "", type, -1);        
    }    
    
    function EditLink(postbackCtrlId, imgUrl ,imgAlt, imgHeight, imgWidth, linkUrl, linkName, linkToolTip, linkTarget, type, index)
    {                
        // Pass the page or parent page id to the popup
        var pageId              = '<%= Page.Request.QueryString["pageid"] ?? "0" %>';
        var parentPageId        = '<%= Page.Request.QueryString["parentId"] ?? "0" %>';
        var lang                = '<%= Page.Request.QueryString["epspagelanguagebranch"] ?? "en" %>';
        
        // The file manager needs the folder id in order to show the "Page Files" directory. If not, it will always
	    // show the folder for the start page. Note! in 4.61 there is a bug that require the page to be saved and viewed
	    // before the "Page Files" can be accessed (it will show the files for the start page instead).
	    var folderId = '<%= Page.Request.QueryString["folderId"] ?? Page.Request.QueryString["pageid"] %>';
    
        var url = '/EPiCode/Dialogs/LinkedImageCreator.aspx?pageid=' + pageId + '&parentId=' + parentPageId + '&epspagelanguagebranch=' + lang;
        url += '&folderId=' + folderId;
        url += '&imgurl=' + imgUrl;
        url += '&imgalt=' + imgAlt;
        url += '&imgheight=' + imgHeight;
        url += '&imgwidth=' + imgWidth;
        url += '&linkurl=' + linkUrl;
        url += '&linkname=' + linkName;
        url += '&linktip=' + linkToolTip;
        url += '&linktarget=' + linkTarget;
        url += '&type=' + type;	    

        var linkAttributes			            = new Object();		
	    linkAttributes.imageDialogUrl			= "";
	    linkAttributes.resizeDialogUrl			= "";
	    linkAttributes.parentWindow				= window;
	    linkAttributes.fileManagerBrowserUrl    = '<%= EPiServer.UriSupport.ResolveUrlFromUIBySettings("edit/FileManagerBrowser.aspx") %>?id=' + pageId + '&parent=' + parentPageId + '&folderid=' + folderId;
        
	    var dialogArguments = linkAttributes;
	    var features = {width: 460, height: 412};
	    var callbackArguments = new Object();
	    callbackArguments.postbackCtrlId = postbackCtrlId;
	    callbackArguments.index = index;
	    callbackArguments.callingLanguageBranch = lang;	    
    
		/* opens the hyperlinkproperty dialog */       
	    EPi.CreateDialog(
	            url,
	            OnEPiDialogClosed,
	            callbackArguments,
	            dialogArguments,
	            features);
    }    
    
     /* function called after dialog has returned and closed*/
    function OnEPiDialogClosed(returnValue, callbackArguments) {
        debugger;	
        if (returnValue != undefined && returnValue != 0 && returnValue != -1)
	    {	      
	        var imgTag = returnValue;
	       
            if(callbackArguments.index>=0)
            {   				
                // save values to update existing links     
                hiddenEditedLinkTag.value = imgTag;
                hiddenEditedLinkTagIndex.value = callbackArguments.index;                 
            }
            __doPostBack(callbackArguments.postbackCtrlId, imgTag);
	    }
    }
    
    // User hit Save button
    function OnCloseAndSave(xmlInputCtrlId, numberOfLinks)
    {
        var xmlInputCtrl  = document.getElementById(xmlInputCtrlId);
        var linkReturnValues = new Object();
        
        linkReturnValues.isOk = true;
        linkReturnValues.isClear = false;
		linkReturnValues.linkCount = numberOfLinks;
        //replacing '(apostrophe) with valid ASCII code 
        var xmlVal = xmlInputCtrl.value;
        if (xmlVal.indexOf("'") > -1)
            xmlVal = xmlVal.replace("'", "&#39;"); 
        linkReturnValues.xml = xmlVal;
        
        EPi.GetDialog().Close(linkReturnValues);
    }    

    function OnCloseAndCancel(numberOfLinks)
    {
        var linkReturnValues = new Object();
        
        linkReturnValues.isOk = false;
        linkReturnValues.isClear = false;
        linkReturnValues.linkCount = numberOfLinks;
        linkReturnValues.xml = "";
        
        EPi.GetDialog().Close(linkReturnValues);
    }
    
    // Global Calls
    
    // Add load event handler
    addLoadEvent(LoadedPage);
    </script>

</head>
<body>
    <form id="LinkImage" enctype="multipart/form-data" method="post" runat="server">
        <div class="epi-contentContainer epi-padding" style="padding-left: 5px; overflow: hidden;">
        <div class="epi-contentArea"> 
            <h1>				
                <EPiServer:Translate runat="server" text="/imagelistselector/pagetext/title" />
            </h1>
            <asp:Literal runat="server" ID="ltrIntro" />
            <EPiServer:Translate runat="server" text="/imagelistselector/pagetext/introduction" />
            </div>
            <!-- adding span tag around buttons to simulate episerver toolbutton look -->    
            <div class="epi-buttonDefault">
	    
	         <span class="epi-cmsButton">
                <customcontrol:LinkItemSelector ID="LinkItemSelectControl" runat="server" OnClick="LinkedImageSelect_Click" Text='<%# LanguageManager.Instance.TranslateFallback("/imagelistselector/pagetext/addlinkedimage", "...")  %>' Type="image" />
             </span> &nbsp;
             <span class="epi-cmsButton" style="display:none;">
                <customcontrol:LinkItemSelector ID="FlashSelectorControl" runat="server" OnClick="FlashSelect_Click" Text='<%# LanguageManager.Instance.TranslateFallback("/imagelistselector/pagetext/addflash", "...")  %>' Type="flash" /></span>
                </div>
            <asp:Repeater ID="PageListRepeater" runat="server" OnItemDataBound="PageListRepeater_ItemDataBound" OnItemCommand="PageListRepeater_ItemCommand">
                <HeaderTemplate>
                    <table class="epi-default" cellspacing="0" border="0">
                        <tr>
							<th  colspan="2">
								&nbsp;
							</th>
							<th  nowrap>
                                <EPiServer:Translate runat="server" text="/imagelistselector/columnheadings/edit" />
                            </th>
                            <th  nowrap>
                                <EPiServer:Translate runat="server" text="/imagelistselector/columnheadings/moveup" />
                            </th>
                            <th  nowrap>
                                <EPiServer:Translate runat="server" text="/imagelistselector/columnheadings/movedown" />
                            </th>
                            <th >
                                <EPiServer:Translate runat="server" text="/imagelistselector/columnheadings/delete" />
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td  width="21px">
                            <img runat="server" src="" id="previewImage" border="0" height="20" width="20" alt="" />
                        </td>
                        <td  style="width: 475px;" >                            
                            <asp:Literal ID="PageNameLiteral" runat="server" />
                            <div><asp:Literal ID="LinkInfo" runat="server" /></div>
                            <asp:Literal ID="ImageNameLiteral" runat="server" />                            
                            <asp:Literal ID="ImageAltText" runat="server" />                            
                        </td>
                        <td  align="center">
							<span class="epitoolbutton">
								<customcontrol:LinkItemSelector ID="EditLinkEx" runat="server" Text='<%# LanguageManager.Instance.TranslateFallback("/imagelistselector/pagetext/edit", "...")  %>' Type='<%# (bool)DataBinder.Eval(Container, "DataItem.ImageLink.IsFlash") ? "flash" : "image" %>' />
							</span>
                        </td>
                        <td  align="center">
                            <asp:ImageButton ID="MoveUpButton" runat="server" ImageUrl='<%# ResolveUrl("~/App_Themes/Default/Images/Tools/Up.gif") %>' CommandName="MoveUp" CommandArgument="<%# Container.ItemIndex %>" /></td>
                        <td  align="center">
                            <asp:ImageButton ID="MoveDownButton" runat="server" ImageUrl='<%# ResolveUrl("~/App_Themes/Default/Images/Tools/Down.gif") %>' AlternateText="Moves the link down one position in the list" CommandName="MoveDown" CommandArgument="<%# Container.ItemIndex %>" />
                        </td>                        
                        <td  align="center">
                            <asp:ImageButton ID="DeleteButton" runat="server" ImageUrl='<%# ResolveUrl("~/App_Themes/Default/Images/Tools/Delete.gif") %>' AlternateText="Removes the link from the list" CommandName="Delete" CommandArgument="<%# Container.ItemIndex %>" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        
		    <asp:HiddenField ID="EditedLinkTag" runat="server" Value="" />        
            <asp:HiddenField ID="EditedLinkTagIndex" runat="server" Value="-1" /> 
            <asp:HiddenField ID="ReturnValueXml" runat="server" />
            <asp:HiddenField ID="InitialXml" runat="server" EnableViewState="false" />
            

            <div id="LinkToolButtonArea" class="epi-buttonDefault"> 
                <!-- adding span tag around buttons to simulate episerver toolbutton look -->    
                <span class="epi-cmsButton">
                    <img alt="" src='<%= ResolveUrl("~/App_Themes/Default/Images/Tools/Save.gif") %>' />
                    <asp:Button ID="InsertButton" CssClass="accept" Translate="/imagelistselector/button/insert"
                        runat="server" OnClick="InsertButton_Click" />
                </span> &nbsp;   
                
                <span class="epi-cmsButton">
                    <img alt="" src='<%= ResolveUrl("~/App_Themes/Default/Images/Tools/Cancel.gif") %>' />
                    <asp:Button ID="CancelButton" CssClass="decline" Translate="/imagelistselector/button/cancel" 
                        runat="server" OnClick="CancelButton_Click" />
                </span>        
            </div>
        </div>
    </form>
</body>
</html>
