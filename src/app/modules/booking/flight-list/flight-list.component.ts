import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FlightCardViewModel } from './flight-card/flight-card.view-models';
import { FlightListViewModel } from './flight-list.view-models';

@Component({
  selector: 'av-flight-list',
  templateUrl: './flight-list.component.html',
  styleUrls: ['./flight-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightListComponent implements OnInit {
  model: FlightListViewModel = {
    items: [
      {
        id: '1',
        company: 'Fly Swift',
        price: 1406
      },
      {
        id: '2',
        company: 'Pop Air',
        price: 988
      },
      {
        id: '3',
        company: 'Zip Airline',
        price: 888
      }
    ]
  };

  constructor() {}

  ngOnInit() {}

  trackByFn(index: number, item: FlightCardViewModel) {
    return item.id;
  }
}
