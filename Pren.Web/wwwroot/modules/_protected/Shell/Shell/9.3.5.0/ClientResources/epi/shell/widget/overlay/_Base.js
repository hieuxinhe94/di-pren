//>>built
define("epi/shell/widget/overlay/_Base",["dojo/_base/declare","dojo/dom-geometry","dojo/dom-construct","dojo/dom-style","dojo/has","dijit/_Widget"],function(_1,_2,_3,_4,_5,_6){return _1([_6],{disabled:false,sourceItemNode:null,position:null,parent:null,adjustForMargins:true,onResize:function(){},destroy:function(){this.sourceItemNode=null;this.parent=null;this.inherited(arguments);},refresh:function(){this._reset();this.resize();this.onResize();},resize:function(_7){_7=_7||this.get("position");this.updatePosition(_7);},updatePosition:function(_8){if(!this.canUpdatePosition()){return false;}_4.set(this.domNode,{left:_8.x+"px",top:_8.y+"px",width:_8.w+"px",height:_8.h+"px"});if(_8.y<1&&_8.w<1){_4.set(this.domNode,{display:"none"});}else{_4.set(this.domNode,{display:""});}return false;},_getPositionAttr:function(){if(!this.canUpdatePosition()){return {x:0,y:0,w:0,h:0};}var _9=_2.position(this.sourceItemNode,false),_a=this._rawPosition,_b,_c,_d;if(_a){if(_5("ie")){_b=Math.abs((_a.x-_9.x))>1||Math.abs((_a.y-_9.y))>1||Math.abs((_a.w-_9.w))>1||Math.abs((_a.h-_9.h))>1;}else{_b=_a.x!==_9.x||_a.y!==_9.y||_a.w!==_9.w||_a.h!==_9.h;}if(!_b){this.position.isChanged=false;return this.position;}}this._rawPosition={x:_9.x,y:_9.y,w:_9.w,h:_9.h};if(this.adjustForMargins){_c=_4.getComputedStyle(this.sourceItemNode);if(_c){_d=_2.getMarginExtents(this.sourceItemNode,_c);if(_d.l<0){_9.x-=_d.l;_9.w+=_d.l;}if(_d.r<0){_9.w-=_d.r;}if(_d.t<0){_9.x-=_d.t;_9.h+=_d.t;}if(_d.b<0){_9.h-=_d.b;}}}_9.isChanged=true;this.set("position",_9);return _9;},_reset:function(){this._clearPosition();},_clearPosition:function(){this.position=this._rawPosition=null;},_setDisabledAttr:function(_e){this._set("disabled",_e);_4.set(this.domNode,{display:_e?"none":""});},_setSourceItemNodeAttr:function(_f){this._clearPosition();this._set("sourceItemNode",_f);},canUpdatePosition:function(){if(this.disabled){return false;}try{if(!this.sourceItemNode||!this.sourceItemNode.ownerDocument){return false;}}catch(ex){return false;}return true;}});});