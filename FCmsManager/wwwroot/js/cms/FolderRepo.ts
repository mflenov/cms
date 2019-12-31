class FolderRepo {
    typedropdown = '.fcms #filterconfigurator [data-object="filtertypedropdown"]';
    valuetype = '.fcms #filterconfigurator [data-object="valuetype"]'
    addvaluebutton = '.fcms #filterconfigurator [data-object="addvalue"]'
    numbderoffilters = '.fcms #filterconfigurator #numbderoffilters';
    filtervalue = '.fcms #filterconfigurator .filtervalue';
    valuelist = '.fcms #filterconfigurator [data-object="values-list"]';

    itemtemplate = '<input type="text" name="Values" data-index="{index}" class="form-control filtervalue" placeholder="Value" />';

    public change(event: any) {
        let type = $(event.target).val();
        if (type == "Folder") {
            this.showValueList();
        }
        else {
            this.hideValueList();
        }
    }

    public hideValueList() {
        $(this.valuetype).hide();
    }

    public showValueList() {
        $(this.valuetype).show();
    }

    public addvalue() {
        let lastindex: number = 0;

        $(this.filtervalue).each(function () {
            var value = parseFloat($(this).attr('data-index'));
            lastindex = (value > lastindex) ? value : lastindex;
        });

        let self = this;
        lastindex++;
        $(self.valuelist).append(this.itemtemplate.replace(/{index}/g, lastindex.toString()));

        $(this.numbderoffilters).val(lastindex);
    }

    public processForm() {
        $(this.typedropdown).change(this.change.bind(this));
        $(this.addvaluebutton).click(this.addvalue.bind(this));
    }
}

let folderRepo = new FolderRepo();
folderRepo.processForm();