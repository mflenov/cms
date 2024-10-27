import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DbconnectionComponent } from './dbconnection.component';

describe('DbconnectionComponent', () => {
  let component: DbconnectionComponent;
  let fixture: ComponentFixture<DbconnectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DbconnectionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DbconnectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
