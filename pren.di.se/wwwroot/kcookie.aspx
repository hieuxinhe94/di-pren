<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kcookie.aspx.cs" Inherits="PrenDiSe.kcookie" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <script type="text/javascript" src="/bootstrapDi/js/jquery.1.9.1.js"></script>
</head>
    
<body id="mainBody" runat="server">
    <form id="form1" runat="server">
        Test
        
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
    </form>
</body>

</html>