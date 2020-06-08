<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AddThis.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.AddThis" %>

<asp:PlaceHolder runat="server" ID="PhAddthis">

<div class="infobox">
	<div class="wrapper">
        <h2>
            <EPiServer:Property runat="server" PropertyName="Heading" />
        </h2>
        <div class="content">


            <div class="tip">
                <!-- AddThis Button BEGIN -->
                <div class="addthis_toolbox addthis_default_style addthis_32x32_style">
                    <asp:PlaceHolder runat="server" Visible='<%# IsValue("ShowFacebookShare") %>'>
                        <a class="addthis_button_facebook"></a>    
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" Visible='<%# IsValue("ShowTwitterShare") %>'>
                        <a class="addthis_button_twitter"</a>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" Visible='<%# IsValue("ShowMailShare") %>'>
                        <a class="addthis_button_email"></a>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" Visible='<%# IsValue("ShowFacebookLike") %>'>
                        <a class="addthis_button_facebook_like"></a>
                    </asp:PlaceHolder>
                </div>
                <script type="text/javascript">
                    var addthis_config = {
                        "ui_language": "sv"
                    };
                    </script>
                <script type="text/javascript" src='//s7.addthis.com/js/300/addthis_widget.js#pubid=<%=DIClassLib.Misc.MiscFunctions.GetAppsettingsValue("AddThisKey")%>'></script>
                <!-- AddThis Button END -->                    
            </div>   

        </div>
    </div>
</div>  

</asp:PlaceHolder>