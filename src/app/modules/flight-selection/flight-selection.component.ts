import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { FlightDetailModel } from './flight-detail/flight-detail.model';
import { FlightSelectionService } from './flight-selection.service';
import { FlightSummaryModel } from './flight-summary/flight-summary.model';

@Component({
  selector: 'av-flight-selection',
  templateUrl: './flight-selection.component.html',
  styleUrls: ['./flight-selection.component.scss'],
  host: {
    class: 'av-flight-selection'
  },
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightSelectionComponent implements OnInit {
  active$: Observable<FlightDetailModel>;
  list$: Observable<FlightSummaryModel[]>;

  constructor(private service: FlightSelectionService) {}

  ngOnInit() {
    this.list$ = this.service.query();
  }

  hideDetail() {
    this.active$ = of(null);
  }

  selectFlight(id: string) {
    alert(`Select flight #${id}...`);
  }

  showDetail(id: string) {
    this.active$ = this.service.find(id);
  }
}
