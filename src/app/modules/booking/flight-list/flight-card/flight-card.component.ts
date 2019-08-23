import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FlightCardViewModel } from './flight-card.view-models';

@Component({
  selector: 'av-flight-card',
  templateUrl: './flight-card.component.html',
  styleUrls: ['./flight-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightCardComponent implements OnInit {
  @Input() model: FlightCardViewModel;

  constructor() {}

  ngOnInit() {}
}
