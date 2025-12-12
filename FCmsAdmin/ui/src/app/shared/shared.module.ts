import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { NgxTinymceModule } from 'ngx-tinymce';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { SlidePanelComponent } from './slideout/slideout-panel.component';
import { StructureComponent } from './structure/structure.component';
import { ContentdefinitionComponent } from './structure/content-definition.component';
import { ToastComponent } from './toast/toast.component';
import { EditContentComponent } from './editcontent/edit-content.component';
import { ContentItemComponent } from './editcontent/content-item.component';
import { ContentItemEditorComponent } from './editcontent/content-item-editor.component';
import { ContentPlaceholderDirective } from './widgets/content-placeholder.directive';
import { EditFiltersComponent } from './editcontent/edit-filters.component';
import { TextFilterEditorComponent } from './editcontent/filter-controls/text-filter-editor.component';
import { BoolFilterEditorComponent } from './editcontent/filter-controls/bool-filter-editor.component';
import { DaterangeFilterEditorComponent } from './editcontent/filter-controls/daterange-filter-editor.component';
import { DateFilterEditorComponent } from './editcontent/filter-controls/date-filter-editor.component';
import { ValuelistFilterEditorComponent } from './editcontent/filter-controls/valuelist-filter-editor.component';
import { RegexFilterEditorComponent } from './editcontent/filter-controls/regex-filter-editor.component';
import { FiltersComponent } from './widgets/filters.component';


@NgModule({ declarations: [
        SlidePanelComponent,
        StructureComponent,
        EditContentComponent,
        ContentdefinitionComponent,
        ToastComponent,
        ContentItemComponent,
        ContentItemEditorComponent,
        ContentPlaceholderDirective,
        EditFiltersComponent,
        TextFilterEditorComponent,
        BoolFilterEditorComponent,
        DaterangeFilterEditorComponent,
        DateFilterEditorComponent,
        ValuelistFilterEditorComponent,
        RegexFilterEditorComponent,
        FiltersComponent,
    ],
    exports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        SlidePanelComponent,
        StructureComponent,
        EditContentComponent,
        ToastComponent,
        FiltersComponent,
        ContentPlaceholderDirective
    ], 
    imports: [
        FormsModule,
        EditorModule,
        NgbModule,
        RouterModule,
        NgxTinymceModule.forRoot({ baseURL: '/tinymce/' }),
        CommonModule,
        BrowserAnimationsModule
    ], 
    providers: [
        provideHttpClient(withInterceptorsFromDi()),
        { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' }
    ] 
})
export class SharedModule { }
