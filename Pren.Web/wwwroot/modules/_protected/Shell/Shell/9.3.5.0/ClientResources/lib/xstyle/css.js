//>>built
define("xstyle/css",["require"],function(_1){"use strict";function _2(_3,id,_4){var _5=document.documentElement;var _6=_5.insertBefore(document.createElement(_3),_5.firstChild);_6.id=id;var _7=(_6.currentStyle||getComputedStyle(_6,null))[_4];_5.removeChild(_6);return _7;};return {load:function(_8,_9,_a,_b){var _c=_9.toUrl(_8);var _d=_9.cache["url:"+_c];if(_d){if(_d.xCss){var _e=_d.parser;var _f=_d.xCss;_d=_d.cssText;}_1(["./util/createStyleSheet"],function(_10){_10(_d);});if(_f){}return _11();}
function _11(){var _12=_2("x-parse",null,"content");
if(_12&&_12!="none"&& _12 != "normal"){_9([eval(_12)],_a);}
else{_a();}};
var _13=_2("div",_8.replace(/\//g,"-").replace(/\..*/,"")+"-loaded","display");if(_13=="none"){return _11();}_1(["./load-css"],function(_14){_14(_c,_11);});}};});