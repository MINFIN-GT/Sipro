import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PeptipoComponent } from './peptipo.component';

describe('PeptipoComponent', () => {
  let component: PeptipoComponent;
  let fixture: ComponentFixture<PeptipoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PeptipoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PeptipoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
