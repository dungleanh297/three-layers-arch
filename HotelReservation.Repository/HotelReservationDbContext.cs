using Microsoft.EntityFrameworkCore;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Repository;

public class HotelReservationDbContext : DbContext
{
    public HotelReservationDbContext()
    {
        
    }

    public HotelReservationDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Reservation> Reservations { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().Property(e => e.Price).HasPrecision(10, 2);
        modelBuilder.Entity<Reservation>().Property(e => e.RoomPrice).HasPrecision(10, 2);
        base.OnModelCreating(modelBuilder);
    }
}
