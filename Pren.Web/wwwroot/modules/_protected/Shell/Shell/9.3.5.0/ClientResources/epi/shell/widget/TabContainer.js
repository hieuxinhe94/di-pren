//>>built
define("epi/shell/widget/TabContainer",["dojo/_base/array","dojo/_base/declare","dojo/_base/Deferred","dojo/_base/lang","dojo/aspect","dojo/dom-class","dijit/layout/ContentPane","dijit/layout/TabContainer","dijit/registry","epi","epi/dependency","epi/shell/widget/layout/ComponentContainer","epi/shell/widget/dialog/Confirmation","epi/shell/widget/TabController","epi/shell/widget/dialog/Alert","epi/shell/widget/_ActionConsumerWidget","epi/i18n!epi/shell/ui/nls/EPiServer.Shell.UI.Resources.TabContainer"],function(_1,_2,_3,_4,_5,_6,_7,_8,_9,_a,_b,_c,_d,_e,_f,_10,res){return _2([_8,_10],{controllerWidget:_e,_selectedPage:null,_componentsController:null,res:res,confirmationBeforeRemoval:true,startup:function(){if(this._started){return;}this.inherited(arguments);this.connect(this.tablist,"onLayoutChanged",this._changeLayout);this.connect(this.tablist,"onTabTitleChanged",this._changeName);this.connect(this.tablist,"onAddNewTab",this._createTab);this._componentsController=_b.resolve("epi.shell.controller.Components");if(this.getChildren().length<=0){this._createTab();}},postCreate:function(){this.inherited(arguments);_6.add(this.domNode,"epi-mainApplicationArea");},_makeController:function(){var _11=this.inherited(arguments);this.connect(_11,"onSelectChild",this._pageSelected);var _12=[{label:this.res.addgadget,onClick:_4.hitch(this,this._showComponentDialog)},{label:this.res.createtab,onClick:_4.hitch(this,this._createTab)},{label:this.res.rearrangegadget,onChange:_4.hitch(this,this._rearrangeGadgets),type:"checkedmenuitem",checked:false,rearrangeGadgets:function(_13){this.set("checked",_13);}}];_1.forEach(_12,_11.addAddMenuItem,_11);return _11;},_rearrangeGadgets:function(_14){if(this._selectedPage){this._selectedPage.changeLockButtonState(_14);}},_showComponentDialog:function(){if(this._selectedPage){this._selectedPage.showComponentSelector();}},_componentSelected:function(){var _15=this._selectedPage;_15.addComponent.apply(_15,arguments);},_pageSelected:function(_16){this._selectedPage=_16;},_createTab:function(){var _17=this._componentsController.getComponentDefinition(this.id);this._componentsController.getEmptyComponentDefinition("EPiServer.Shell.ViewComposition.Containers.ComponentContainer",_4.hitch(this,function(_18){var _19=_18[0];_19.settings.numberOfColumns=2;_19.settings.containerUnlocked=true;_19.settings.closable=true;this._componentsController.addComponent(this,_19,_4.hitch(this,function(_1a){var _1b=this.getChildren();this.selectChild(_1b[_1b.length-1]);}));}));},_changeName:function(_1c,_1d){if(this._isEmpty(_1d)){this._showMessageDialog(this.res.tabnamecannotbeemptymessage).then(function(_1e){if(_1e){_1c.controlButton.tabName.innerHTML=_1c.controlButton.tabCurrentName;_1c.controlButton._setTabTitleEditable(true);}});}else{var _1f=String(_1c.title);var _20=this._componentsController.getComponentDefinition(_1c.id);_1c.title=_1d;_1c.tabCurrentName=_1d;if(_20!=null){_20.settings.personalizableHeading=_1d;this._componentsController.saveComponent(_20,function(){},_4.hitch(this,function(){this._showMessageDialog(this.res.tabnamecouldnotbesaved).then(function(_21){if(_21){_1c.set("title",_1f);_1c.set("tabCurrentName",_1f);_1c.controlButton.revertTabTitleChanges(true,_1f,_1d);}});}));}else{}}},_getNumberOfColumns:function(evt){if(!evt||!evt.target){return 1;}var _22=_9.getEnclosingWidget(evt.target);if(!_22||!_22.column){return 1;}return _22.column;},_changeLayout:function(_23,evt){var _24=this._getNumberOfColumns(evt),_25=null;if(typeof _23.setColumns=="function"){_23.setColumns(_24);}_25=this._componentsController.getComponentDefinition(_23.id);if(_25!=null){_25.settings.numberOfColumns=_24;this._componentsController.saveComponent(_25);}else{}},closeChild:function(_26){if(this.getChildren().length==1){return;}if(!this.confirmationBeforeRemoval){this.inherited(arguments);this._removeTab(_26);}else{return this._showRemovalConfirmationDialog(_4.hitch(this,function(_27){if(_27){_8.prototype.closeChild.apply(this,[_26]);this._removeTab(_26);}}));}},_removeTab:function(_28){var _29=this._componentsController.getComponentDefinition(_28.id);this._componentsController.removeComponent(_29.id);var _2a=this.getChildren().length;if(_2a==1){for(var _2b in this.tablist.pane2button){this.tablist.pane2button[_2b].closeTabMenuItem.set("disabled",true);}}},_showRemovalConfirmationDialog:function(_2c){var _2d=new _d({description:this.res.removetabquestion,title:_a.resources.header.episerver,onAction:_2c});_2d.show();},_showMessageDialog:function(_2e){var _2f=new _3();var _30=new _f({description:_2e});_30.show();_5.after(_30,"destroy",function(){_2f.resolve(true);});return _2f;},_isEmpty:function(_31){return (_4.trim(_31)===""?true:false);}});});