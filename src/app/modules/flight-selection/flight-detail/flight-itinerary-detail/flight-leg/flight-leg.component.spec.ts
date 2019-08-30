import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FlightLegComponent } from './flight-leg.component';

describe('FlightLegComponent', () => {
  let component: FlightLegComponent;
  let fixture: ComponentFixture<FlightLegComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FlightLegComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightLegComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
