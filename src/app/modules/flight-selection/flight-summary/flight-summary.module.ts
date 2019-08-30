import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FlightItineraryModule } from './flight-itinerary/flight-itinerary.module';
import { FlightSummaryComponent } from './flight-summary.component';

@NgModule({
  declarations: [FlightSummaryComponent],
  imports: [CommonModule, FlexLayoutModule, FlightItineraryModule],
  exports: [FlightSummaryComponent]
})
export class FlightSummaryModule {}
