import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatIconModule } from '@angular/material/icon';
import { ArrowModule } from '~shared/arrow/arrow.module';
import { FlightLegComponent } from './flight-leg.component';

@NgModule({
  declarations: [FlightLegComponent],
  imports: [CommonModule, FlexLayoutModule, MatIconModule, ArrowModule],
  exports: [FlightLegComponent]
})
export class FlightLegModule {}
