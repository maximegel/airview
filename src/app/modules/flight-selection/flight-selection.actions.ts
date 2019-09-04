function getType(actionClass: { name: string }) {
  return `[FlightSelection] ${actionClass.name}`;
}

export class HideFlightDetail {
  static readonly type = getType(HideFlightDetail);
  constructor() {}
}

export class QueryFlights {
  static readonly type = getType(QueryFlights);
  constructor() {}
}

export class SelectFlight {
  static readonly type = getType(SelectFlight);
  constructor(public id: string) {}
}

export class ShowFlightDetail {
  static readonly type = getType(ShowFlightDetail);
  constructor(public id: string) {}
}
