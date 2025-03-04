using DatabaseService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Repository;

public class DatabaseContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        =>optionsBuilder.UseNpgsql("Host=localhost;Database=DSL_Compulsory_1;Username=postgres;Password=postgres");

    public DbSet<Words> Words { get; set; }
    public DbSet<Files> Files { get; set; }
    public DbSet<Occurrences> Occurrences { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Occurrences>()
            .HasOne(occurrences => occurrences.Word)
            .WithMany()
            .HasForeignKey(occurrences => occurrences.WordId);
        
        modelBuilder.Entity<Occurrences>()
            .HasOne(occurrences => occurrences.File)
            .WithMany()
            .HasForeignKey(occurrences => occurrences.FileId);
        
        base.OnModelCreating(modelBuilder);
    }
}