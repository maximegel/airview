import { ChangeDetectionStrategy, Component, ContentChild, Input, OnInit, TrackByFunction } from '@angular/core';
import { FlightListActiveItemDirective } from './flight-list-active-item.directive';
import { FlightListItemDirective } from './flight-list-item.directive';

@Component({
  selector: 'av-flight-list',
  templateUrl: './flight-list.component.html',
  styleUrls: ['./flight-list.component.scss'],
  host: {
    class: 'av-flight-list'
  },
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FlightListComponent implements OnInit {
  @Input() items: { id: string }[] = [];
  @Input() active: string | null = null;
  @ContentChild(FlightListActiveItemDirective, { static: false }) activeOutlet: FlightListActiveItemDirective;
  @ContentChild(FlightListItemDirective, { static: false }) itemOutlet: FlightListItemDirective;

  get trackByFn(): TrackByFunction<{ id: string }> {
    return (_, item) => item.id;
  }

  ngOnInit() {}
}
