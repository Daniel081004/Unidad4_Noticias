using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Unidad4_Noticias.Models.Entities;

public partial class NoticiasItesrcContext : DbContext
{
    public NoticiasItesrcContext()
    {
    }

    public NoticiasItesrcContext(DbContextOptions<NoticiasItesrcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comentarios> Comentarios { get; set; }

    public virtual DbSet<Noticias> Noticias { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=noticias_itesrc;user=root;password=root;port=3307", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.3.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Comentarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("comentarios");

            entity.HasIndex(e => e.NoticiaId, "idx_comentarios_noticia");

            entity.HasIndex(e => e.UsuarioId, "idx_comentarios_usuario");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("'1'")
                .HasColumnName("activo");
            entity.Property(e => e.Contenido)
                .HasColumnType("text")
                .HasColumnName("contenido")
                .UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.EsReportero)
                .HasDefaultValueSql("'0'")
                .HasColumnName("es_reportero");
            entity.Property(e => e.FechaComentario)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_comentario");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .HasColumnName("nombre_usuario")
                .UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.NoticiaId)
                .HasColumnType("int(11)")
                .HasColumnName("noticia_id");
            entity.Property(e => e.UsuarioId)
                .HasColumnType("int(11)")
                .HasColumnName("usuario_id");

            entity.HasOne(d => d.Noticia).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.NoticiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comentarios_ibfk_2");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("comentarios_ibfk_1");
        });

        modelBuilder.Entity<Noticias>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("noticias");

            entity.HasIndex(e => e.FechaPublicacion, "idx_noticias_fecha");

            entity.HasIndex(e => e.UsuarioId, "idx_noticias_usuario");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Activa)
                .HasDefaultValueSql("'1'")
                .HasColumnName("activa");
            entity.Property(e => e.Contenido)
                .HasColumnName("contenido")
                .UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.FechaEvento).HasColumnName("fecha_evento");
            entity.Property(e => e.FechaPublicacion)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_publicacion");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(500)
                .HasColumnName("imagen_url")
                .UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Titulo)
                .HasMaxLength(255)
                .HasColumnName("titulo")
                .UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UsuarioId)
                .HasColumnType("int(11)")
                .HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Noticias)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("noticias_ibfk_1");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Nombre, "nombre").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasColumnName("descripcion")
                .UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre")
                .UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.Activo, "idx_usuarios_activo");

            entity.HasIndex(e => e.RolId, "idx_usuarios_rol");

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("'1'")
                .HasColumnName("activo");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email")
                .UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash")
                .UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.RolId)
                .HasColumnType("int(11)")
                .HasColumnName("rol_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username")
                .UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuarios_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
