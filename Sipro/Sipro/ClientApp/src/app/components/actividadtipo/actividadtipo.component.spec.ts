import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActividadtipoComponent } from './actividadtipo.component';

describe('ActividadtipoComponent', () => {
  let component: ActividadtipoComponent;
  let fixture: ComponentFixture<ActividadtipoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActividadtipoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActividadtipoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
