using DatabaseService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Repository;

public class DatabaseContext : DbContext
{
    private const string Username = "postgres";
    private const string Password = "postgres";
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        =>optionsBuilder.UseNpgsql($"Host=db-postgres;Port=5432;Database=dls;Username={Username};Password={Password};Trust Server Certificate=true;");

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