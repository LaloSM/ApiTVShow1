using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TvShow.Entity;

public partial class ApitvshowContext : DbContext
{
    // Constructor por defecto del contexto de base de datos
    // No realiza ninguna acción específica en este caso
    public ApitvshowContext()
    {
    }

    // Constructor que recibe opciones de configuración para el contexto de base de datos
    // Llama al constructor base con las opciones proporcionadas
    public ApitvshowContext(DbContextOptions<ApitvshowContext> options)
        : base(options)
    {
    }

    // Propiedad virtual que representa la tabla Tvshows en la base de datos
    public virtual DbSet<Tvshow> Tvshows { get; set; }

    // Propiedad virtual que representa la tabla Usuarios en la base de datos
    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tvshow>(entity =>
        {
            entity.HasKey(e => e.IdTvShow);

            entity.ToTable("TVShow");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Tvshows)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_TVShow_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.DateRegistration).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Mail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telephone)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
