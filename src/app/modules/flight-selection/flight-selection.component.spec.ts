import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightSelectionComponent } from './flight-selection.component';

describe('FlightSelectionComponent', () => {
  let component: FlightSelectionComponent;
  let fixture: ComponentFixture<FlightSelectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlightSelectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightSelectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
