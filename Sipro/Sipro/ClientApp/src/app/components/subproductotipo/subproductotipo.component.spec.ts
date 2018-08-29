import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubproductotipoComponent } from './subproductotipo.component';

describe('SubproductotipoComponent', () => {
  let component: SubproductotipoComponent;
  let fixture: ComponentFixture<SubproductotipoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubproductotipoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubproductotipoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
