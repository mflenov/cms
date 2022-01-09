import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FolderdefinitionComponent } from './folderdefinition.component';

describe('FolderdefinitionComponent', () => {
  let component: FolderdefinitionComponent;
  let fixture: ComponentFixture<FolderdefinitionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FolderdefinitionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FolderdefinitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
