import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { FlightDetailModel } from './flight-detail/flight-detail.model';
import { HideFlightDetail, SelectFlight, ShowFlightDetail } from './flight-selection.actions';
import { FlightSelectionState } from './flight-selection.state';
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
  @Select(FlightSelectionState.active) active$: Observable<FlightDetailModel>;
  @Select(FlightSelectionState.list) list$: Observable<FlightSummaryModel[]>;

  constructor(private store: Store) {}

  ngOnInit() {}

  hideDetail() {
    return this.store.dispatch(new HideFlightDetail());
  }

  select(id: string) {
    return this.store.dispatch(new SelectFlight(id));
  }

  showDetail(id: string) {
    return this.store.dispatch(new ShowFlightDetail(id));
  }
}
