import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'av-arrow',
  templateUrl: './arrow.component.html',
  styleUrls: ['./arrow.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ArrowComponent implements OnInit {
  @Input() badge = '';
  @Input() direction: 'left' | 'right' = 'right';

  constructor() {}

  ngOnInit() {}
}
