import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BookingRoutingModule } from './booking-routing.module';
import { BookingComponent } from './booking.component';
import { FlightListModule } from './flight-list/flight-list.module';

@NgModule({
  declarations: [BookingComponent],
  imports: [CommonModule, BookingRoutingModule, FlightListModule]
})
export class BookingModule {}
