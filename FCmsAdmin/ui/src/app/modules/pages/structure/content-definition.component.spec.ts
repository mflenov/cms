import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContentdefinitionComponent } from './content-definition.component';

describe('ContentdefinitionComponent', () => {
  let component: ContentdefinitionComponent;
  let fixture: ComponentFixture<ContentdefinitionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ContentdefinitionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContentdefinitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
