import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoolFilterEditorComponent } from './bool-filter-editor.component';

describe('BoolFilterEditorComponent', () => {
  let component: BoolFilterEditorComponent;
  let fixture: ComponentFixture<BoolFilterEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BoolFilterEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BoolFilterEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
