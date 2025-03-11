using DatabaseService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Repository;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Words> Words { get; set; }
    public DbSet<Emails> Emails { get; set; }
    public DbSet<Occurrences> Occurrences { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Occurrences>()
            .HasOne(occurrences => occurrences.Word)
            .WithMany()
            .HasForeignKey(occurrences => occurrences.WordId);
        
        modelBuilder.Entity<Occurrences>()
            .HasOne(occurrences => occurrences.Email)
            .WithMany()
            .HasForeignKey(occurrences => occurrences.FileId);
        
        modelBuilder.Entity<Occurrences>()
            .Property(o => o.Count)
            .HasDefaultValue(1) // Ensure Count is correctly handled
            .IsRequired();
        
        base.OnModelCreating(modelBuilder);
    }
}