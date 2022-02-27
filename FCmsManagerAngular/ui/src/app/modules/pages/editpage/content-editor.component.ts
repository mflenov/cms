import { Component, OnInit, Input, ViewChild, ComponentFactoryResolver } from '@angular/core';

import { IContentDefinitionsModel } from '../models/content-definitions.model';
import { IContentItemModel } from '../models/content-item.model';
import { ContentPlaceholderDirective } from './content-placeholder.directive';
import { EditFiltersComponent } from './edit-filters.component';

@Component({
  selector: 'pg-content-editor',
  templateUrl: './content-editor.component.html',
  styleUrls: ['./content-editor.component.css']
})
export class ContentEditorComponent implements OnInit {
  @Input() definition!: IContentDefinitionsModel;
  @Input() content!: any;
  isFiltersPanelVisible: boolean = false;

  @ViewChild(ContentPlaceholderDirective, { static: true }) placeholder!: ContentPlaceholderDirective;

  constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit(): void {
  }

  showFilters(id: string): void {
    this.createFiltersComponent();
    this.isFiltersPanelVisible = true;
  }

  onCancelFilters(): void {
    this.isFiltersPanelVisible = false;
  }

  onSaveFilters() {
    this.isFiltersPanelVisible = false;
  }

  createFiltersComponent() {
    this.placeholder.viewContainerRef.clear();
    let contentEditorComponent = this.componentFactoryResolver.resolveComponentFactory(EditFiltersComponent);
    let contentEditorComponentRef = this.placeholder.viewContainerRef.createComponent(contentEditorComponent);
    (<EditFiltersComponent>(contentEditorComponentRef.instance)).model = this.content.filters;
  }
}
