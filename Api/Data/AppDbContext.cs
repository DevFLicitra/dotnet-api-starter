using Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();

    protected override void  OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Projects");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
               .HasMaxLength(200)
               .IsRequired();

            entity.Property(x => x.Description)
               .HasMaxLength(2000);

            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.UpdatedAt).IsRequired();

        });

    }


}