<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CompetitionAddThis.ascx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Competition.CompetitionAddThis" %>

<div class="tip">
    <div class="header small"><EPiServer:Property ID="Property3" runat="server" PropertyName="TipHeaderStep2" /></div>
    <!-- AddThis Button BEGIN -->
    <div class="addthis_toolbox addthis_default_style addthis_32x32_style">
        <a class="addthis_button_facebook" style="cursor:pointer"></a>
        <a class="addthis_button_twitter" style="cursor:pointer"></a>
        <a class="addthis_button_email" style="cursor:pointer"></a>
    </div>
    <script type="text/javascript">
        var addthis_config = {
            "ui_language": "sv"
        };
        </script>
    <script type="text/javascript" src='//s7.addthis.com/js/300/addthis_widget.js#pubid=<%=DIClassLib.Misc.MiscFunctions.GetAppsettingsValue("AddThisKey")%>'></script>
    <!-- AddThis Button END -->                    
</div>   