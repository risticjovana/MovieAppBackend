namespace MovieAPI;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models.User;
using MovieAPI.Models.TicketReservation;

public class AppDbContext : DbContext
{
     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

    public DbSet<User> Users { get; set; }
    public DbSet<RegularUser> RegularUsers { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<VisualContent> VisualContents { get; set; }
    public DbSet<Projection> Projections { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }


}
