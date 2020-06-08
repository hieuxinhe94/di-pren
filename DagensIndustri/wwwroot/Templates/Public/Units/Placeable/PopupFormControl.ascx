<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="PopupFormControl.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.PopupFormControl" %>
<%@ Register TagPrefix="di" TagName="xform" Src="~/Templates/Public/Units/Placeable/XForm.ascx" %>
<%@ Import Namespace="EPiServer" %>
<%@ Import Namespace="EPiServer.Core" %>
<%@ Import Namespace="EPiServer.Web" %>




<style type="text/css">    
    .modalwrapper{background-color:#000;height: 100%;opacity: 0.5;position: fixed;top: 0;width: 100%;z-index: 1;filter:alpha(opacity=50);-moz-opacity:0.5;-khtml-opacity: 0.5;}    
    .btnred{color:#FFF;cursor:pointer;padding:5px;background-color:#AA1714;border-radius:3px 3px 3px 3px;-moz-border-radius: 3px; -webkit-border-radius: 3px; border:1px solid #812421;}        
    #popupBox{display:none;background: #F7EAE4;position: fixed;top: 50%; left: 50%;z-index: 1000;box-shadow: 0px 0px 20px #999; -moz-box-shadow: 0px 0px 20px #999;-webkit-box-shadow: 0px 0px 20px #999; border-radius:3px 3px 3px 3px;-moz-border-radius: 3px; -webkit-border-radius: 3px; /*min-width:400px;*/padding:20px;}        
    #popupBox .thankyoumessage{color:#AA1714;font-weight:bold;}    
    #popupBox .cw{float:right;}
    #popupBox a.close{color:#FFF;background-color:#AA1714;text-decoration:none;padding:2px 4px;border-radius:3px 3px 3px 3px;-moz-border-radius: 3px; -webkit-border-radius: 3px; }
    #popupBox label{margin:0 5px 0 0;font-weight:bold;}	
    #popupBox input[type=text] {margin-bottom:10px;width:86%;-webkit-box-sizing: border-box; -moz-box-sizing: border-box; box-sizing: border-box;border: 1px solid #E2DFDF;border-radius: 3px 3px 3px 3px;box-shadow: 1px 1px 2px rgba(0, 0, 0, 0.5) inset;color: #444444;font-family: 'Arial',Sans-serif;font-size: 14px;line-height: 16px;padding:5px 6px;	           }	
</style>

<script type="text/javascript">
    
    var events = 'load mousemove click mouseup mousedown keydown keypress keyup submit change mouseenter scroll resize dblclick';
    $(window).bind(events, someEvent);
    var timer = null;
    var show = false;
    var delay = <%= SettingsPage["TimerInSeconds"]%>;
            
    function someEvent()
    {
        if (timer) 
            clearTimeout(timer);
        timer = setTimeout(function(t) { showPopUp(false); }, delay*1000);
    }

    function showOnPanelLoad(){
        if(show)
        {
            if($("#popupBox .thankyoumessage").length){
                $("#popupBox .thankyoumessage").text('<%= SettingsPage["ThankYouText"]%>');
            }
            showPopUp();
        }
    }

    function showPopUp() {
        show = true;
        $(window).unbind(events, someEvent);

        if(!$(".modalwrapper").length){
            $('body').append('<div class="modalwrapper"></div>');
        }
        $('.modalwrapper').fadeIn(500);
                    
        //Fade in the Popup
        var popupBox = "#popupBox";
        $(popupBox).fadeIn(500);

        //Set the center alignment
        $(popupBox).css({ 
            'margin-top' : -($(popupBox).height() + 24) / 2,
            'margin-left' : -($(popupBox).width() + 24) / 2
        });
    }
            
    function doClose() {        
        $('.modalwrapper , #popupBox').fadeOut(300, function() {
            $('.modalwrapper').remove();                        
        });
        $("#popupBox").remove();
        $(window).unbind(events, someEvent);
    }

    
</script>
         

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <script type="text/javascript">
            $(document).ready(function () {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(showOnPanelLoad);             
            });
        </script>   
        <div id="popupBox" class="popupForm">  
            <div class="cw"> 
                <a href="#" class="close" onclick="doClose()">&#10006;</a>
            </div>
            <div id="popup-inner">
                <EPiServer:Property ID="PropBody" runat="server" PropertyName="BodyText" />
                <asp:Panel runat="server" ID="XFormPanel">
                    <di:xform ID="Xform1" ShowStatistics="true" XFormProperty="XForm" runat="server" />
                </asp:Panel>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>