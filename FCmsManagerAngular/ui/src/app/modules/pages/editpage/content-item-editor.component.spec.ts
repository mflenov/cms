import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContentItemEditorComponent } from './content-item-editor.component';

describe('ContentEditorComponent', () => {
  let component: ContentItemEditorComponent;
  let fixture: ComponentFixture<ContentItemEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ContentItemEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContentItemEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
