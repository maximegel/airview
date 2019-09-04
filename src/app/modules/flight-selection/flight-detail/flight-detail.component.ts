import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FlightDetailModel } from './flight-detail.model';

@Component({
  selector: 'av-flight-detail',
  templateUrl: './flight-detail.component.html',
  styleUrls: ['./flight-detail.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightDetailComponent implements OnInit {
  @Input() model: FlightDetailModel;
  @Output() closed = new EventEmitter<void>();
  @Output() selected = new EventEmitter<FlightDetailModel>();

  constructor() {}

  ngOnInit() {}

  close() {
    this.closed.emit();
  }

  select() {
    this.selected.emit(this.model);
  }
}
