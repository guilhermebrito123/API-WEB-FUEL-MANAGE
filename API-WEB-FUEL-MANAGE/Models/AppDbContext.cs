﻿using Microsoft.EntityFrameworkCore;

namespace API_WEB_FUEL_MANAGE.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
             
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<VeiculoUsuarios>()
                .HasKey(c => new { c.VeiculoId, c.UsuarioId });//Definindo que o model VeiculoUsuarios possui mais de uma primary key, possui uma chave composta
            builder.Entity<VeiculoUsuarios>()
                .HasOne(c => c.Veiculo).WithMany(c => c.Usuarios)
                .HasForeignKey(c => c.VeiculoId);
            builder.Entity<VeiculoUsuarios>()
                .HasOne(c => c.Usuario).WithMany(c => c.Veiculos)
                .HasForeignKey(c => c.UsuarioId);
        }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Consumo> Consumos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<VeiculoUsuarios> VeiculosUsuarios { get; set; }

    }

}
