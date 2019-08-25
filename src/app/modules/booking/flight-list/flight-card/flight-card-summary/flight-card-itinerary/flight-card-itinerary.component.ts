import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FlightCardItinerary } from './flight-card-itinerary.model';

@Component({
  selector: 'av-flight-card-itinerary',
  templateUrl: './flight-card-itinerary.component.html',
  styleUrls: ['./flight-card-itinerary.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightCardItineraryComponent implements OnInit {
  @Input() model = new FlightCardItinerary();
  @Input() type: 'depart' | 'return' = 'depart';

  constructor() {}

  get stopsBadge(): string {
    return this.model.stops === 0 ? '' : `${this.model.stops}`;
  }

  ngOnInit() {}
}
