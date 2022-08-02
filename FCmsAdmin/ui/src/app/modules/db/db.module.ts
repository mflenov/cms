import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { StructureComponent } from 'src/app/shared/structure/structure.component';
import { DbContentComponent } from './content/dbcontent.component';

import { NewRepoComponent } from './newrepo/new-repo.component';
import { RepositoryComponent } from './repository/repository.component';



@NgModule({
  declarations: [
    DbContentComponent,
    RepositoryComponent,
    NewRepoComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(
      [
        { path: 'db', component: RepositoryComponent },
        { path: 'db/content/:id', component: DbContentComponent },
        { path: 'db/add', component: NewRepoComponent },
        { path: 'db/structure/:id', component: StructureComponent },
      ]
    )
  ]
})
export class DbModule { }
