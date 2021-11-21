import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, of } from 'rxjs';
import { NgForm } from '@angular/forms';
import { Observable } from 'rxjs';


import { FiltersService } from './filters.service';
import { IFilterModel } from './filter-model';
import { CmsenumsService } from '../cmsenums.service'
import { IEnumsModel } from '../enums-model'


@Component({
  selector: 'app-filter',
  templateUrl: './filter-edit.component.html',
  styleUrls: ['./filter-edit.component.css'],
  providers: [FiltersService]
})

export class FilterComponent implements OnInit, OnDestroy {
  model: IFilterModel = {
    name : '',
    type : 'string'
  };

  filterTypes!: Observable<string[]>;


  modelSubs!: Subscription;
  filterTypeSubs!: Subscription;

  constructor(private filtersService: FiltersService, private route: ActivatedRoute,
    private router: Router, private cmsenumsService: CmsenumsService) {
  }

  ngOnInit(): void {
    this.filterTypeSubs = this.cmsenumsService.getEnums().subscribe({
      next: model => { this.filterTypes = of(model.filterTypes); }
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.modelSubs = this.filtersService.getById(id).subscribe({
        next: model => { this.model = model }
      });
    }
  }

  ngOnDestroy(): void {
    if (this.modelSubs) {
      this.modelSubs.unsubscribe();
    }
    if (this.filterTypeSubs) {
      this.filterTypeSubs.unsubscribe();
    }
  }

  onSubmit(form: NgForm): void {
    this.filtersService.save(this.model).subscribe({
      next: data => {
        this.router.navigate(['/config/filters']);
      }
    });
  }
}