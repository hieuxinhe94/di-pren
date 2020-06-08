<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoogleTranslateBox.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.GoogleTranslateBox" %>


<div style="margin-bottom:3px;">
<b>Click here to translate this webpage</b>
</div>

<div id="google_translate_element"></div>
<script type="text/javascript">
    function googleTranslateElementInit() {
        new google.translate.TranslateElement({ pageLanguage: 'sv', includedLanguages: 'en' }, 'google_translate_element');
    }
</script>
<script type="text/javascript" src="//translate.google.com/translate_a/element.js?cb=googleTranslateElementInit"></script>

<div style="margin-top:8px;">
<i>Please note that translation is performed using Google Translate, grammatical errors and incorrect translations may occur.</i>
</div>

<br />
<br />