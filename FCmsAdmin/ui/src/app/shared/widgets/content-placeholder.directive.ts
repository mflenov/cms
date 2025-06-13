import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
    selector: '[contentplaceholder]',
    standalone: false
})
export class ContentPlaceholderDirective {
  constructor(public viewContainerRef: ViewContainerRef) { }
}