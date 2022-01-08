import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageDefinitionComponent } from './page-definition.component';

describe('PageDefinitionComponent', () => {
  let component: PageDefinitionComponent;
  let fixture: ComponentFixture<PageDefinitionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageDefinitionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageDefinitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
