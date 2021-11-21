import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';

import { HomeComponent } from './home/home.component';
import { FiltersComponent } from './filters/filter-list.component';
import { FilterComponent } from './filters/filter-edit.component';



@NgModule({
  declarations: [
    FiltersComponent,
    HomeComponent,
    FilterComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(
      [
        { path: 'config', component: HomeComponent },
        { path: 'config/filters', component: FiltersComponent },
        { path: 'config/filter/:id', component: FilterComponent },
        { path: 'config/filter', component: FilterComponent },
      ]
    )
  ]
})
export class ConfigModule { }
