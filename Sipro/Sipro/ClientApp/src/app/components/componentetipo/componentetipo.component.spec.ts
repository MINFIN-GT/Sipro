import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentetipoComponent } from './componentetipo.component';

describe('ComponentetipoComponent', () => {
  let component: ComponentetipoComponent;
  let fixture: ComponentFixture<ComponentetipoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComponentetipoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComponentetipoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
