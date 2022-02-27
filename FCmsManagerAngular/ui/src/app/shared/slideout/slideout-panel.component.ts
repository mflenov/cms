import { Component, Input, Output, EventEmitter} from '@angular/core';

@Component({
  selector: 'sh-slide-panel',
  styleUrls: ['./slideout-panel.component.css'],
  templateUrl: './slideout-panel.component.html'
})

export class SlidePanelComponent {
  @Input() isVisible: boolean = false;
  @Input() closeCaption: string = "Close";
  @Output() onSave: EventEmitter<any> = new EventEmitter();
  @Output() onCancel: EventEmitter<any> = new EventEmitter();

  close() {
    this.onSave.emit();
  }

  cancel() {
    this.onCancel.emit();
  }
}