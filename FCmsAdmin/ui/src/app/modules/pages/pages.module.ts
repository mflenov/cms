import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';

import { PageListComponent } from './page-list/page-list.component';
import { StructureComponent } from './structure/structure.component';
import { ContentdefinitionComponent } from './structure/content-definition.component';
import { EditpageComponent } from './editpage/edit-page.component';
import { ContentItemComponent } from './editpage/content-item.component';
import { ContentItemEditorComponent } from './editpage/content-item-editor.component';
import { ContentPlaceholderDirective } from './editpage/content-placeholder.directive';
import { FiltersComponent } from './editpage/filters.component';
import { EditFiltersComponent } from './editpage/edit-filters.component';
import { TextFilterEditorComponent } from './editpage/filter-controls/text-filter-editor.component';
import { BoolFilterEditorComponent } from './editpage/filter-controls/bool-filter-editor.component';
import { DaterangeFilterEditorComponent } from './editpage/filter-controls/daterange-filter-editor.component';
import { NewPageComponent } from './newpage/new-page.component';
import { ListPageContentComponent } from './list-content/list-page-content.component';

@NgModule({
  declarations: [
    PageListComponent,
    StructureComponent,
    ContentdefinitionComponent,
    EditpageComponent,
    ContentItemComponent,
    ContentItemEditorComponent,
    ContentPlaceholderDirective,
    FiltersComponent,
    EditFiltersComponent,
    TextFilterEditorComponent,
    BoolFilterEditorComponent,
    DaterangeFilterEditorComponent,
    NewPageComponent,
    ListPageContentComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(
      [
        { path: 'pages', component: PageListComponent },
        { path: 'pages/add', component: NewPageComponent },
        { path: 'pages/structure/:id', component: StructureComponent },
        { path: 'pages/edit/:id', component: EditpageComponent },
        { path: 'pages/list/:repo/:id', component: ListPageContentComponent }
      ]
    )
  ]
})
export class PagesModule { }