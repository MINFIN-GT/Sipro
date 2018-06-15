import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrestamoComponent } from './prestamo.component';

describe('PrestamoComponent', () => {
  let component: PrestamoComponent;
  let fixture: ComponentFixture<PrestamoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrestamoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrestamoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
