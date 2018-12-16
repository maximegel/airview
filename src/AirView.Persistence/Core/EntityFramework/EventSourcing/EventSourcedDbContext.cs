using Microsoft.EntityFrameworkCore;

namespace AirView.Persistence.Core.EntityFramework.EventSourcing
{
    public class EventSourcedDbContext : DbContext
    {
        public EventSourcedDbContext(DbContextOptions options) :
            base(options)
        {
        }

        public DbSet<PersistentEvent> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersistentEvent>()
                .HasKey(@event => new {@event.StreamId, @event.StreamVersion});
        }
    }
}