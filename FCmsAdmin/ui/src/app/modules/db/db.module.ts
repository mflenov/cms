import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { StructureComponent } from 'src/app/shared/structure/structure.component';
import { DbContentComponent } from './content/db-content.component';

import { NewRepoComponent } from './newrepo/new-repo.component';
import { RepositoryComponent } from './repository/repository.component';
import { EditDbContentComponentComponent } from './content/edit-db-content-component.component';
import { DbContentEditorComponent } from './content/db-content-editor.component';

import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { NgxTinymceModule } from 'ngx-tinymce';


@NgModule({
  declarations: [
    DbContentComponent,
    RepositoryComponent,
    NewRepoComponent,
    EditDbContentComponentComponent,
    DbContentEditorComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(
      [
        { path: 'db', component: RepositoryComponent },
        { path: 'db/content/add/:id', component: EditDbContentComponentComponent },
        { path: 'db/content/:id', component: DbContentComponent },
        { path: 'db/add', component: NewRepoComponent },
        { path: 'db/structure/:id', component: StructureComponent },
      ]
    ),
    NgxTinymceModule.forRoot({
      baseURL: '/tinymce/'
    }),
  ],
  providers: [
    { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' }
  ]
})
export class DbModule { }
