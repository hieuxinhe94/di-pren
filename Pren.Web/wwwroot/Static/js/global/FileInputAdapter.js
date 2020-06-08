var FileInputAdapter = function (fileFieldElement, fileFieldElementsShadow) {
    
    this.fileFieldElement = fileFieldElement;
    this.fileFieldElementsShadow = fileFieldElementsShadow;

    this.init();
}

FileInputAdapter.prototype.init = function ()
{
    var ieVersion = msieversion();
    if (ieVersion > 0 && ieVersion < 10) {
        // IE does not support shadow elements triggering click on input file due to security. 
        // It empties the input on form submit. 
        this.fileFieldElement.show();
        this.fileFieldElementsShadow.hide();
        return;
    }

    var self = this;
    this.fileFieldElementsShadow.on('click', function (e) { self._handleClick(e); });
    this.fileFieldElement.on('change', function (e) { self._handleChange(e, this); });
    this.fileFieldElement.hide();
}

FileInputAdapter.prototype._handleClick = function (e) {   
    e.preventDefault();
    this.fileFieldElement.trigger('click');
}


FileInputAdapter.prototype._handleChange = function (e, element) {
    var fileName = $(element).val().replace(/.*(\/|\\)/, '');
    this.fileFieldElementsShadow[0].value = fileName;
}


