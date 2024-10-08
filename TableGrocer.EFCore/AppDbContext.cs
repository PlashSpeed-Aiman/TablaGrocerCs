﻿using Microsoft.EntityFrameworkCore;
using TableGrocer.EFCore.Models;
using Xamarin.Essentials;
namespace TableGrocer.EFCore
{
    public class AppDbContext : DbContext
    {
        public string? DbPath;
        public DbSet<GroceryItem> GroceryItems { get; set; }
        public DbSet<GroceryRun> GroceryRuns { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateItem> TemplateItems { get; set; }
        
        public AppDbContext()
        {
             Database.EnsureCreated();    
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DbPath = Path.Combine(FileSystem.AppDataDirectory, "grocery.db3");
            optionsBuilder
                .UseSqlite($"Filename={DbPath}");
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

            modelBuilder.Entity<Template>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TemplateItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            
            
            modelBuilder.Entity<GroceryRun>()
                .HasMany(gr => gr.GroceryItems)
                .WithOne(gi => gi.GroceryRun)
                .HasForeignKey(gi => gi.GroceryRunId);

            modelBuilder.Entity<Template>()
                .HasMany(tmp => tmp.TemplateItems)
                .WithOne(tmpi => tmpi.Template)
                .HasForeignKey(tmp => tmp.TemplateId);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}