class FolderRepo {
    typedropdown = '.fcms #folderconfigurator [data-object="filtertypedropdown"]';
    folderdefinition = '.fcms #folderconfigurator [data-object="folderdefinition"]'
    addvaluebutton = '.fcms #folderconfigurator [data-object="addvalue"]'
    numbderoffvalues = '.fcms #folderconfigurator #numbderoffvalues';
    folderitem = '.fcms #folderconfigurator .folderitem';
    valuelist = '.fcms #folderconfigurator [data-object="values-list"]';

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
        $(this.folderdefinition).hide();
    }

    public showValueList() {
        $(this.folderdefinition).show();
    }

    public addvalue() {
        let lastindex: number = 0;

        $(this.folderitem).each(function () {
            var value = parseFloat($(this).attr('data-index'));
            lastindex = (value > lastindex) ? value : lastindex;
        });

        let self = this;
        lastindex++;

        $.post("/fcmsmanager/definition/addchild",
            { contenttype: "String", index: lastindex },
            function (data) {
                $(self.valuelist).append(data);
            });

        $(this.numbderoffvalues).val(lastindex);
    }

    public processForm() {
        $(this.typedropdown).change(this.change.bind(this));
        $(this.addvaluebutton).click(this.addvalue.bind(this));
    }
}

let folderRepo = new FolderRepo();
folderRepo.processForm();