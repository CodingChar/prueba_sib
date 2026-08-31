using Microsoft.EntityFrameworkCore;
using SB.Management.Domain.Entities;

namespace SB.Management.Infrastructure.Persistence
{
    public class SbGestionPagosDbContext : DbContext
    {
        public SbGestionPagosDbContext(DbContextOptions<SbGestionPagosDbContext> options)
            : base(options) { }

        public DbSet<Empleado> Empleados => Set<Empleado>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<Pago> Pagos => Set<Pago>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeo TPT: cada clase de la jerarquía de Empleado -> su propia tabla
            modelBuilder.Entity<Empleado>().ToTable("Empleado");
            modelBuilder.Entity<EmpleadoAsalariado>().ToTable("EmpleadoAsalariado");
            modelBuilder.Entity<EmpleadoPorHora>().ToTable("EmpleadoPorHora");
            modelBuilder.Entity<EmpleadoPorComision>().ToTable("EmpleadoPorComision");
            modelBuilder.Entity<EmpleadoAsalariadoComision>().ToTable("EmpleadoAsalariadoComision");

            modelBuilder.Entity<Empleado>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.ApellidoPaterno).HasMaxLength(50).IsRequired();
                e.Property(x => x.PrimerNombre).HasMaxLength(50);
                e.Property(x => x.NumeroSeguroSocial).HasMaxLength(20).IsRequired();
                e.HasIndex(x => x.NumeroSeguroSocial).IsUnique();
                e.Property(x => x.Departamento).HasMaxLength(100).IsRequired();
                e.Property(x => x.Estado).HasMaxLength(20).IsRequired();
            });

            modelBuilder.Entity<Rol>(r =>
            {
                r.HasKey(x => x.Id);
                r.Property(x => x.Nombre).HasMaxLength(20).IsRequired();
            });

            modelBuilder.Entity<Usuario>(u =>
            {
                u.HasKey(x => x.Id);
                u.Property(x => x.Username).HasMaxLength(50).IsRequired();
                u.HasIndex(x => x.Username).IsUnique();
                u.Property(x => x.PasswordHash).HasMaxLength(256).IsRequired();

                u.HasOne(x => x.Rol)
                    .WithMany(r => r.Usuarios)
                    .HasForeignKey(x => x.RolId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Pago>(p =>
            {
                p.HasKey(x => x.Id);
                p.Property(x => x.MontoCalculado).HasColumnType("decimal(14,2)");
                p.Property(x => x.DetalleCalculo).HasMaxLength(500);

                p.HasOne(x => x.Empleado)
                    .WithMany(emp => emp.Pagos)
                    .HasForeignKey(x => x.EmpleadoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmpleadoAsalariado>().Property(x => x.SalarioSemanal).HasColumnType("decimal(12,2)");
            modelBuilder.Entity<EmpleadoPorHora>().Property(x => x.SueldoPorHora).HasColumnType("decimal(12,2)");
            modelBuilder.Entity<EmpleadoPorHora>().Property(x => x.HorasTrabajadas).HasColumnType("decimal(6,2)");
            modelBuilder.Entity<EmpleadoPorComision>().Property(x => x.VentasBrutas).HasColumnType("decimal(14,2)");
            modelBuilder.Entity<EmpleadoPorComision>().Property(x => x.TarifaComision).HasColumnType("decimal(6,4)");
            modelBuilder.Entity<EmpleadoAsalariadoComision>().Property(x => x.VentasBrutas).HasColumnType("decimal(14,2)");
            modelBuilder.Entity<EmpleadoAsalariadoComision>().Property(x => x.TarifaComision).HasColumnType("decimal(6,4)");
            modelBuilder.Entity<EmpleadoAsalariadoComision>().Property(x => x.SalarioBase).HasColumnType("decimal(12,2)");
        }
    }
}