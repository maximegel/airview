import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightCardSummaryComponent } from './flight-card-summary.component';

describe('FlightCardSummaryComponent', () => {
  let component: FlightCardSummaryComponent;
  let fixture: ComponentFixture<FlightCardSummaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlightCardSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightCardSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
