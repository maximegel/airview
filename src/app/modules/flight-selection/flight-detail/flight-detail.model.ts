import { FlightItineraryDetailModel } from './flight-itinerary-detail/flight-itinerary-detail.model';

export class FlightDetailModel {
  id: string = null;
  price = 0;
  depart = new FlightItineraryDetailModel();
  return = new FlightItineraryDetailModel();
}
