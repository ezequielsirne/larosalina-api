using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LaRosalinaAPI.Models;

public partial class LaRosalinaDbContext : DbContext
{
    public LaRosalinaDbContext()
    {
    }

    public LaRosalinaDbContext(DbContextOptions<LaRosalinaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<categorias_gasto> categorias_gastos { get; set; }

    public virtual DbSet<comprobante> comprobantes { get; set; }

    public virtual DbSet<cuenta> cuentas { get; set; }

    public virtual DbSet<departamento> departamentos { get; set; }

    public virtual DbSet<estado_reserva> estado_reservas { get; set; }

    public virtual DbSet<gasto> gastos { get; set; }

    public virtual DbSet<huespede> huespedes { get; set; }

    public virtual DbSet<movimiento> movimientos { get; set; }

    public virtual DbSet<movimientos_tipo> movimientos_tipos { get; set; }

    public virtual DbSet<presupuesto> presupuestos { get; set; }

    public virtual DbSet<presupuestos_detalle> presupuestos_detalles { get; set; }

    public virtual DbSet<reserva> reservas { get; set; }

    public virtual DbSet<usuario> usuarios { get; set; }

    public virtual DbSet<web_LogErrorMessage> web_LogErrorMessages { get; set; }

    public virtual DbSet<web_suceso> web_sucesos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=ConnectionStrings:SqlConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<categorias_gasto>(entity =>
        {
            entity.Property(e => e.descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.f_alta).HasColumnType("datetime");
            entity.Property(e => e.f_baja).HasColumnType("datetime");
            entity.Property(e => e.f_mod).HasColumnType("datetime");
        });

        modelBuilder.Entity<comprobante>(entity =>
        {
            entity.Property(e => e.archivo).IsUnicode(false);
            entity.Property(e => e.archivo_corto).IsUnicode(false);
            entity.Property(e => e.extension)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.f_alta).HasColumnType("datetime");
            entity.Property(e => e.f_baja).HasColumnType("datetime");
            entity.Property(e => e.id_random)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<cuenta>(entity =>
        {
            entity.Property(e => e.descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.f_alta).HasColumnType("datetime");
            entity.Property(e => e.f_baja).HasColumnType("datetime");
            entity.Property(e => e.f_mod).HasColumnType("datetime");
        });

        modelBuilder.Entity<departamento>(entity =>
        {
            entity.Property(e => e.descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.planta)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.vista)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<estado_reserva>(entity =>
        {
            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<gasto>(entity =>
        {
            entity.Property(e => e.descripcion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.f_alta).HasColumnType("datetime");
            entity.Property(e => e.f_baja).HasColumnType("datetime");
            entity.Property(e => e.f_mod).HasColumnType("datetime");
            entity.Property(e => e.fecha).HasColumnType("datetime");
            entity.Property(e => e.id_random)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.importe).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.observaciones).HasColumnType("text");

            entity.HasOne(d => d.categoriaNavigation).WithMany(p => p.gastos)
                .HasForeignKey(d => d.categoria)
                .HasConstraintName("FK_gastos_categorias_gastos");

            entity.HasOne(d => d.cuentaNavigation).WithMany(p => p.gastos)
                .HasForeignKey(d => d.cuenta)
                .HasConstraintName("FK_gastos_cuentas");
        });

        modelBuilder.Entity<huespede>(entity =>
        {
            entity.Property(e => e.checkin).HasColumnType("datetime");
            entity.Property(e => e.checkout).HasColumnType("datetime");
            entity.Property(e => e.domicilio)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.f_alta).HasColumnType("datetime");
            entity.Property(e => e.f_baja).HasColumnType("datetime");
            entity.Property(e => e.f_mod).HasColumnType("datetime");
            entity.Property(e => e.nombre_apellido)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.patente)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.id_reservaNavigation).WithMany(p => p.huespedes)
                .HasForeignKey(d => d.id_reserva)
                .HasConstraintName("FK_huespedes_reservas");
        });

