import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrestamotipoComponent } from './prestamotipo.component';

describe('PrestamotipoComponent', () => {
  let component: PrestamotipoComponent;
  let fixture: ComponentFixture<PrestamotipoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrestamotipoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrestamotipoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
