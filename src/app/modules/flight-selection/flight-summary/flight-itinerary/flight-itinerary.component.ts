import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FlightItineraryModel } from './flight-itinerary.model';

@Component({
  selector: 'av-flight-itinerary',
  templateUrl: './flight-itinerary.component.html',
  styleUrls: ['./flight-itinerary.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightItineraryComponent implements OnInit {
  @Input() model = new FlightItineraryModel();

  constructor() {}

  get stopsBadge(): string {
    return this.model.stops === 0 ? '' : `${this.model.stops}`;
  }

  ngOnInit() {}
}
