export class FlightItineraryModel {
  type: 'depart' | 'return' = 'depart';
  arrivalTime = new Date(0);
  arrivalAirport = '';
  departureTime = new Date(0);
  departureAirport = '';
  travelTimeInHours = 0;
  stops = 0;
}
