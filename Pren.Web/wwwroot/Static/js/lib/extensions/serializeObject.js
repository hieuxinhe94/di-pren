define(["jquery"], function($) {

    $.fn.serializeObject = function(alwaysAsArrayElementName) {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function() {
            if (o[this.name] ) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
                if (this.name === alwaysAsArrayElementName) {
                    o[this.name] = [o[this.name]];
                } 
            }
        });
        return o;
    };

});