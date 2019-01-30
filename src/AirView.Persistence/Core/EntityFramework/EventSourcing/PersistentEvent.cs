using System;

namespace AirView.Persistence.Core.EntityFramework.EventSourcing
{
    public class PersistentEvent
    {
        public string Data { get; set; }

        public string Metadata { get; set; }

        public string Name { get; set; }

        public string StreamId { get; set; }

        public long StreamVersion { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}