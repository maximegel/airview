import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FlightCardDetail } from './flight-card/flight-card-detail/flight-card-detail.model';
import { FlightCardSummary } from './flight-card/flight-card-summary/flight-card-summary.model';

@Component({
  selector: 'av-flight-list',
  templateUrl: './flight-list.component.html',
  styleUrls: ['./flight-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightListComponent implements OnInit {
  active: FlightCardDetail;
  items: FlightCardSummary[] = [
    {
      id: '1',
      type: 'depart',
      carriers: ['Fly Swift'],
      price: 1406,
      itinerary: {
        arrivalTime: new Date(new Date().setHours(22, 30)),
        arrivalAirport: 'JFK',
        departureTime: new Date(new Date().setHours(6, 10)),
        departureAirport: 'SEL',
        travelTime: 31,
        stops: 0
      }
    },
    {
      id: '2',
      type: 'depart',
      carriers: ['Pop Air'],
      price: 988,
      itinerary: {
        arrivalTime: new Date(new Date().setHours(13, 30)),
        arrivalAirport: 'JFK',
        departureTime: new Date(new Date().setHours(9, 30)),
        departureAirport: 'SEL',
        travelTime: 18,
        stops: 2
      }
    },
    {
      id: '3',
      type: 'depart',
      carriers: ['Zip Airline'],
      price: 888,
      itinerary: {
        arrivalTime: new Date(new Date().setHours(8, 45)),
        arrivalAirport: 'SEL',
        departureTime: new Date(new Date().setHours(18, 10)),
        departureAirport: 'JFK',
        travelTime: 18,
        stops: 0
      }
    }
  ];

  constructor() {}

  ngOnInit() {}

  onDetailShown() {
    this.active = {
      id: '3',
      type: 'depart',
      price: 888,
      arrivalAirport: {
        iataCode: 'JFK',
        name: 'John F. Kennedy Intl'
      },
      departureDate: new Date(new Date().setHours(11, 25)),
      departureAirport: {
        iataCode: 'SEL',
        name: 'Taoyuan Intl'
      },
      legs: [
        {
          arrivalTime: new Date(new Date().setHours(14, 21)),
          arrivalAirport: 'JFK',
          departureTime: new Date(new Date().setHours(11, 25)),
          departureAirport: 'SEL',
          travelTime: 17
        },
        {
          arrivalTime: new Date(new Date().setHours(14, 21)),
          arrivalAirport: 'JFK',
          departureTime: new Date(new Date().setHours(11, 25)),
          departureAirport: 'SEL',
          travelTime: 17
        }
      ]
    };
  }

  trackByFn(index: number, item: FlightCardSummary) {
    return item.id;
  }
}
