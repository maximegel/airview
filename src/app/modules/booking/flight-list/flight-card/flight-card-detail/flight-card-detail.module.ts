import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { FlightCardDetailComponent } from './flight-card-detail.component';
import { FlightCardLegModule } from './flight-card-leg/flight-card-leg.module';

@NgModule({
  declarations: [FlightCardDetailComponent],
  imports: [
    CommonModule,
    FlexLayoutModule,
    MatButtonModule,
    MatDividerModule,
    MatIconModule,
    FlightCardLegModule
  ],
  exports: [FlightCardDetailComponent]
})
export class FlightCardDetailModule {}
