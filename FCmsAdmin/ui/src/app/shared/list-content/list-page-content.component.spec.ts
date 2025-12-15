import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListPageContentComponent } from './list-page-content.component';

describe('ListPageContentComponent', () => {
  let component: ListPageContentComponent;
  let fixture: ComponentFixture<ListPageContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListPageContentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListPageContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
