namespace MovieAPI;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models.User;
using MovieAPI.Models.TicketReservation;
using MovieAPI.Models.Content;
using MovieAPI.Models.Collections;

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
    public DbSet<TVSeries> TVSeries { get; set; }
    public DbSet<VisualContent> VisualContents { get; set; }
    public DbSet<Projection> Projections { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Ticket> Tickets {  get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<Pripada> Pripada { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<MovieCollection> MovieCollections { get; set; }
    public DbSet<CollectionItem> CollectionItems { get; set; }
    public DbSet<EditorialCollection> EditorialCollections { get; set; }
    public DbSet<PersonalCollection> PersonalCollections { get; set; }
    public DbSet<SavedCollection> SavedCollections { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Critique> Critiques { get; set; }
    public DbSet<UserActivity> UserActivities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pripada>()
            .HasKey(p => new { p.ContentId, p.GenreId });
        modelBuilder.Entity<CollectionItem>()
            .HasKey(ci => new { ci.ContentId, ci.CollectionId });
        modelBuilder.Entity<SavedCollection>()
            .HasKey(sc => new { sc.UserId, sc.CollectionId });
        modelBuilder.Entity<Follow>()
            .HasKey(f => new { f.FollowerId, f.FolloweeId });

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany()
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Followee)
            .WithMany()
            .HasForeignKey(f => f.FolloweeId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
