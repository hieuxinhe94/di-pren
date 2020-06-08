//>>built
define("epi/shell/dnd/tree/dndSource",["dojo/_base/array","dojo/_base/connect","dojo/_base/declare","dojo/dom-class","dojo/dom-geometry","dojo/_base/lang","dojo/on","dojo/touch","dojo/topic","dojo/dnd/Manager","dojo/when","dijit/tree/dndSource","../_DndDataMixin","epi/shell/TypeDescriptorManager"],function(_1,_2,_3,_4,_5,_6,on,_7,_8,_9,_a,_b,_c,_d){return _3([_b,_c],{generateText:false,singular:true,constructor:function(){var _e=this.reject;if(_e){var _f=[];for(var key in this.accept){_f.push(key);}this.accept={};_f=_d.removeIntersectingTypes(_f,this.reject);_1.forEach(_f,function(_10){this.accept[_10]=1;},this);for(var i=0;i<_e.length;++i){this.accept[_e[i]]=0;}}},deleteSelectedNodes:function(){},userSelect:function(_11){this.inherited(arguments,[_11,false,false]);},onDndDrop:function(_12,_13,_14,_15){if(this.containerState=="Over"){var _16=this.tree,_17=_16.model,_18=this.targetAnchor;this.isDragging=false;var _19;var _1a=(_18&&_18.item)||_16.item;if(this.dropPosition=="Before"||this.dropPosition=="After"){_1a=(_18.getParent()&&_18.getParent().item)||_16.item;_19=_18.getIndexInParent();if(this.dropPosition=="After"){_19=_18.getIndexInParent()+1;}}var _1b=this.itemCreator(_13,_18.rowNode,_12,_15);if(_12==this){if(_17.pasteItems){_17.pasteItems(_1b,_1a,_14,_19);}else{_1.forEach(_1b,function(_1c){if(_1c.dndData){var _1d=_1c.dndData.options;var _1e=_1d?_1d.oldParentItem:null;var _1f=_1d?_1d.indexInParent:0;if(typeof _19=="number"){if(_1a==_1e&&_1f<_19){_19-=1;}}_17.pasteItem(_1c.dndData.data,_1e,_1a,_14,_19);}});}}else{if(_17.newItems){_17.newItems(_1b,_1a,_19);}else{_1.forEach(_1b,function(_20){_17.newItem(_20,_1a,_19);});}}if(!_12.isDragging&&!_15.isDragging){this.onDndEnd();}}if(_12!==_15&&!_14&&_12.onDndItemRemoved){var _21=_1.map(_13,function(_22){return this._getDndData(_12.getItem(_22.id),this.accept,this===_12);},this);_12.onDndItemRemoved(_21,_12,_13,_14,_15);}this.onDndCancel();},onMouseOut:function(){this.inherited(arguments);if(this._expandOnDndTimeout){clearTimeout(this._expandOnDndTimeout);}},_onDragMouse:function(e,_23){this.inherited(arguments);var m=_9.manager();if(m.canDropFlag){_a(this.checkItemAcceptance(this.current.rowNode,m.source,null),function(_24){m.canDrop(_24);});}if(this._expandOnDndTimeout){clearTimeout(this._expandOnDndTimeout);}if(this.tree&&this.tree.expandOnDnd===true&&this.targetAnchor){this._expandOnDndTimeout=setTimeout(_6.hitch(this,function(){this.isDragging&&this.tree._expandNode(this.targetAnchor);}),500);}},onDndEnd:function(){},onDndItemRemoved:function(_25,_26,_27,_28,_29){},itemCreator:function(_2a,_2b,_2c,_2d){return _1.map(_2a,function(_2e){return {id:_2e.id,name:_2e.textContent||_2e.innerText||"",dndData:this._getDndData(_2c.getItem(_2e.id),this.accept,this===_2c)};},this);},getItem:function(key){var _2f=this.inherited(arguments);if(this.tree.getItemType&&_2f&&_2f.data&&_2f.data.item){_2f.type=this.tree.getItemType(_2f.data.item);}_2f.options=this.getItemOptions(_2f);return _2f;},getItemOptions:function(_30){var _31={};if(_30.data){if(_30.data.getParent){var _32=_30.data.getParent();_31.oldParentItem=_32?_32.item:null;}if(_30.data.getIndexInParent){_31.indexInParent=_30.data.getIndexInParent();}}return _31;},onMouseDown:function(e){var _33=_4.contains(e.target,"dojoDndContainerOver");if(_33){return;}this.inherited(arguments);},checkAcceptance:function(_34,_35){if(this.readOnly){return false;}var _36=_1.map(_35,function(_37){return _34.getItem(_37.id);});return this._checkAcceptanceForItems(_36,this.accept);}});});