define([
    "dojo/_base/connect",
    "dojo/_base/declare",
    "dojo/store/Memory",

    "dijit/_CssStateMixin",
    "dijit/_Widget",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/FilteringSelect",
    "dijit/TitlePane",

    "epi/dependency",
    "epi/epi",
    "epi/shell/widget/_ValueRequiredMixin",
    "epi/shell/widget/dialog/Dialog",
    "epi/cms/widget/_HasChildDialogMixin",

    "pren/editors/CampaignSelectorDialog",
    "dojo/text!./templates/CampaignSelector.html"
],
function (
    connect,
    declare,
    Memory,

    _CssStateMixin,
    _Widget,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,
    FilteringSelect,
    TitlePane,

    dependency,
    epi,
    _ValueRequiredMixin,
    Dialog,
    _HasChildDialogMixin,

    CampaignSelectorDialog,
    template
) {

    return declare("pren.editors.CampaignSelector", [_Widget, _TemplatedMixin, _WidgetsInTemplateMixin, _CssStateMixin, _ValueRequiredMixin, _HasChildDialogMixin], {

        templateString: template,
        baseClass: "epiStringList",
        intermediateChanges: false,
        postCreate: function () {
            this.inherited(arguments);

            this.connect(this.btnSelectCampaign, "onClick", this.openSelectCampaignDialog);
            this.connect(this.txtSelectedCampaign, "onChange", this._updateValue);

            this.set('value', this.value);
        },
        openSelectCampaignDialog : function() {
            if (this.dialog) {
                this.dialog.destroy();
                this.CampaignSelectorDialog.destroy();
            }

            this.createDialog();
            this.dialog.show(true);
            this.isShowingChildDialog = true;
        },
        createDialog: function () {
            this.CampaignSelectorDialog = new CampaignSelectorDialog({
                selectedCampaign: this.txtSelectedCampaign.get('value')
            });

            this.dialog = new Dialog({
                title: "Välj kampanj",
                content: this.CampaignSelectorDialog,
                dialogClass: "epi-dialog-portrait"
            });

            this.connect(this.dialog, 'onExecute', '_onExecute');
            this.connect(this.dialog, 'onHide', '_onDialogHide');

            this.dialog.startup();            
        },
        _onDialogHide: function () {
            this.focus();
        },
        _onExecute: function () {
            this.isShowingChildDialog = false;
            this._setValueAttr(this.CampaignSelectorDialog.get("value"));
        },
        _setValueAttr: function (value) {
            this._set('value', value);
            this.txtSelectedCampaign.set("value", value);
        },
        _updateValue: function (value) {
            this._set('value', value);
            this.onChange(value);
        }
    });
});