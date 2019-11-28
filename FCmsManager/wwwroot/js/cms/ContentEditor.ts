class ContentFilter {
    filterType: JQuery;
    filtersList: JQuery;
    repositoryid: String;

    loadFilter(event: any) {
        let filtertype = $('.cmsfilterlist [data-object="filter-select"]').val();
        let lastindex: number = 0;

        $('.cmsfilterlist .cmsfiltervalue').each(function () {
            var value = parseFloat($(this).attr('data-index'));
            lastindex = (value > lastindex) ? value : lastindex;
        });

        $.post("/fcmsmanager/content/filter",
            { filterid: filtertype, index: lastindex + 1 },
            function (data) {
                $('.cmsfilterlist [data-object="filters-list"]').append(data);
            });

        $('.fcms #numbderoffilters').val(lastindex + 1);
    }


    public processForm() {
        $('.cmsfilterlist [data-object="addfilterbutton"]').on('click', this.loadFilter);
    }
}


let contentFilter = new ContentFilter();
contentFilter.processForm();