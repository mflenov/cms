import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DaterangeFilterEditorComponent } from './daterange-filter-editor.component';

describe('DaterangeFilterEditorComponent', () => {
  let component: DaterangeFilterEditorComponent;
  let fixture: ComponentFixture<DaterangeFilterEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DaterangeFilterEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DaterangeFilterEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
