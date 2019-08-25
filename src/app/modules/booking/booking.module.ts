import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BookingRoutingModule } from './booking-routing.module';
import { BookingComponent } from './booking.component';
import { FlightListModule } from './flight-list/flight-list.module';

@NgModule({
  declarations: [BookingComponent],
  imports: [CommonModule, FlexLayoutModule, BookingRoutingModule, FlightListModule]
})
export class BookingModule {}
