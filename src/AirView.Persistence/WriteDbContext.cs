using System.Diagnostics.CodeAnalysis;
using AirView.Persistence.Core.EntityFramework.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace AirView.Persistence
{
    public class WriteDbContext : EventSourcedDbContext
    {
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public WriteDbContext(DbContextOptions<WriteDbContext> options) :
            base(options)
        {
        }
    }
}