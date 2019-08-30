import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: '[avFlightListActiveItem]'
})
export class FlightListActiveItemDirective {
  constructor(public templateRef: TemplateRef<any>) {}
}
