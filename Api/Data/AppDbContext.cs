using Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();

    public DbSet<AppUser> Users => Set<AppUser>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


    protected override void  OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<RefreshToken>(entity =>    
        {
            entity.ToTable("RefreshTokens");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TokenHash).IsRequired();
            entity.HasOne(x => x.User)
                  .WithMany()
                  .HasForeignKey(x => x.UserId);
        });


        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
          .HasIndex(u => u.Email)
          .IsUnique();

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