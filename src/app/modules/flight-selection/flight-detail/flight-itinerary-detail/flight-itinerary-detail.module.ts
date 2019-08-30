import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { FlightItineraryDetailComponent } from './flight-itinerary-detail.component';
import { FlightLegModule } from './flight-leg/flight-leg.module';

@NgModule({
  declarations: [FlightItineraryDetailComponent],
  imports: [CommonModule, FlexLayoutModule, MatDividerModule, MatIconModule, FlightLegModule],
  exports: [FlightItineraryDetailComponent]
})
export class FlightItineraryDetailModule {}
