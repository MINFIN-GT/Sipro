import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccesodenegadoComponent } from './accesodenegado.component';

describe('AccesodenegadoComponent', () => {
  let component: AccesodenegadoComponent;
  let fixture: ComponentFixture<AccesodenegadoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccesodenegadoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccesodenegadoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
