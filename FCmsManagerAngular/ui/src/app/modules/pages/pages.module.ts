import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';
import { ContentDefinitionComponent } from './content-definition/content-definition.component';

import { PageListComponent } from './page-list/page-list.component';
import { StructureComponent } from './structure/structure.component';
import { ContentdefinitionComponent } from './structure/content-definition.component';
import { EditpageComponent } from './editpage/editpage.component'



@NgModule({
  declarations: [
    PageListComponent,
    StructureComponent,
    ContentdefinitionComponent,
    EditpageComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(
      [
        { path: 'pages', component: PageListComponent },
        { path: 'pages/add', component: StructureComponent },
        { path: 'pages/structure/:id', component: StructureComponent },
        { path: 'pages/definition/:id', component: ContentDefinitionComponent },
        { path: 'pages/edit/:id', component: EditpageComponent }
      ]
    )
  ]
})
export class PagesModule { }