import { FlightLegModel } from './flight-leg/flight-leg.model';

export class FlightItineraryDetailAirportModel {
  iataCode = '';
  name = '';
}

export class FlightItineraryDetailModel {
  type: 'depart' | 'return' = 'depart';
  number = '';
  price = 0;
  carrier = '';
  departureDate = new Date(0);
  arrivalAirport = new FlightItineraryDetailAirportModel();
  departureAirport = new FlightItineraryDetailAirportModel();
  legs: FlightLegModel[] = [];
}
