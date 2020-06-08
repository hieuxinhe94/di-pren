<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AddThisFieldset.ascx.cs" Inherits="PrenDiSe.Templates.Public.Units.CampaignTemplates.AddThisFieldset" %>

<fieldset>
    <legend><%=Headline%></legend>
    <!-- AddThis Button BEGIN -->
    <div class="addthis_toolbox addthis_default_style addthis_32x32_style">
        <div onclick="TrackSocial('facebook','send');"><a class="addthis_button_facebook">Facebook</a></div>
        <div onclick="TrackSocial('twitter','tweet');"><a class="addthis_button_twitter">Twitter</a></div>
        <div onclick="TrackSocial('linkedin','send');"><a class="addthis_button_linkedin">LinkedIn</a></div>
        <div onclick="TrackSocial('share','mail');"><a class="addthis_button_email">E-post</a></div>
    </div>
    <script type="text/javascript">
      var addthis_config = {
        "ui_language": "sv"
      };
      <%=ShareSettingsOutput %>
    </script>
    <script type="text/javascript" src='//s7.addthis.com/js/300/addthis_widget.js#pubid=<%=DIClassLib.Misc.MiscFunctions.GetAppsettingsValue("AddThisKey")%>'></script>
    <script type="text/javascript">
      $(document).ready(function () {
        // Remove all querystring parameters from url to use shen sharing url
        $('a[class*="addthis_button_"]').attr('addthis:url', window.location.origin + window.location.pathname);
      });
    </script>
    <!-- AddThis Button END -->
</fieldset>