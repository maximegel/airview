import { TestBed } from '@angular/core/testing';

import { FlightSelectionService } from './flight-selection.service';

describe('FlightSelectionService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FlightSelectionService = TestBed.get(FlightSelectionService);
    expect(service).toBeTruthy();
  });
});
