import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActividadpropiedadComponent } from './actividadpropiedad.component';

describe('ActividadpropiedadComponent', () => {
  let component: ActividadpropiedadComponent;
  let fixture: ComponentFixture<ActividadpropiedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActividadpropiedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActividadpropiedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
