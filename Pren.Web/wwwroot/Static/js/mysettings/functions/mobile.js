define([
    "jquery",
    "lib/dragend",
    "func/shortcut",
    "dom/misc"],
    function ($, dragend, shortcut, dom) {
        return new MobileDisplayer(dom, dragend, shortcut);
    });

function MobileDisplayer(dom, dragend, shortcut) {
    this.dom = dom;
    this.Dragend = dragend;
    this.shortcut = shortcut;
}

// To check if mobile device, we check if an element with class "hidden-xs" is visible or hidden. 
// So it's the css that decides if it's a small device or not.
MobileDisplayer.prototype.setUpMobile = function () {
    var self = this;    
    var active = false;
    var draggable = null;

    if (isSmallDevice() && !active) {
        createDraggable();
        active = true;
    }

    $(window).resize(function () {
        if (isSmallDevice() && !active) {
            createDraggable();
            active = true;
        } else if (!isSmallDevice() && active) {
            destroyDraggable();
            active = false;        
        }
    });    

    // Shortcuts must be initialized after creation and destroying of draggable.
    // This because Dragend messes with the dom disables the shortcuts events
    function createDraggable() {
        draggable = new self.Dragend(document.getElementById("drag-container"), {
            scribe: "100px;",
            afterInitialize: function () {
                this.container.style.visibility = "visible";
            }
        });
        self.shortcut.init();
    }

    function destroyDraggable() {
        draggable.destroy();
        self.shortcut.init();
    }

    function isSmallDevice() {
        return $("#mobileCheck").is(":hidden");
    }

}