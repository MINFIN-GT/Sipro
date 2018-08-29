import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubproductoComponent } from './subproducto.component';

describe('SubproductoComponent', () => {
  let component: SubproductoComponent;
  let fixture: ComponentFixture<SubproductoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubproductoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubproductoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
