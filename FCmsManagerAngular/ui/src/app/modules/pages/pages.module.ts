import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';

import { PageListComponent } from './page-list/page-list.component';



@NgModule({
  declarations: [
    PageListComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(
      [
        { path: 'pages', component: PageListComponent }
      ]
    )
  ]
})
export class PagesModule { }