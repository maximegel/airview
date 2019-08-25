import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FlightCardSummary } from './flight-card-summary.model';

@Component({
  selector: 'av-flight-card-summary',
  templateUrl: './flight-card-summary.component.html',
  styleUrls: ['./flight-card-summary.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightCardSummaryComponent implements OnInit {
  @Input() model = new FlightCardSummary();

  constructor() {}

  ngOnInit() {}
}
