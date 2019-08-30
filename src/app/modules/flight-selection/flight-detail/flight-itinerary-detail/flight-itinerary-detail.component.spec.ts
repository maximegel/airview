import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightItineraryDetailComponent } from './flight-itinerary-detail.component';

describe('FlightItineraryDetailComponent', () => {
  let component: FlightItineraryDetailComponent;
  let fixture: ComponentFixture<FlightItineraryDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlightItineraryDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightItineraryDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
