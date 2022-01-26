import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';

import { PageListComponent } from './page-list/page-list.component';
import { StructureComponent } from './structure/structure.component';
import { ContentdefinitionComponent } from './structure/content-definition.component';
import { EditpageComponent } from './editpage/edit-page.component';
import { PageContentComponent } from './editpage/page-content.component';
import { ContentEditorComponent } from './editpage/content-editor.component'
import { ContentPlaceholderDirective } from './editpage/content-placeholder.directive';



@NgModule({
  declarations: [
    PageListComponent,
    StructureComponent,
    ContentdefinitionComponent,
    EditpageComponent,
    PageContentComponent,
    ContentEditorComponent,
    ContentPlaceholderDirective

  ],
  imports: [
    SharedModule,
    RouterModule.forChild(
      [
        { path: 'pages', component: PageListComponent },
        { path: 'pages/add', component: StructureComponent },
        { path: 'pages/structure/:id', component: StructureComponent },
        { path: 'pages/edit/:id', component: EditpageComponent }
      ]
    )
  ]
})
export class PagesModule { }