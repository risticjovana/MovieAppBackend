namespace MovieAPI;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models.User;

public class AppDbContext : DbContext
{
     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

    public DbSet<User> Users { get; set; }
    public DbSet<RegularUser> RegularUsers { get; set; }
}
