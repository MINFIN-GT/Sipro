import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubcomponentepropiedadComponent } from './subcomponentepropiedad.component';

describe('SubcomponentepropiedadComponent', () => {
  let component: SubcomponentepropiedadComponent;
  let fixture: ComponentFixture<SubcomponentepropiedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubcomponentepropiedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubcomponentepropiedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
