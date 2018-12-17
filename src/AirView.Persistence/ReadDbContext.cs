﻿using System.Diagnostics.CodeAnalysis;
using AirView.Domain;
using Microsoft.EntityFrameworkCore;

namespace AirView.Persistence
{
    public class ReadDbContext : DbContext
    {
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public ReadDbContext(DbContextOptions<ReadDbContext> options) :
            base(options)
        {
        }

        public DbSet<FlightProjection> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}