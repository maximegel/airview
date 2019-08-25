import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FlightCardItineraryModule } from './flight-card-itinerary/flight-card-itinerary.module';
import { FlightCardSummaryComponent } from './flight-card-summary.component';

@NgModule({
  declarations: [FlightCardSummaryComponent],
  imports: [CommonModule, FlexLayoutModule, FlightCardItineraryModule],
  exports: [FlightCardSummaryComponent]
})
export class FlightCardSummaryModule {}
