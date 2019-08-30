import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: '[avFlightListItem]'
})
export class FlightListItemDirective {
  constructor(public templateRef: TemplateRef<any>) {}
}
