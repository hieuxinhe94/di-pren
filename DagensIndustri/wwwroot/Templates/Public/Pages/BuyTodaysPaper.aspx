<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="BuyTodaysPaper.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.BuyTodaysPaper" %>
<%@ Register TagPrefix="public" TagName="MainBody" Src="~/Templates/Public/Units/Placeable/OldMainBody.ascx" %>


<asp:Content ID="Content4" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <public:MainBody ID="MainBody1" runat="server" />   
    
    <asp:Literal runat="server" ID="LitMessage"></asp:Literal>
    
    <asp:PlaceHolder runat="server" ID="PhSmsArea">
    
        <p>
            <EPiServer:Translate ID="Translate1" runat="server" Text="/sms/heading1" />
            <strong>
                <EPiServer:Property ID="Property1" runat="server" PropertyName="SMSprefix" />&nbsp;
                <asp:Literal id="litSmsCode" runat="server"></asp:Literal>
            </strong>
        </p>
        <p>
            <EPiServer:Translate ID="Translate2" runat="server" Text="/sms/heading2" />&nbsp;
            <EPiServer:Property ID="Property2" runat="server" CssClass="bold" PropertyName="SMSnumber" />
        </p>

        <div id="divMessage" class="error paddingB10 underline"></div>

        <div class="bold">
            <asp:Image ID="ImageAnimation" runat="server" ImageUrl="/Templates/DI/Images/ajax-loader.gif" />&nbsp;
            <EPiServer:Translate ID="Translate3" runat="server" Text="/sms/wait" />
        </div>
    					               							    				    						           
        <EPiServer:Property ID="Property3" runat="server" PropertyName="FooterEditor" />

        <script type="text/javascript">

            var i = 0;
            var request;

            function checkPayedStatus() {
                i++;
                var url = '/Functions/HiddenSMS.aspx?id=' + i;
                request = null;
                request = new httpConnect();
                request.open("GET", url, true);
                var returnString = '';

                request.setRequestHeader("Cache-Control", "no-cache");
                request.setRequestHeader("If-Modified-Since", "Wed, 31 Dec 1980 00:00:00 GMT");
                request.setRequestHeader("Expires", "Wed, 31 Dec 1980 00:00:00 GMT");
                request.setRequestHeader("Connection", "close");

                request.onreadystatechange = function () {
                    if (request.readyState == 4)      //4 = "loaded"
                    {
                        if (request.status == 200)    //200 = "OK"
                        {
                            returnString = request.responseText;

                            if (returnString.toLowerCase() == "yes") {
                                top.location.href = "http://dagensindustri.se/";
                            }
                            else {
                                if (i >= 36)   //36 = 3 minutes has passed (36 times * 5 sec)
                                {
                                    document.getElementById('divMessage').innerHTML = '<%=Translate("/sms/timeout") %>';
                                }
                            }
                        }
                        else {
                            document.getElementById('divMessage').innerHTML = '<%=Translate("/sms/error") %>';
                        }
                    }
                }

                request.send(null);
            }


            //run method with interval...
            setInterval('checkPayedStatus()', 5000);

        </script>	    
    	
	</asp:PlaceHolder>

</asp:Content>

