import { Injectable } from '@angular/core';
import { Observable, of, timer } from 'rxjs';
import { mapTo } from 'rxjs/operators';
import { FlightDetailModel } from './flight-detail/flight-detail.model';
import { FlightSummaryModel } from './flight-summary/flight-summary.model';

@Injectable()
export class FlightSelectionService {
  constructor() {}

  find(id: string): Observable<FlightDetailModel> {
    return of({
      id,
      price: 1406,
      depart: {
        type: 'depart',
        departureDate: new Date(new Date().setHours(6, 10)),
        departureAirport: {
          iataCode: 'SEL',
          name: 'Taoyuan Intl'
        },
        arrivalAirport: {
          iataCode: 'JFK',
          name: 'John F. Kennedy Intl'
        },
        legs: [
          {
            arrivalTime: new Date(new Date().setHours(14, 21)),
            arrivalAirport: 'JFK',
            departureTime: new Date(new Date().setHours(11, 25)),
            departureAirport: 'SEL',
            travelTimeInHours: 17
          },
          {
            arrivalTime: new Date(new Date().setHours(14, 21)),
            arrivalAirport: 'JFK',
            departureTime: new Date(new Date().setHours(11, 25)),
            departureAirport: 'SEL',
            travelTimeInHours: 17
          }
        ]
      },
      return: {
        type: 'return',
        departureDate: new Date(new Date().setHours(14, 21)),
        departureAirport: {
          iataCode: 'SEL',
          name: 'Taoyuan Intl'
        },
        arrivalAirport: {
          iataCode: 'JFK',
          name: 'John F. Kennedy Intl'
        },
        legs: [
          {
            arrivalTime: new Date(new Date().setHours(14, 21)),
            arrivalAirport: 'JFK',
            departureTime: new Date(new Date().setHours(11, 25)),
            departureAirport: 'SEL',
            travelTimeInHours: 17
          }
        ]
      }
    });
  }

  query(): Observable<FlightSummaryModel[]> {
    return timer(1000).pipe(
      mapTo([
        {
          id: '1',
          price: 1406,
          carriers: ['Fly Swift'],
          depart: {
            type: 'depart',
            departureTime: new Date(new Date().setHours(6, 10)),
            departureAirport: 'SEL',
            arrivalTime: new Date(new Date().setHours(22, 30)),
            arrivalAirport: 'JFK',
            travelTimeInHours: 23,
            stops: 2
          },
          return: {
            type: 'return',
            departureTime: new Date(new Date().setHours(14, 21)),
            departureAirport: 'SEL',
            arrivalTime: new Date(new Date().setHours(11, 25)),
            arrivalAirport: 'JFK',
            travelTimeInHours: 17,
            stops: 1
          }
        },
        {
          id: '2',
          price: 988,
          carriers: ['Pop Air'],
          depart: {
            type: 'depart',
            departureTime: new Date(new Date().setHours(6, 10)),
            departureAirport: 'SEL',
            arrivalTime: new Date(new Date().setHours(22, 30)),
            arrivalAirport: 'JFK',
            travelTimeInHours: 18,
            stops: 0
          },
          return: {
            type: 'return',
            departureTime: new Date(new Date().setHours(14, 21)),
            departureAirport: 'SEL',
            arrivalTime: new Date(new Date().setHours(11, 25)),
            arrivalAirport: 'JFK',
            travelTimeInHours: 30,
            stops: 3
          }
        },
        {
          id: '3',
          price: 888,
          carriers: ['Zip Airline'],
          depart: {
            type: 'depart',
            departureTime: new Date(new Date().setHours(6, 10)),
            departureAirport: 'SEL',
            arrivalTime: new Date(new Date().setHours(22, 30)),
            arrivalAirport: 'JFK',
            travelTimeInHours: 17,
            stops: 0
          },
          return: {
            type: 'return',
            departureTime: new Date(new Date().setHours(14, 21)),
            departureAirport: 'SEL',
            arrivalTime: new Date(new Date().setHours(11, 25)),
            arrivalAirport: 'JFK',
            travelTimeInHours: 18,
            stops: 0
          }
        }
      ])
    );
  }
}
