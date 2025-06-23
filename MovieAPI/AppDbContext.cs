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
    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<Critic> Critics { get; set; }
    public DbSet<Editor> Editors { get; set; }
    public DbSet<Moderator> Moderators { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<VisualContent> VisualContents { get; set; }
    public DbSet<Projection> Projections { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Ticket> Tickets {  get; set; }
    public DbSet<Request> Requests { get; set; }



}
