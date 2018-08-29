import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductotipoComponent } from './productotipo.component';

describe('ProductotipoComponent', () => {
  let component: ProductotipoComponent;
  let fixture: ComponentFixture<ProductotipoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductotipoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductotipoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
