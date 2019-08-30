import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FlightLegModel } from './flight-leg.model';

@Component({
  selector: 'av-flight-leg',
  templateUrl: './flight-leg.component.html',
  styleUrls: ['./flight-leg.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightLegComponent implements OnInit {
  @Input() model = new FlightLegModel();

  constructor() {}

  ngOnInit() {}
}
