import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightCardDetailComponent } from './flight-card-detail.component';

describe('FlightCardDetailComponent', () => {
  let component: FlightCardDetailComponent;
  let fixture: ComponentFixture<FlightCardDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlightCardDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightCardDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
