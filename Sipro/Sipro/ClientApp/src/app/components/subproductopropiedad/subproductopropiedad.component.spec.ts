import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubproductopropiedadComponent } from './subproductopropiedad.component';

describe('SubproductopropiedadComponent', () => {
  let component: SubproductopropiedadComponent;
  let fixture: ComponentFixture<SubproductopropiedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubproductopropiedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubproductopropiedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
