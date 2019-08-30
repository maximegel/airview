import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatCardModule } from '@angular/material/card';
import { FlightListActiveItemDirective } from './flight-list-active-item.directive';
import { FlightListItemDirective } from './flight-list-item.directive';
import { FlightListComponent } from './flight-list.component';

@NgModule({
  declarations: [FlightListComponent, FlightListItemDirective, FlightListActiveItemDirective],
  imports: [CommonModule, FlexLayoutModule, MatCardModule],
  exports: [FlightListComponent, FlightListItemDirective, FlightListActiveItemDirective]
})
export class FlightListModule {}
