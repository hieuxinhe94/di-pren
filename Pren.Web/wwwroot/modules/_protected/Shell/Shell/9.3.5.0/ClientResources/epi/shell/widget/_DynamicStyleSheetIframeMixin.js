//>>built
define("epi/shell/widget/_DynamicStyleSheetIframeMixin",["dojo/_base/declare","dijit/Destroyable","epi/shell/DynamicStyleSheet"],function(_1,_2,_3){return _1([_2],{cssRules:null,postMixInProperties:function(){this.inherited(arguments);this.cssRules=[];},postCreate:function(){this.inherited(arguments);this.own(this.watch("isLoading",this._isLoadingHandler));this._setup();},destroy:function(){if(this._css){this._css.destroy();}this.inherited(arguments);},addCssRules:function(_4){var _5=_4 instanceof Array;_5?this.cssRules=this.cssRules.concat(_4):this.cssRules.push(_4);if(this._css){_5?this._css.addRules(_4):this._css.addRule(_4);}},_isLoadingHandler:function(_6,_7,_8){if(_8){if(this._css){this._css.destroy();}}else{this._setup();}},_setup:function(){if(this.isInspectable()){this._css=new _3({doc:this.getDocument(),rules:this.cssRules});}}});});