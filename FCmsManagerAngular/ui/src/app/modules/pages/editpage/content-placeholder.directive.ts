import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[newContentItem]'
})
export class ContentPlaceholderDirective {
  constructor(public viewContainerRef: ViewContainerRef) { }
}