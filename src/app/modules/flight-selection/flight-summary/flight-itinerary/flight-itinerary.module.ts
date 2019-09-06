import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatIconModule } from '@angular/material/icon';
import { ArrowModule } from '~shared/arrow/arrow.module';
import { FlightItineraryComponent } from './flight-itinerary.component';

@NgModule({
  declarations: [FlightItineraryComponent],
  imports: [CommonModule, FlexLayoutModule, MatIconModule, ArrowModule],
  exports: [FlightItineraryComponent]
})
export class FlightItineraryModule {}
