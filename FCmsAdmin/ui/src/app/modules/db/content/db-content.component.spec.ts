import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DbContentComponent } from './dbcontent.component';

describe('ContentComponent', () => {
  let component: DbContentComponent;
  let fixture: ComponentFixture<DbContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DbContentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DbContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
