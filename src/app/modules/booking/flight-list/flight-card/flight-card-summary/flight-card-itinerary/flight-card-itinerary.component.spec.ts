import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightCardItineraryComponent } from './flight-card-itinerary.component';

describe('FlightCardItineraryComponent', () => {
  let component: FlightCardItineraryComponent;
  let fixture: ComponentFixture<FlightCardItineraryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlightCardItineraryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightCardItineraryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
