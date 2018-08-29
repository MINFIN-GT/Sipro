import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductopropiedadComponent } from './productopropiedad.component';

describe('ProductopropiedadComponent', () => {
  let component: ProductopropiedadComponent;
  let fixture: ComponentFixture<ProductopropiedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductopropiedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductopropiedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
