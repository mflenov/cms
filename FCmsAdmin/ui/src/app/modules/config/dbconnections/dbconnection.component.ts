import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of, Subscription } from 'rxjs';

import { IDbConnectionModel } from 'src/app/models/dbconnection-model';
import { CmsenumsService } from 'src/app/services/cmsenums.service';
import { DbconnectionsService } from 'src/app/services/dbconnections.service';

@Component({
  selector: 'app-dbconnection',
  templateUrl: './dbconnection.component.html',
  styleUrl: './dbconnection.component.css',
  providers: [DbconnectionsService, CmsenumsService]
})
export class DbconnectionComponent {
  model: IDbConnectionModel = {
    name: '',
    connectionString: '',
    databaseType: 'PostgresSQL'
  };

  databaseTypes!: Observable<string[]>;

  modelSubs!: Subscription;
  filterTypeSubs!: Subscription;

  constructor(private dbconnectionsService: DbconnectionsService,
    private route: ActivatedRoute,
    private router: Router,
    private cmsenumsService: CmsenumsService) {
  }

  ngOnInit(): void {
    this.filterTypeSubs = this.cmsenumsService.getEnums().subscribe({
      next: model => { this.databaseTypes = of(model.databaseTypes); }
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.modelSubs = this.dbconnectionsService.getById(id).subscribe({
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
    this.dbconnectionsService.save(this.model).subscribe({
      next: data => {
        this.router.navigate(['/config/dbconnections']);
      }
    });
  }
}
