import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDbContentComponentComponent } from './edit-db-content-component.component';

describe('NewDbContentComponentComponent', () => {
  let component: EditDbContentComponentComponent;
  let fixture: ComponentFixture<EditDbContentComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditDbContentComponentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditDbContentComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
