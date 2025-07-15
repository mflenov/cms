import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import {ContentListComponent} from './content-list/content-list.component'


@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(
      [
        { path: 'content', component: ContentListComponent },
      ]
    )
  ]
})
export class ContentModule { }
