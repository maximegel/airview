import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FlightItineraryDetailModel } from './flight-itinerary-detail.model';

@Component({
  selector: 'av-flight-itinerary-detail',
  templateUrl: './flight-itinerary-detail.component.html',
  styleUrls: ['./flight-itinerary-detail.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightItineraryDetailComponent implements OnInit {
  @Input() model = new FlightItineraryDetailModel();

  constructor() {}

  ngOnInit() {}
}
