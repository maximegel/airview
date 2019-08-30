import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'flights', pathMatch: 'full' },
  {
    path: 'flights',
    loadChildren: () =>
      import('./modules/flight-selection/flight-selection.module').then(
        m => m.FlightSelectionModule
      )
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
