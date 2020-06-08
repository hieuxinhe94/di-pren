define(["jquery", "bootstrap", "dom/misc"], function ($, bootstrap, dom) {    
    return new Loader(dom);
});

function Loader(dom) {
    this.dom = dom;

    this.dom.loadModal.modal({
        keyboard: false,
        backdrop: 'static'
    });
}

Loader.prototype.closeLoader = function() {
    this.dom.loadModal.modal('hide');
}