import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatBadgeModule } from '@angular/material/badge';
import { MatIconModule } from '@angular/material/icon';
import { FlightCardItineraryComponent } from './flight-card-itinerary.component';

@NgModule({
  declarations: [FlightCardItineraryComponent],
  imports: [CommonModule, FlexLayoutModule, MatBadgeModule, MatIconModule],
  exports: [FlightCardItineraryComponent]
})
export class FlightCardItineraryModule {}
