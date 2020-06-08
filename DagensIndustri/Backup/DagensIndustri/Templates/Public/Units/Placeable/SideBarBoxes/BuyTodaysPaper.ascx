<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyTodaysPaper.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.BuyTodaysPaper" %>

<!-- Infobox --> 
<div class="infobox"> 
	<div class="wrapper"> 
		<h2>
            <EPiServer:Translate Text="/sms/read" runat="server" />
        </h2> 
		<div class="content"> 
			<p> 
				<EPiServer:Translate Text="/sms/subscriber" runat="server" /> <%--<a href="#"><EPiServer:Translate Text="/sms/loggedin" runat="server" /></a>--%>
			</p> 
			<p> 
				<EPiServer:Translate Text="/sms/nosubscriber" runat="server" /><br /><br />
				<EPiServer:Translate Text="/sms/pdfnumber" runat="server" /> <asp:Literal ID="SMSCodeLitral" runat="server"/><br /> 
				<EPiServer:Translate Text="/sms/sendsms" runat="server" /><br /> 
							
				<small>
                    <asp:Literal ID="SMSDescriptionLiteral" runat="server" />
				</small> 
			</p> 

            <div id="divMessage" style="color:Red;"></div>
		</div> 
	</div> 
</div>

<script type="text/javascript">

    var i = 0;
    var request;

    function checkPayedStatus() {
        i++;

        if (i >= 30)  //5 min - stop making http requests
            return;

        var url = '/Tools/Operations/HiddenSMS.aspx?id=' + i;
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
                        top.location.href = '<%= smsLandingPageURL %>';
                    }
                    else {
                        if (i >= 18)   //3 min
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
    setInterval('checkPayedStatus()', 10000);

</script>	  
<!-- // Infobox -->	