<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMail.aspx.cs" MasterPageFile="~/Templates/Public/MasterPages/CampaignMaster.Master" Inherits="DagensIndustri.Tools.Operations.Articles.SendMail" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Templates/Public/Units/Placeable/InputWithValidation.ascx" %>

<asp:Content ContentPlaceHolderID="CampaignRegion" runat="server">

    <div id="MainBodyArea">            
        <%--Send article--%>
        <asp:PlaceHolder runat="server" ID="PhSendArticleForm" Visible="false">    
            <div class="paddingT10 bold">
                <EPiServer:Translate runat="server" Text="/mail/sendarticle/heading" />
            </div>
            <div class="EjBrodText">
                <p>
                    <DI:Input runat="server" id="InputToEmail" HeadingTranslateKey="/mail/sendarticle/recipientemailheading" Width="210" MaxLength="50" Validate="true" ValidationGroup="form3" RegularExpressionValidate="true" />
                </p>
                <p>
                    <DI:Input runat="server" id="InputFromEmail" HeadingTranslateKey="/mail/sendarticle/emailheading" Width="210" MaxLength="50" Validate="true" ValidationGroup="form3" RegularExpressionValidate="true" />
                </p>
                <p>
                    <DI:Input runat="server" ID="InputToMessage" HeadingTranslateKey="/mail/sendarticle/messageheading" Rows="8" Width="270px" TextMode="MultiLine" />     
                </p>                        
                <asp:Button runat="server" Text="Skicka" ValidationGroup="form3" OnClick="SendArticleMail"  />  
            </div>    
        </asp:PlaceHolder>
        
        <asp:PlaceHolder runat="server" ID="PhMessage" Visible="false">
            <div class="paddingT10">
                <asp:Label runat="server" ID="LblMessage" />
                <p>
                    <a href="javascript:void();" onclick="javascript:history.back(-1)"><EPiServer:Translate runat="server" Text="/various/back" /></a>&nbsp;
                    <a href="javascript:void();" onclick="javascript:window.close()"><EPiServer:Translate runat="server" Text="/various/close" /></a>
                </p>
            </div>
        </asp:PlaceHolder>        
    </div>   
</asp:Content>

