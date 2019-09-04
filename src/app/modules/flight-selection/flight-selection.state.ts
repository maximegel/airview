import { ActivatedRoute } from '@angular/router';
import { Navigate } from '@ngxs/router-plugin';
import { Action, NgxsOnInit, Selector, State, StateContext } from '@ngxs/store';
import { filter, map, tap } from 'rxjs/operators';
import { FlightDetailModel } from './flight-detail/flight-detail.model';
import { HideFlightDetail, QueryFlights, SelectFlight, ShowFlightDetail } from './flight-selection.actions';
import { FlightSelectionService } from './flight-selection.service';
import { FlightSummaryModel } from './flight-summary/flight-summary.model';

export class FlightSelectionStateModel {
  active: FlightDetailModel = null;
  list: FlightSummaryModel[] = [];
}

@State<FlightSelectionStateModel>({
  name: 'flightSelection',
  defaults: new FlightSelectionStateModel()
})
export class FlightSelectionState implements NgxsOnInit {
  constructor(private service: FlightSelectionService, private route: ActivatedRoute) {}

  @Selector()
  static active({ active }: FlightSelectionStateModel): FlightDetailModel {
    return active;
  }

  @Selector()
  static list({ list }: FlightSelectionStateModel): FlightSummaryModel[] {
    return list;
  }

  ngxsOnInit({ dispatch }: StateContext<FlightSelectionStateModel>) {
    this.route.queryParams
      .pipe(
        map(params => params.active),
        filter(id => !!id),
        tap(id => dispatch(new ShowFlightDetail(id)))
      )
      .subscribe();
  }

  @Action(HideFlightDetail)
  hideDetail({ dispatch, patchState }: StateContext<FlightSelectionStateModel>) {
    patchState({ active: null });
    dispatch(new Navigate(['flights']));
  }

  @Action(QueryFlights)
  query({ patchState }: StateContext<FlightSelectionStateModel>) {
    return this.service.query().pipe(tap(list => patchState({ list })));
  }

  @Action(SelectFlight)
  select(ctx: StateContext<FlightSelectionStateModel>, action: SelectFlight) {}

  @Action(ShowFlightDetail)
  showDetail({ dispatch, patchState }: StateContext<FlightSelectionStateModel>, { id }: ShowFlightDetail) {
    return this.service.find(id).pipe(
      tap(active => patchState({ active })),
      tap(active => dispatch(new Navigate(['flights'], { active: active.id })))
    );
  }
}
