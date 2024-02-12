using Microsoft.EntityFrameworkCore;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    public DbSet<Hotel> Hotels { get; set; }
}
