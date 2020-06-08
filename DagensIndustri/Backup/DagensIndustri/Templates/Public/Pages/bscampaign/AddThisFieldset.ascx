<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddThisFieldset.ascx.cs" Inherits="DagensIndustri.Templates.Public.Pages.bscampaign.AddThisFieldset" %>

<fieldset>
    <legend>TIPSA EN VÄN OM DET HÄR ERBJUDANDET</legend>
    <!-- AddThis Button BEGIN -->
    <div class="addthis_toolbox addthis_default_style addthis_32x32_style">
        <div onclick="TrackSocial('facebook','send');"><a class="addthis_button_facebook">Facebook</a></div>
        <div onclick="TrackSocial('twitter','tweet');"><a class="addthis_button_twitter">Twitter</a></div>
        <div onclick="TrackSocial('share','mail');"><a class="addthis_button_email">E-post</a></div>
    </div>
    <script type="text/javascript">
        var addthis_config = {
            "ui_language": "sv"
        };
    </script>
    <script type="text/javascript" src='//s7.addthis.com/js/300/addthis_widget.js#pubid=<%=DIClassLib.Misc.MiscFunctions.GetAppsettingsValue("AddThisKey")%>'></script>
    <!-- AddThis Button END -->
</fieldset>