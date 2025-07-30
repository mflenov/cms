import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegexFilterEditorComponent } from './regex-filter-editor.component';

describe('RegexFilterEditorComponent', () => {
  let component: RegexFilterEditorComponent;
  let fixture: ComponentFixture<RegexFilterEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegexFilterEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegexFilterEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
