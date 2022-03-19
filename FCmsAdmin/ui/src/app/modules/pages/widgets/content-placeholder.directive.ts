import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[contentplaceholder]'
})
export class ContentPlaceholderDirective {
  constructor(public viewContainerRef: ViewContainerRef) { }
}