import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FlightSummaryModel } from './flight-summary.model';

@Component({
  selector: 'av-flight-summary',
  templateUrl: './flight-summary.component.html',
  styleUrls: ['./flight-summary.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightSummaryComponent implements OnInit {
  @Input() model = new FlightSummaryModel();

  constructor() {}

  ngOnInit() {}
}
