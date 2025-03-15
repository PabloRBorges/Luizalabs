using LogisticaVertical.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaVertical.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurando os IDs para não serem gerados automaticamente
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedNever();  // Impede que o banco gere automaticamente o ID

            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);
            
            // Configuração da precisão para valores decimais
            modelBuilder.Entity<Order>()
                .Property(o => o.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Value)
                .HasPrecision(18, 2);


            // Configuração dos relacionamentos (corrigida)
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasMany(u => u.Orders)
                      .WithOne(o => o.User)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.HasMany(o => o.Products)
                      .WithOne(p => p.Order)
                      .HasForeignKey(p => p.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
