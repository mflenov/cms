import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';

import { PageListComponent } from './page-list/page-list.component';
import { NewPageComponent } from './newpage/new-page.component';
import { ListPageContentComponent } from './list-content/list-page-content.component';
import { PreviewComponent } from './preview/preview.component';
import { StructureComponent } from 'src/app/shared/structure/structure.component';
import { EditContentComponent } from 'src/app/shared/editcontent/edit-content.component';

@NgModule({
  declarations: [
    PageListComponent,
    NewPageComponent,
    ListPageContentComponent,
    PreviewComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(
      [
        { path: 'pages', component: PageListComponent },
        { path: 'pages/add', component: NewPageComponent },
        { path: 'pages/structure/:id', component: StructureComponent },
        { path: 'pages/edit/:id', component: EditContentComponent },
        { path: 'pages/preview/:id', component: PreviewComponent },
        { path: 'pages/list/:repo/:id', component: ListPageContentComponent }
      ]
    )
  ],
})
export class PagesModule { }