        modelBuilder.Entity<movimiento>(entity =>
        {
            entity.Property(e => e.descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.f_alta).HasColumnType("datetime");
            entity.Property(e => e.f_baja).HasColumnType("datetime");
            entity.Property(e => e.f_mod).HasColumnType("datetime");
            entity.Property(e => e.fecha).HasColumnType("datetime");
            entity.Property(e => e.id_random)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.id_transferencia)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.movimiento1)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("movimiento");

            entity.HasOne(d => d.cuentaNavigation).WithMany(p => p.movimientos)
                .HasForeignKey(d => d.cuenta)
                .HasConstraintName("FK_movimientos_cuentas");

            entity.HasOne(d => d.id_gastoNavigation).WithMany(p => p.movimientos)
                .HasForeignKey(d => d.id_gasto)
                .HasConstraintName("FK_movimientos_gastos");

            entity.HasOne(d => d.id_reservaNavigation).WithMany(p => p.movimientos)
                .HasForeignKey(d => d.id_reserva)
                .HasConstraintName("FK_movimientos_reservas");

            entity.HasOne(d => d.tipoNavigation).WithMany(p => p.movimientos)
                .HasForeignKey(d => d.tipo)
                .HasConstraintName("FK_movimientos_movimientos_tipos");
        });

        modelBuilder.Entity<movimientos_tipo>(entity =>
        {
            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<presupuesto>(entity =>
        {
            entity.Property(e => e.cuit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.domicilio)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.f_alta).HasColumnType("datetime");
            entity.Property(e => e.f_baja).HasColumnType("datetime");
            entity.Property(e => e.f_mod).HasColumnType("datetime");
            entity.Property(e => e.fecha).HasColumnType("datetime");
            entity.Property(e => e.forma_de_pago)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.total).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<presupuestos_detalle>(entity =>
        {
            entity.ToTable("presupuestos_detalle");

            entity.Property(e => e.descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.f_alta).HasColumnType("datetime");
            entity.Property(e => e.f_baja).HasColumnType("datetime");
            entity.Property(e => e.f_mod).HasColumnType("datetime");
            entity.Property(e => e.importe).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.id_presupuestoNavigation).WithMany(p => p.presupuestos_detalles)
                .HasForeignKey(d => d.id_presupuesto)
                .HasConstraintName("FK_presupuestos_detalle_presupuestos");
        });

        modelBuilder.Entity<reserva>(entity =>
        {
            entity.Property(e => e.checkin).HasColumnType("datetime");
            entity.Property(e => e.checkout).HasColumnType("datetime");
            entity.Property(e => e.f_alta).HasColumnType("datetime");
            entity.Property(e => e.f_baja).HasColumnType("datetime");
            entity.Property(e => e.f_mod).HasColumnType("datetime");
            entity.Property(e => e.localidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.mail)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.nombre_apellido)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.observaciones).HasColumnType("text");
            entity.Property(e => e.telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.departamentoNavigation).WithMany(p => p.reservas)
                .HasForeignKey(d => d.departamento)
                .HasConstraintName("FK_reserva_departamento");

            entity.HasOne(d => d.estadoNavigation).WithMany(p => p.reservas)
                .HasForeignKey(d => d.estado)
                .HasConstraintName("FK_reservas_estado");
        });

        modelBuilder.Entity<usuario>(entity =>
        {
            entity.Property(e => e.f_alta).HasColumnType("datetime");
            entity.Property(e => e.f_baja).HasColumnType("datetime");
            entity.Property(e => e.f_mod).HasColumnType("datetime");
            entity.Property(e => e.last_login).HasColumnType("datetime");
            entity.Property(e => e.nick)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.nombre_apellido)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.password)
                .HasMaxLength(32)
                .IsUnicode(false);
        });

        modelBuilder.Entity<web_LogErrorMessage>(entity =>
        {
            entity.ToTable("web_LogErrorMessage");

            entity.Property(e => e.f_error).HasColumnType("datetime");
            entity.Property(e => e.logErrorMessage).HasColumnType("text");
            entity.Property(e => e.origen).HasColumnType("text");
            entity.Property(e => e.querry).HasColumnType("text");
        });

        modelBuilder.Entity<web_suceso>(entity =>
        {
            entity.Property(e => e.descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.f_suceso).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
