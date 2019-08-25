import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output
} from '@angular/core';
import { FlightCardDetail } from './flight-card-detail/flight-card-detail.model';
import { FlightCardSummary } from './flight-card-summary/flight-card-summary.model';

@Component({
  selector: 'av-flight-card',
  templateUrl: './flight-card.component.html',
  styleUrls: ['./flight-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightCardComponent implements OnInit {
  @Input() detail = new FlightCardDetail();
  @Input() detailed = false;
  @Input() summary = new FlightCardSummary();
  @Output() detailShown = new EventEmitter<void>();

  constructor() {}

  ngOnInit() {}

  hideDetail() {
    this.detailed = false;
  }

  showDetail() {
    this.detailed = true;
    this.detailShown.emit();
  }
}
