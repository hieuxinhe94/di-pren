<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kcookie.aspx.cs"  MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master"  Inherits="DagensIndustri.kcookie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <script type='text/javascript'>
(function ($) {

    var cookieNameConf = 'disableMaintPopup',
        cookieExpDaysConf = 30;
     
        var parseBool = function (value) {
	        if (typeof value !== 'string') return false;
	        return (value.toLowerCase() === 'true');
        };

        var getCookie = function (cookieName) {
	        var name,
	        cookie,
	        cookies = document.cookie.split(';');

	        for (var i = 0, len = cookies.length; i != len; i += 1) {
	          cookie = cookies[i];

	          name = $.trim(cookie.substr(0, cookie.indexOf('=')));
	          if (name === cookieName) {
		        return $.trim(unescape(cookie.substr(cookie.indexOf('=') + 1)));
	          }
	        }

	        return null;
        };

        var setCookie = function (cookieName, value, expDays) {
	        var expDate = new Date();
	        expDate.setDate(expDate.getDate() + expDays);

	        var cookieValue = escape(value)
	          + (expDays == null ? '' : '; expires=' + expDate.toUTCString());
	        document.cookie = cookieName + '=' + cookieValue;
        };

    $(document).ready(function() {
            alert(parseBool(getCookie(cookieNameConf)));
            //if (isValidDay() && !parseBool(getCookie(cookieNameConf))) {
            setCookie(cookieNameConf, 'true', cookieExpDaysConf);
            alert('Kakan är nu satt');
            //}
        }
    );
})(jQuery);
</script>
</asp:Content>