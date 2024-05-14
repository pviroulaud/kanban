using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace entidadesKanban.modelo
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<kbn_estado> kbn_estado { get; set; } = null!;
        public virtual DbSet<kbn_incidencia> kbn_incidencia { get; set; } = null!;
        public virtual DbSet<kbn_log> kbn_log { get; set; } = null!;
        public virtual DbSet<kbn_proyecto> kbn_proyecto { get; set; } = null!;
        public virtual DbSet<kbn_registroTiempo> kbn_registroTiempo { get; set; } = null!;
        public virtual DbSet<kbn_tarea> kbn_tarea { get; set; } = null!;
        public virtual DbSet<kbn_tipoIncidencia> kbn_tipoIncidencia { get; set; } = null!;
        public virtual DbSet<kbn_tipoTarea> kbn_tipoTarea { get; set; } = null!;
        public virtual DbSet<kbn_usuario> kbn_usuario { get; set; } = null!;
        public virtual DbSet<kbn_usuarioPassword> kbn_usuarioPassword { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DELL-PABLO\\sqlexpress;Database=kanban;persist security info=True;user id=sa;password=ade@D47@3a53;;multipleactiveresultsets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<kbn_incidencia>(entity =>
            {
                entity.Property(e => e.fechaCreacion).HasColumnType("datetime");

                entity.HasOne(d => d.estado)
                    .WithMany(p => p.kbn_incidencia)
                    .HasForeignKey(d => d.estadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__kbn_incid__estad__32E0915F");

                entity.HasOne(d => d.proyecto)
                    .WithMany(p => p.kbn_incidencia)
                    .HasForeignKey(d => d.proyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__kbn_incid__proye__30F848ED");

                entity.HasOne(d => d.tipoIncidencia)
                    .WithMany(p => p.kbn_incidencia)
                    .HasForeignKey(d => d.tipoIncidenciaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__kbn_incid__tipoI__31EC6D26");

                entity.HasOne(d => d.usuarioCreador)
                    .WithMany(p => p.kbn_incidenciausuarioCreador)
                    .HasForeignKey(d => d.usuarioCreadorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__kbn_incid__usuar__33D4B598");

                entity.HasOne(d => d.usuarioResponsable)
                    .WithMany(p => p.kbn_incidenciausuarioResponsable)
                    .HasForeignKey(d => d.usuarioResponsableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__kbn_incid__usuar__34C8D9D1");
            });

            modelBuilder.Entity<kbn_log>(entity =>
            {
                entity.Property(e => e.fechaHora).HasColumnType("datetime");
            });

            modelBuilder.Entity<kbn_registroTiempo>(entity =>
            {
                entity.Property(e => e.ejecucion)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.fechaEjecucion).HasColumnType("datetime");

                entity.Property(e => e.fechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<kbn_tarea>(entity =>
            {
                entity.Property(e => e.ejecucion)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.estimacion)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.fechaCreacion).HasColumnType("datetime");

                entity.HasOne(d => d.estado)
                    .WithMany(p => p.kbn_tarea)
                    .HasForeignKey(d => d.estadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__kbn_tarea__estad__3A81B327");

                entity.HasOne(d => d.incidencia)
                    .WithMany(p => p.kbn_tarea)
                    .HasForeignKey(d => d.incidenciaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__kbn_tarea__incid__3D5E1FD2");

                entity.HasOne(d => d.tipoTarea)
                    .WithMany(p => p.kbn_tarea)
                    .HasForeignKey(d => d.tipoTareaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__kbn_tarea__tipoT__398D8EEE");

                entity.HasOne(d => d.usuarioCreador)
                    .WithMany(p => p.kbn_tareausuarioCreador)
                    .HasForeignKey(d => d.usuarioCreadorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__kbn_tarea__usuar__3B75D760");

                entity.HasOne(d => d.usuarioResponsable)
                    .WithMany(p => p.kbn_tareausuarioResponsable)
                    .HasForeignKey(d => d.usuarioResponsableId)
                    .HasConstraintName("FK__kbn_tarea__usuar__3C69FB99");
            });

            modelBuilder.Entity<kbn_usuarioPassword>(entity =>
            {
                entity.HasOne(d => d.usuario)
                    .WithMany(p => p.kbn_usuarioPassword)
                    .HasForeignKey(d => d.usuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__kbn_usuar__usuar__267ABA7A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
