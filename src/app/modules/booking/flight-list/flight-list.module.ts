import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FlightCardModule } from './flight-card/flight-card.module';
import { FlightListComponent } from './flight-list.component';

@NgModule({
  declarations: [FlightListComponent],
  imports: [CommonModule, FlexLayoutModule, FlightCardModule],
  exports: [FlightListComponent]
})
export class FlightListModule {}
