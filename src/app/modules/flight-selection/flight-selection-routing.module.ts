import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FlightSelectionComponent } from './flight-selection.component';

const routes: Routes = [{ path: '', component: FlightSelectionComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FlightSelectionRoutingModule {}
