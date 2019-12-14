class ContentFilter {
    constructor() {
        this.addButton = '.fcms #cmsfilterlist [data-object="addfilterbutton"]';
        this.filterSelect = '.fcms #cmsfilterlist [data-object="filter-select"]';
        this.filterList = '.fcms #cmsfilterlist [data-object="filters-list"]';
        this.filterValue = '.fcms #cmsfilterlist .cmsfiltervalue';
        this.numbderoffilters = '.fcms #cmsfilterlist #numbderoffilters';
    }
    loadFilter(event) {
        let filtertype = $(this.filterSelect).val();
        let lastindex = 0;
        $(this.filterValue).each(function () {
            var value = parseFloat($(this).attr('data-index'));
            lastindex = (value > lastindex) ? value : lastindex;
        });
        let self = this;
        $.post("/fcmsmanager/content/filter", { filterid: filtertype, index: lastindex + 1 }, function (data) {
            $(self.filterList).append(data);
        });
        $(this.numbderoffilters).val(lastindex + 1);
    }
    processForm() {
        $(this.addButton).click(this.loadFilter.bind(this));
    }
}
let contentFilter = new ContentFilter();
contentFilter.processForm();
//# sourceMappingURL=ContentEditor.js.map