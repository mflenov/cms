class Filter {
    constructor() {
        this.typedropdown = '.fcms #filterconfigurator [data-object="filtertypedropdown"]';
        this.valuetype = '.fcms #filterconfigurator [data-object="valuetype"]';
        this.addvaluebutton = '.fcms #filterconfigurator [data-object="addvalue"]';
        this.numbderoffilters = '.fcms #filterconfigurator #numbderoffilters';
        this.filtervalue = '.fcms #filterconfigurator .filtervalue';
        this.valuelist = '.fcms #filterconfigurator [data-object="values-list"]';
        this.itemtemplate = '<input type="text" name="Values" data-index="{index}" class="form-control filtervalue" placeholder="Value" />';
    }
    change(event) {
        let type = $(event.target).val();
        if (type == "ValueList") {
            this.showValueList();
        }
        else {
            this.hideValueList();
        }
    }
    hideValueList() {
        $(this.valuetype).hide();
    }
    showValueList() {
        $(this.valuetype).show();
    }
    addvalue() {
        let lastindex = 0;
        $(this.filtervalue).each(function () {
            var value = parseFloat($(this).attr('data-index'));
            lastindex = (value > lastindex) ? value : lastindex;
        });
        let self = this;
        lastindex++;
        $(self.valuelist).append(this.itemtemplate.replace(/{index}/g, lastindex.toString()));
        $(this.numbderoffilters).val(lastindex);
    }
    processForm() {
        $(this.typedropdown).change(this.change.bind(this));
        $(this.addvaluebutton).click(this.addvalue.bind(this));
    }
}
let filter = new Filter();
filter.processForm();
//# sourceMappingURL=Filter.js.map