import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatIconModule } from '@angular/material/icon';
import { FlightLegComponent } from './flight-leg.component';

@NgModule({
  declarations: [FlightLegComponent],
  imports: [CommonModule, FlexLayoutModule, MatIconModule],
  exports: [FlightLegComponent]
})
export class FlightLegModule {}
