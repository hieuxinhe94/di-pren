
function downloadPDF(issueId,appendix)
{
    window.open('/Tools/Operations/Stream/DownloadPDF.aspx?appendix=' + appendix + '&issueid=' + issueId ,'Download','width=300,height=200,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=Yes,copyhistory=no');
}

function downloadTomorrowPDF(issueId, appendix, tomorrow) {
    window.open('/Tools/Operations/Stream/DownloadPDF.aspx?appendix=' + appendix + '&issueid=' + issueId + '&tomorrow=' + tomorrow, 'Download', 'width=300,height=200,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=Yes,copyhistory=no');
}

function ContactPopUp(custompageid) 
{
    window.open('/Functions/SendMail.aspx?custompageid=' + custompageid, 'Kontakt', 'width=300,height=350,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no');
}

function sendGasellLinkMail(conferenceid,custompageid)
{
    window.open('/Functions/SendMail.aspx?type=gasell&custompageid=' + custompageid + "&conferenceid=" + conferenceid, 'Gasell', 'width=300,height=350,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no');
}

function DiNatetOpenPopup(url)
{
	mainWinParams = 'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=750,height=500';
	var newWindow = window.open(url, 'di_article', mainWinParams);
}

function jsShowHideDiv(divName) {
    var ele = document.getElementById(divName);
    if (ele.style.visibility == 'visible') {
        ele.style.visibility = 'hidden';
        ele.style.display = 'none';
    }
    else {
        ele.style.visibility = 'visible';
        ele.style.display = 'block';
    }

    return false;
}

/* -------------------------------------------------------------
 * Function for creating basic XMLHttpRequest connection object
 * -------------------------------------------------------------
*/
function httpConnect() {
    // Mozilla and other standard compliant browsers
    if (window.XMLHttpRequest) { 
        http_request = new XMLHttpRequest();
    }
    // Internet Explorer
    else if (window.ActiveXObject) {
        try {
            http_request = new ActiveXObject("Msxml2.XMLHTTP.4.0");
        }
        catch (e) {
            
            try {
                http_request = new ActiveXObject("Microsoft.XMLHTTP");
            } 
            catch (e) {}
        }
    }
    // Check if connection object is created
    if (!http_request) {
        alert('Ett tillfälligt fel uppstod.');
        return false;
    }
    else {
        return http_request;
    }
}