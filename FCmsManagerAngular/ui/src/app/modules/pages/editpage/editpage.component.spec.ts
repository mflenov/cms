import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditpageComponent } from './editpage.component';

describe('EditpageComponent', () => {
  let component: EditpageComponent;
  let fixture: ComponentFixture<EditpageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditpageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditpageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
