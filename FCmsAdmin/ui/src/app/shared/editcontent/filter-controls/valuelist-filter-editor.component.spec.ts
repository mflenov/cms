import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ValuelistFilterEditorComponent } from './valuelist-filter-editor.component';

describe('ValuelistFilterEditorComponent', () => {
  let component: ValuelistFilterEditorComponent;
  let fixture: ComponentFixture<ValuelistFilterEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ValuelistFilterEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ValuelistFilterEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
