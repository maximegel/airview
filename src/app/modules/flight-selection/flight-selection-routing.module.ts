import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FlightSelectionComponent } from './flight-selection.component';
import { FlightSelectionGuard } from './flight-selection.guard';

const routes: Routes = [{ path: '', component: FlightSelectionComponent, canActivate: [FlightSelectionGuard] }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FlightSelectionRoutingModule {}
