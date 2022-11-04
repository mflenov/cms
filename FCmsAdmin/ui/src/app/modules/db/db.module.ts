import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { StructureComponent } from 'src/app/shared/structure/structure.component';
import { DbContentComponent } from './content/db-content.component';

import { NewRepoComponent } from './newrepo/new-repo.component';
import { RepositoryComponent } from './repository/repository.component';
import { NewDbContentComponentComponent } from './content/new-db-content-component.component';



@NgModule({
  declarations: [
    DbContentComponent,
    RepositoryComponent,
    NewRepoComponent,
    NewDbContentComponentComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(
      [
        { path: 'db', component: RepositoryComponent },
        { path: 'db/content/add/:id', component: NewDbContentComponentComponent },
        { path: 'db/content/:id', component: DbContentComponent },
        { path: 'db/add', component: NewRepoComponent },
        { path: 'db/structure/:id', component: StructureComponent },
      ]
    )
  ]
})
export class DbModule { }
