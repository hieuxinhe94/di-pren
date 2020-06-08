
function GetLinkListAsHtml(xmlDoc, isJsLink) {

    var html = "";
    var x = xmlDoc.getElementsByTagName("NewsItem");
    for (i = 0; i < x.length; i++) {
        html += "<div class='date'>" + FormatDate(x[i].getElementsByTagName("DateId")[0].childNodes[0].nodeValue) + "</div>";
        html += "<div class='headline'>" + x[i].getElementsByTagName("HeadLine")[0].childNodes[0].nodeValue + "</div>";

        var intro = x[i].getElementsByTagName("block")[0].childNodes[0];
        if (intro != null)
            html += "<div class='intro'>" + GetShortIntro(intro.nodeValue) + "</div>";

        if(isJsLink == true)
            html += "<a href='javascript:void(0);' onclick='PrintSelectedItem(" + x[i].getElementsByTagName("NewsItemId")[0].childNodes[0].nodeValue + ")'>Läs mer</a>";
        else
            html += "<a href='RssBig.aspx?id=" + x[i].getElementsByTagName("NewsItemId")[0].childNodes[0].nodeValue + "'>Läs mer</a>";
           
        html += "<hr>";
    }

    //alert(xmlDoc.getElementsByTagName("NewsItemId")[0].childNodes[0].nodeValue);
    //html += x[i].getElementsByTagName("block")[1].childNodes[0].nodeValue);
    //html += x[i].getElementsByTagName("body")[0].childNodes[0].nodeValue);
    //html += x[i].getElementsByTagName("NewsItemId")[0].childNodes[0].nodeValue);
    //alert(i);

    return html;
}



function GetXmlDoc() {
    
    if (window.XMLHttpRequest) {
        xmlhttp = new XMLHttpRequest();
    }
    else {
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    xmlhttp.open("GET", "Data/DataLatest.xml", false);
    xmlhttp.send();
    xmlDoc = xmlhttp.responseXML;

    return xmlDoc;
}

//20120207T103458+0100 ==> 2012-02-07 10:34:58
function FormatDate(str) {
    if (str.length == 20)
        return str.substring(0, 4) + "-" + str.substring(4, 6) + "-" + str.substring(6, 8) + " " + str.substring(9, 11) + ":" + str.substring(11, 13) + ":" + str.substring(13, 15);

    return "";
}

function GetShortIntro(str) {
    if (str.length > 100)
        return str.substring(0, 100) + "...";

    return str;
}