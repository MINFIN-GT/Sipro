import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubcomponentetipoComponent } from './subcomponentetipo.component';

describe('SubcomponentetipoComponent', () => {
  let component: SubcomponentetipoComponent;
  let fixture: ComponentFixture<SubcomponentetipoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubcomponentetipoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubcomponentetipoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
