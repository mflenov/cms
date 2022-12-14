import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DbContentEditorComponent } from './db-content-editor.component';

describe('DbContentEditorComponent', () => {
  let component: DbContentEditorComponent;
  let fixture: ComponentFixture<DbContentEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DbContentEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DbContentEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
