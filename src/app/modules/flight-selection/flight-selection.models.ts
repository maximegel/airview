export class FlightAirportStateModel {
  iataCode = '';
  name = '';
}

export class FlightLegStateModel {
  arrivalTime = new Date(0);
  arrivalAirport = '';
  departureTime = new Date(0);
  departureAirport = '';
  travelTime = 0;
}

export class FlightItineraryStateModel {
  type: 'depart' | 'return' = 'depart';
  arrivalTime = new Date(0);
  arrivalAirport = new FlightAirportStateModel();
  departureTime = new Date(0);
  departureAirport = new FlightAirportStateModel();
  travelTime = 0;
  stops = 0;
  legs: FlightLegStateModel[] = [];
}

export class FlightOptionStateModel {
  id = '';
  price = 0;
  carriers: ['Fly Swift'];
  depart = new FlightItineraryStateModel();
  return = new FlightItineraryStateModel();
}
