import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { FlightDetailModule } from './flight-detail/flight-detail.module';
import { FlightListModule } from './flight-list/flight-list.module';
import { FlightSelectionRoutingModule } from './flight-selection-routing.module';
import { FlightSelectionComponent } from './flight-selection.component';
import { FlightSelectionGuard } from './flight-selection.guard';
import { FlightSelectionService } from './flight-selection.service';
import { FlightSelectionState } from './flight-selection.state';
import { FlightSummaryModule } from './flight-summary/flight-summary.module';

@NgModule({
  providers: [FlightSelectionService, FlightSelectionGuard],
  declarations: [FlightSelectionComponent],
  imports: [
    CommonModule,
    NgxsModule.forFeature([FlightSelectionState]),
    FlightListModule,
    FlightDetailModule,
    FlightSummaryModule,
    FlightSelectionRoutingModule
  ]
})
export class FlightSelectionModule {}
