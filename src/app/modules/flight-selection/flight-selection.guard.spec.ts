import { TestBed, async, inject } from '@angular/core/testing';

import { FlightSelectionGuard } from './flight-selection.guard';

describe('FlightSelectionGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FlightSelectionGuard]
    });
  });

  it('should ...', inject([FlightSelectionGuard], (guard: FlightSelectionGuard) => {
    expect(guard).toBeTruthy();
  }));
});
