import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextFilterEditorComponent } from './text-filter-editor.component';

describe('TextFilterEditorComponent', () => {
  let component: TextFilterEditorComponent;
  let fixture: ComponentFixture<TextFilterEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TextFilterEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TextFilterEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
