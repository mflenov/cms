import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewDbContentComponentComponent } from './new-db-content-component.component';

describe('NewDbContentComponentComponent', () => {
  let component: NewDbContentComponentComponent;
  let fixture: ComponentFixture<NewDbContentComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewDbContentComponentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewDbContentComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
