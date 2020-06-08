function openWindowNoAddress(page_name, page_width, page_height) {
    mainWinParams = 'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=' + page_width + ',height=' + page_height;
    var newWindow = window.open("about:blank", page_name, mainWinParams);
    newWindow.focus();
    return newWindow;
}

function openWindowNoAddress(page_url, page_name, page_width, page_height) {
    mainWinParams = 'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=' + page_width + ',height=' + page_height;
    var newWindow = window.open(page_url, page_name, mainWinParams);
    newWindow.focus();
    return newWindow;
}

function openTopLeftWindowNoAddress(page_url, page_name, page_width, page_height) {
    if (screen.width) {
        var winl = (screen.width - page_width) / 2;
        var wint = (screen.height - page_height) / 2;
    }
    else {
        winl = 0;
        wint = 0;
    }

    if (winl < 0)
        winl = 0;
    if (wint < 0)
        wint = 0;

    var settings = 'height=' + page_height + ',';
    settings += 'width=' + page_width + ',';
    settings += 'top=' + 0 + ',';
    settings += 'left=' + 0 + ',';
    settings += 'scrollbars=1,resizable=1';
    win = window.open(page_url, page_name, settings);
    win.window.focus();
}

function openCenterWindowNoAddress(page_url, page_name, page_width, page_height) {
    if (screen.width) {
        var winl = (screen.width - page_width) / 2;
        var wint = (screen.height - page_height) / 2;
    }
    else {
        winl = 0;
        wint = 0;
    }

    if (winl < 0)
        winl = 0;
    if (wint < 0)
        wint = 0;

    var settings = 'height=' + page_height + ',';
    settings += 'width=' + page_width + ',';
    settings += 'top=' + wint + ',';
    settings += 'left=' + winl + ',';
    settings += 'scrollbars=1,resizable=1';
    win = window.open(page_url, page_name, settings);
    win.window.focus();
}
function sendDIArticle(fileID) {
    //alert(fileID)
    try {
        window.open('/Tools/Operations/Articles/SendMail.aspx?FileID=' + fileID, 'Skicka', 'width=300,height=350,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no');
    }
    catch (e) { }
}