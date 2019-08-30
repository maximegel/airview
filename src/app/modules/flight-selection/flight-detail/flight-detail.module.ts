import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { FlightDetailComponent } from './flight-detail.component';
import { FlightItineraryDetailModule } from './flight-itinerary-detail/flight-itinerary-detail.module';

@NgModule({
  declarations: [FlightDetailComponent],
  imports: [
    CommonModule,
    FlexLayoutModule,
    MatButtonModule,
    MatDividerModule,
    MatIconModule,
    FlightItineraryDetailModule
  ],
  exports: [FlightDetailComponent]
})
export class FlightDetailModule {}
