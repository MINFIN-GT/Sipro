import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PepComponent } from './pep.component';

describe('ProyectoComponent', () => {
  let component: PepComponent;
  let fixture: ComponentFixture<PepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PepComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
