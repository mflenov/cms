import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewContentComponent } from './new-content.component';

describe('NewContentComponent', () => {
  let component: NewContentComponent;
  let fixture: ComponentFixture<NewContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewContentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
