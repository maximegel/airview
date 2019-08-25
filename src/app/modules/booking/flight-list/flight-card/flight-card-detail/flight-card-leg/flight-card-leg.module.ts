import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatIconModule } from '@angular/material/icon';
import { FlightCardLegComponent } from './flight-card-leg.component';

@NgModule({
  declarations: [FlightCardLegComponent],
  imports: [CommonModule, FlexLayoutModule, MatIconModule],
  exports: [FlightCardLegComponent]
})
export class FlightCardLegModule {}
