//>>built
define("epi/shell/widget/_SectionedTemplatedMixin",["dojo/_base/declare","dojo/query","dojo/string","dijit/_TemplatedMixin"],function(_1,_2,_3,_4){return _1([_4],{sections:[],_fillContent:function(_5){this.sections.forEach(function(_6){var _7=this[_6],_8=_2(_3.substitute("[data-epi-section=${0}]",[_6]),_5);_8.forEach(function(_9){while(_9.childNodes.length>0){_7.appendChild(_9.childNodes[0]);}});},this);}});});