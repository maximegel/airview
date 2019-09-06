import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatBadgeModule } from '@angular/material/badge';
import { ArrowComponent } from './arrow.component';

@NgModule({
  declarations: [ArrowComponent],
  imports: [CommonModule, FlexLayoutModule, MatBadgeModule],
  exports: [ArrowComponent]
})
export class ArrowModule {}
