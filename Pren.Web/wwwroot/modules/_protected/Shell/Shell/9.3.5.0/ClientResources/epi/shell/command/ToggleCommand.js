//>>built
define("epi/shell/command/ToggleCommand",["dojo/_base/declare","epi/shell/DestroyableByKey","epi/shell/command/_Command"],function(_1,_2,_3){return _1([_3,_2],{property:null,active:false,_execute:function(){var _4=this.property,_5=!this.active;this.model.set(_4,_5);this.set("active",_5);},_onModelChange:function(){var _6=this;var _7=this.model,_8=_7&&_7[this.property],_9=typeof _8=="boolean";this.set("canExecute",_9);this.set("active",_9&&_8);if(_7){this.destroyByKey("modelPropertyWatcher");this.ownByKey("modelPropertyWatcher",_7.watch(this.property,function(_a,_b,_c){_6.set("active",!!_c);}));}}});});