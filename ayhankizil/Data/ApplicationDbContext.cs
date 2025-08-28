using Microsoft.EntityFrameworkCore;
using ayhankizil.Models;

namespace ayhankizil.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Paylasim> Paylasimlar { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Paylasim entity
            modelBuilder.Entity<Paylasim>(entity =>
            {
                entity.ToTable("Paylasimlar");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("Id")
                      .ValueGeneratedOnAdd(); // otomatik artan

                entity.Property(e => e.AdSoyad)
                      .HasColumnName("AdSoyad")
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Email)
                      .HasColumnName("Email")
                      .HasMaxLength(100);

                entity.Property(e => e.Icerik)
                      .HasColumnName("Icerik")
                      .HasColumnType("NVARCHAR(MAX)")
                      .IsRequired();

                entity.Property(e => e.Foto1).HasColumnName("Foto1");
                entity.Property(e => e.Foto2).HasColumnName("Foto2");
                entity.Property(e => e.Foto3).HasColumnName("Foto3");
                entity.Property(e => e.Foto4).HasColumnName("Foto4");
                entity.Property(e => e.ResimYolu).HasColumnName("ResimYolu");
                entity.Property(e => e.Onayli).HasColumnName("Onayli").HasDefaultValue(false);

                entity.Property(e => e.EklenmeTarihi)
                      .HasColumnName("EklemeTarihi")
                      .HasDefaultValueSql("GETDATE()");
            });

            // Admin entity
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admins");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.Username).HasColumnName("Username").IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).HasColumnName("PasswordHash").IsRequired();
                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.LastLoginDate).HasColumnName("LastLoginDate");
                entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);

                entity.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}
