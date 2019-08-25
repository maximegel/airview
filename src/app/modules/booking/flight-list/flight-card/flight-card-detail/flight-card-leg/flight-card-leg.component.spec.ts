import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FlightCardLegComponent } from './flight-card-leg.component';

describe('FlightCardLegComponent', () => {
  let component: FlightCardLegComponent;
  let fixture: ComponentFixture<FlightCardLegComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FlightCardLegComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightCardLegComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
