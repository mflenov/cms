class ContentFilter {
    addButton = '.fcms #cmsfilterlist [data-object="addfilterbutton"]';
    filterSelect = '.fcms #cmsfilterlist [data-object="filter-select"]';
    filterList = '.fcms #cmsfilterlist [data-object="filters-list"]';
    filterValue = '.fcms #cmsfilterlist .cmsfiltervalue';
    numbderoffilters = '.fcms #cmsfilterlist #numbderoffilters';

    loadFilter(event: any) {
        let filtertype = $(this.filterSelect).val();
        let lastindex: number = 0;

        $(this.filterValue).each(function () {
            var value = parseFloat($(this).attr('data-index'));
            lastindex = (value > lastindex) ? value : lastindex;
        });

        let self = this;
        $.post("/fcmsmanager/pagecontent/filter",
            { filterid: filtertype, index: lastindex + 1 },
            function (data) {
                $(self.filterList).append(data);
            });

        $(this.numbderoffilters).val(lastindex + 1);
    }

    public processForm() {
        $(this.addButton).click(this.loadFilter.bind(this));
    }
}


let contentFilter = new ContentFilter();
contentFilter.processForm();