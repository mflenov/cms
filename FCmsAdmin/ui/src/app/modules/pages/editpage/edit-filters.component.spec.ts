import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditFiltersComponent } from './edit-filters.component';

describe('EditFiltersComponent', () => {
  let component: EditFiltersComponent;
  let fixture: ComponentFixture<EditFiltersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditFiltersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
