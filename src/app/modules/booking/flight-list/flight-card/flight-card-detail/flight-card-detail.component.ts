import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FlightCardDetail } from './flight-card-detail.model';

@Component({
  selector: 'av-flight-card-detail',
  templateUrl: './flight-card-detail.component.html',
  styleUrls: ['./flight-card-detail.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightCardDetailComponent implements OnInit {
  @Input() model: FlightCardDetail;

  constructor() {}

  ngOnInit() {
    console.log(this.model);
  }
}
