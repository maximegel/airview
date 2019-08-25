import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FlightCardLeg } from './flight-card-leg.model';

@Component({
  selector: 'av-flight-card-leg',
  templateUrl: './flight-card-leg.component.html',
  styleUrls: ['./flight-card-leg.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightCardLegComponent implements OnInit {
  @Input() model = new FlightCardLeg();

  constructor() {}

  ngOnInit() {}
}
