using Microsoft.EntityFrameworkCore;
using TableGrocer.EFCore.Models;
using Xamarin.Essentials;
namespace TableGrocer.EFCore
{
    public class AppDbContext : DbContext
    {
        public DbSet<GroceryItem> GroceryItems { get; set; }
        public DbSet<GroceryRun> GroceryRuns { get; set; }

        public AppDbContext()
        {
            
            SQLitePCL.Batteries_V2.Init();
            this.Database.EnsureCreated();

        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "grocery.db3");
            optionsBuilder
                .UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroceryRun>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            
            modelBuilder.Entity<GroceryItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            
            modelBuilder.Entity<GroceryRun>()
                .HasMany(gr => gr.GroceryItems)
                .WithOne(gi => gi.GroceryRun)
                .HasForeignKey(gi => gi.GroceryRunId);

            base.OnModelCreating(modelBuilder);
        }
    }
}