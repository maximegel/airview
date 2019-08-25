import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatCardModule } from '@angular/material/card';
import { FlightCardDetailModule } from './flight-card-detail/flight-card-detail.module';
import { FlightCardSummaryModule } from './flight-card-summary/flight-card-summary.module';
import { FlightCardComponent } from './flight-card.component';

@NgModule({
  declarations: [FlightCardComponent],
  imports: [
    CommonModule,
    FlexLayoutModule,
    MatCardModule,
    FlightCardDetailModule,
    FlightCardSummaryModule
  ],
  exports: [FlightCardComponent]
})
export class FlightCardModule {}
