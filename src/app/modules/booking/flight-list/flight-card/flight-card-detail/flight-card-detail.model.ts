import { FlightCardLeg } from './flight-card-leg/flight-card-leg.model';

export class FlightCardDetailAirport {
  iataCode = '';
  name = '';
}

export class FlightCardDetail {
  id = '';
  type: 'depart' | 'return' = 'depart';
  price = 0;
  arrivalAirport = new FlightCardDetailAirport();
  departureDate = new Date(0);
  departureAirport = new FlightCardDetailAirport();
  legs: FlightCardLeg[] = [];
}
