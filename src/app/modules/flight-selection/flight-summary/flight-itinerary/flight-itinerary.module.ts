import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatBadgeModule } from '@angular/material/badge';
import { MatIconModule } from '@angular/material/icon';
import { FlightItineraryComponent } from './flight-itinerary.component';

@NgModule({
  declarations: [FlightItineraryComponent],
  imports: [CommonModule, FlexLayoutModule, MatBadgeModule, MatIconModule],
  exports: [FlightItineraryComponent]
})
export class FlightItineraryModule {}
