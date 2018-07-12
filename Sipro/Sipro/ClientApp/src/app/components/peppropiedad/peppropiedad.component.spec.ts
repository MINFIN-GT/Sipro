import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PeppropiedadComponent } from './peppropiedad.component';

describe('PeppropiedadComponent', () => {
  let component: PeppropiedadComponent;
  let fixture: ComponentFixture<PeppropiedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PeppropiedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PeppropiedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
