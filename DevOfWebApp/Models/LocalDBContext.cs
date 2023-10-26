using System;
using System.Collections.Generic;
using DevOfWebApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevOfWebApp.Models;

public partial class LocalDBContext : DbContext
{
    public LocalDBContext()
    {
    }

    public LocalDBContext(DbContextOptions<LocalDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Институты> Институтыs { get; set; }

    public virtual DbSet<Преподаватели> Преподавателиs { get; set; }

    public virtual DbSet<УчёноеЗвание> УчёноеЗваниеs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddUserSecrets<LocalDBContext>()
               .Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString(nameof(LocalDBContext)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Cyrillic_General_CI_AI");

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.IdRole).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.IdUser).ValueGeneratedNever();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<Институты>(entity =>
        {
            entity.Property(e => e.IdИнститута).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Преподаватели>(entity =>
        {
            entity.Property(e => e.IdПреподавателя).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdИнститутаNavigation).WithMany(p => p.Преподавателиs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Преподаватели_Институты");

            entity.HasOne(d => d.КодУчёногоЗванияNavigation).WithMany(p => p.Преподавателиs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Преподава__Код у__4F47C5E3");
        });

        modelBuilder.Entity<УчёноеЗвание>(entity =>
        {
            entity.HasKey(e => e.КодУчёногоЗвания).HasName("PK__Учёное з__D4149455478ECBDB");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
