//>>built
define("epi/string",["dojo/_base/lang","dojo/dom-construct","dojox/html/entities"],function(_1,_2,_3){return {pascalToCamel:function(_4){var _5=_4.split(".");var _6=this._toCamelCase(_5[0]);for(var i=1;i<_5.length;i++){_6+="."+this._toCamelCase(_5[i]);}return _6;},_toCamelCase:function(_7){if(!_7){return "";}if(_7[0]===_7[0].toLowerCase()){return _7;}var _8="";for(var i=0;i<_7.length;i++){var _9=(i+1<_7.length);if((i===0||!_9)||_7[i+1]===_7[i+1].toUpperCase()){_8+=_7[i].toLowerCase();}else{_8+=_7.substring(i);break;}}return _8;},stripHtmlTags:function(_a){if(this.isNullOrEmpty(_a)){return "";}var _b=_2.toDom(_a.replace(/&/gim,"&amp;"));return this.removeAllCarriageReturns(_b.textContent||_b.innerText);},removeAllCarriageReturns:function(_c){if(this.isNullOrEmpty(_c)){return "";}var _d=_c.replace(/\t|\r|\n/gim,"");return _d.replace(/ {2,}/gim," ");},encodeForWebString:function(_e,_f){if(this.isNullOrEmpty(_e)){return "";}var _10=_3.encode(_e);if(_f==null||_f.length===0){return _10;}var _11=new RegExp("&lt;(/?("+_f.join("|")+")/?\\s*)&gt;","gim");return _10.replace(_11,"<$1>");},isNullOrEmpty:function(_12){return !_12||(/^\s*$/).test(_12);},toHTML:function(_13){if(this.isNullOrEmpty(_13)){return "";}return _13.replace(/\r\n|\n|\r|\\n/g,"<br/>");},toTooltipText:function(_14){if(this.isNullOrEmpty(_14)){return "";}return _14.replace(/\\n/gim,"\n");},slashName:function(_15){return _15.replace(/\./g,"/");},appendTrailingSlash:function(url){return url.replace(/\/?$/,"/");},truncateMiddle:function(_16,_17,_18){if(typeof _16==="string"){_16=_16.split(/\s+/g);}else{_16=_16.concat();}if(_16.join("").length<=_17){return _16;}var _19=function(_1a,_1b,_1c){if(_1c){return _19(_1a.reverse(),_1b).reverse();}var _1d=0;return _1a.filter(function(_1e){_1d+=_1e.length;return _1d<=_1b;});};var _1f=Math.ceil(_17/2);var _20=Math.floor(_17/2);var _21=_19(_16,_1f);_18=_18||"&hellip;";_21.push(_18);return _21.concat(_19(_16,_20,true));}};});