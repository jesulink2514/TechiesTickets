using Microsoft.EntityFrameworkCore;
using Techies.Registration.Models;

namespace Techies.Registration.Infrastructure;

public class RegistrationDbContext : DbContext
{
    public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options) : base(options)
    {
    }

    public DbSet<Seat> Seats { get; set; }
    public DbSet<Event> Events { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Seat>().ToTable("Seats");
        modelBuilder.Entity<Seat>().Property(e => e.Version)
        .IsRowVersion()
        .IsConcurrencyToken();
        modelBuilder.Entity<Event>().ToTable("Events");
    }
}