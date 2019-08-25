import { FlightCardItinerary } from './flight-card-itinerary/flight-card-itinerary.model';

export class FlightCardSummary {
  id = '';
  type: 'depart' | 'return' = 'depart';
  price = 0;
  carriers: string[] = [];
  itinerary = new FlightCardItinerary();
}
