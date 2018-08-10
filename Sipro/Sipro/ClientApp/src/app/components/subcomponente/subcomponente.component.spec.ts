import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubcomponenteComponent } from './subcomponente.component';

describe('SubcomponenteComponent', () => {
  let component: SubcomponenteComponent;
  let fixture: ComponentFixture<SubcomponenteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubcomponenteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubcomponenteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
