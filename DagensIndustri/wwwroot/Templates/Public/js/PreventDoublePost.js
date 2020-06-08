
function jsPreventDoublePost(submitBtnDivId, formSentDivId) {

    var btn = document.getElementById(submitBtnDivId);
    var info = document.getElementById(formSentDivId);

    btn.style.visibility = 'hidden';
    btn.style.display = 'none';

    info.style.visibility = 'visible';
    info.style.display = 'block';

    return true;
}