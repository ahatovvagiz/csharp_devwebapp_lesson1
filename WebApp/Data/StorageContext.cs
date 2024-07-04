﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using WebApp.Models;

namespace WebApp.Data
{
    public class StorageContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductGroup> ProductGroups { get; set; }
        public virtual DbSet<Storage> Storages { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Data Source =.\\SQLEXPRESS; Initial Catalog = Products; Trusted_Connection=True; TrustServerCertificate=True").UseLazyLoadingProxies().LogTo(Console.WriteLine);
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql("Host=localhost;Database=web_lesson1;Username=postgres;Password=example").UseLazyLoadingProxies().LogTo(Console.WriteLine);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductGroup>(entity =>
            {

                entity.HasKey(pg => pg.Id)
                      .HasName("product_group_pk");

                entity.ToTable("category");

                entity.Property(pg => pg.Name)
                      .HasColumnName("name")
                      .HasMaxLength(255);
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id)
                      .HasName("product_pk");

                entity.Property(p => p.Name)
                      .HasColumnName("name")
                      .HasMaxLength(255);

                entity.HasOne(p => p.ProductGroup).WithMany(p => p.Products).HasForeignKey(p => p.ProductGroupId);
            });
            modelBuilder.Entity<Storage>(entity =>
            {
                entity.HasKey(s => s.Id)
                      .HasName("storage_pk");

                entity.HasOne(s => s.Product).WithMany(p => p.Storages).HasForeignKey(p => p.ProductId);
            });

        }
    }
}