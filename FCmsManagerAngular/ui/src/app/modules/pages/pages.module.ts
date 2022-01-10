import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';
import { ContentDefinitionComponent } from './content-definition/content-definition.component';

import { PageListComponent } from './page-list/page-list.component';
import { StructureComponent } from './structure/structure.component';
import { FolderdefinitionComponent } from './structure/folderdefinition.component';
import { ContentdefinitionComponent } from './structure/contentdefinition.component'



@NgModule({
  declarations: [
    PageListComponent,
    StructureComponent,
    FolderdefinitionComponent,
    ContentdefinitionComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(
      [
        { path: 'pages', component: PageListComponent },
        { path: 'pages/add', component: StructureComponent },
        { path: 'pages/structure/:id', component: StructureComponent },
        { path: 'pages/definition/:id', component: ContentDefinitionComponent }
      ]
    )
  ]
})
export class PagesModule { }