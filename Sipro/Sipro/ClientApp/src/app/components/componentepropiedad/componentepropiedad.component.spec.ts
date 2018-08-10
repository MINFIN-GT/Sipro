import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentepropiedadComponent } from './componentepropiedad.component';

describe('ComponentepropiedadComponent', () => {
  let component: ComponentepropiedadComponent;
  let fixture: ComponentFixture<ComponentepropiedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComponentepropiedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComponentepropiedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
