//>>built
require({cache:{"url:epi/shell/widget/templates/GlobalMenu.html":"<div>\r\n    <div data-dojo-attach-point=\"accordionNode\" class=\"epi-navigation-accordioncontainer\">\r\n        <div data-dojo-attach-point=\"containerNode\" class=\"${hiddenClass}\" style=\"position:absolute; bottom:0; left:0; width:100%\"></div>\r\n    </div>\r\n    <div data-dojo-attach-point=\"expandCollapseContainer\" class=\"epi-navigation-expandcollapseContainer\">\r\n        <a data-dojo-attach-point=\"expandCollapseButton\" href=\"javascript:void(0)\" class=\"epi-navigation-expandcollapseBtn\"\r\n            tabindex=\"0\"><span class=\"epi-navigation-expandcollapseIcon\"></span></a>\r\n    </div>\r\n</div>\r\n"}});define("epi/shell/widget/GlobalMenu",["dojo/_base/declare","dojo/_base/lang","dojo/_base/event","dojo/NodeList-traverse","dojo/dom-attr","dojo/dom-style","dojo/dom-class","dojo/dom-geometry","dojo/query","dojo/topic","dojo/text!./templates/GlobalMenu.html","dijit/focus","dijit/_Container","dijit/_TemplatedMixin","dijit/_Widget","epi/Url","epi/i18n!epi/shell/ui/nls/EPiServer.Shell.UI.Resources.GlobalMenu","dojo/has","dgrid/util/has-css3"],function(_1,_2,_3,_4,_5,_6,_7,_8,_9,_a,_b,_c,_d,_e,_f,Url,_10,has){return _1([_f,_e,_d],{res:_10,templateString:_b,_focusNode:null,_siteUrl:null,openClass:"epi-globalNavigation--open",hoverClass:"epi-globalNavigation--hover",shadowClass:"epi-globalNavigation--shadow",hiddenClass:"dijitHidden",postCreate:function(){this.inherited(arguments);this._focusNode=_9(".epi-navigation-selected > a",this.containerNode)[0];this.connect(this,"onBlur",function(){this._toggleGlobalSearch(false);this._hideMenu();});this.connect(this.domNode,"onmouseenter",this._toggleHintOn);this.connect(this.domNode,"onmouseleave",this._toggleHintOff);this.connect(this.accordionNode,"onclick",this._showMenu);this.connect(this.expandCollapseContainer,"onclick",this._toggleMenu);this._hideMenu();_a.subscribe("/epi/shell/context/changed",_2.hitch(this,"_onContextChanged"));if(has("css-transitions")){this.connect(this.accordionNode,has("transitionend"),_2.hitch(this,function(){if(this._isOpen()){_c.focus(this._focusNode);return;}if(_7.contains(this.accordionNode,this.hoverClass)){return;}_7.add(this.containerNode,this.hiddenClass);}));}},_getContentHeight:function(){return _8.getContentBox(this.containerNode).h;},_toggleMenu:function(e){if(this._isOpen()){this._hideMenu();}else{this._showMenu();}_3.stop(e);},_toggleHintOn:function(){if(this._isOpen()){return;}_7.add(this.accordionNode,this.hoverClass);},_toggleHintOff:function(){if(this._isOpen()){return;}_7.remove(this.accordionNode,this.hoverClass);},_showMenu:function(){_7.remove(this.accordionNode,this.hoverClass);_7.add(this.accordionNode,this.openClass);_7.remove(this.containerNode,this.hiddenClass);_6.set(this.accordionNode,"height",this._getContentHeight()+"px");_7.add(this.domNode,this.shadowClass);},_hideMenu:function(){_7.remove(this.accordionNode,this.hoverClass);_7.remove(this.accordionNode,this.openClass);_7.remove(this.domNode,this.shadowClass);_6.set(this.accordionNode,"height","0px");},_isOpen:function(){return _7.contains(this.accordionNode,this.openClass);},_getToViewModeElement:function(){var _11=_9(".epi-navigation-container-utils",this.element)[0];return _9(".epi-navigation-global_sites.epi-navigation-currentSite.epi-navigation-iconic",_11)[0];},_onContextChanged:function(ctx,_12){var _13=this._getToViewModeElement();if(!_13){return;}var url=this._buildPublicUrl(ctx,_13);var _14=_2.replace(this.res.toviewmode,[url.toString()]);_5.set(_13,{href:url.path,title:_14});},_buildPublicUrl:function(ctx,_15){if(this._siteUrl===null){this._siteUrl=_15.href;}var _16=new Url(ctx.publicUrl||this._siteUrl);var _17=window.location;var _18={scheme:_17.protocol,authority:_17.host,path:_16.path};var url=new Url(null,_18,true);return url;},_toggleGlobalSearch:function(_19){var _1a=_9("#epi-searchContainer")[0];if(_1a){_6.set(_1a,"display",_19?"block":"none");}}});});