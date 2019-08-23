import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { FlightCardComponent } from './flight-card.component';

@NgModule({
  declarations: [FlightCardComponent],
  imports: [CommonModule, MatCardModule],
  exports: [FlightCardComponent]
})
export class FlightCardModule {}
