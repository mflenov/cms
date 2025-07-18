import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { ContentListComponent } from './content-list/content-list.component'
import { NewContentComponent } from './new-content/new-content.component';


@NgModule({
  declarations: [
    ContentListComponent,
    NewContentComponent
  ],
  imports: [
    SharedModule,
    NgbModule,
    RouterModule.forChild(
      [
        { path: 'content', component: ContentListComponent },
        { path: 'content/add', component: NewContentComponent },
      ]
    )
  ]
})
export class ContentModule { }
