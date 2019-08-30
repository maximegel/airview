import { FlightItineraryModel } from './flight-itinerary/flight-itinerary.model';

export class FlightSummaryModel {
  id: string = null;
  price = 0;
  carriers: string[] = [];
  depart = new FlightItineraryModel();
  return = new FlightItineraryModel();
}
