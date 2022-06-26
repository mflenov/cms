import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { IDbContentModel } from '../models/dncontent.model';
import { DbContentService } from '../services/dbcontent.service';

@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.css'],
  providers: [DbContentService]
})
export class ContentComponent implements OnInit, OnDestroy {
  data: IDbContentModel[] = [];
  contentSubs!: Subscription;
  definitionId!: string;

  constructor(
    private contentService: DbContentService,
    private route: ActivatedRoute
  )
  { }

  ngOnInit(): void {
    this.definitionId = this.route.snapshot.paramMap.get('id') ?? '';

    this.contentSubs = this.contentService.getDbContent(this.definitionId).subscribe(
      dbcontent => {
        this.data = (dbcontent.data as IDbContentModel[]);
      }
    );
  }

  ngOnDestroy(): void {
    this.contentSubs.unsubscribe();
  }
}
