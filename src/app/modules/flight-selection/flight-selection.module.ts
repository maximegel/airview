import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlightDetailModule } from './flight-detail/flight-detail.module';
import { FlightListModule } from './flight-list/flight-list.module';
import { FlightSelectionRoutingModule } from './flight-selection-routing.module';
import { FlightSelectionComponent } from './flight-selection.component';
import { FlightSelectionService } from './flight-selection.service';
import { FlightSummaryModule } from './flight-summary/flight-summary.module';

@NgModule({
  providers: [FlightSelectionService],
  declarations: [FlightSelectionComponent],
  imports: [CommonModule, FlightSelectionRoutingModule, FlightListModule, FlightDetailModule, FlightSummaryModule]
})
export class FlightSelectionModule {}
