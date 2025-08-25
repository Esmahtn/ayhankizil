using Microsoft.EntityFrameworkCore;
using ayhankizil.Models;

namespace ayhankizil.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Veritabanındaki tablo isimleriyle eşleştir
        public DbSet<Paylasim> Paylasimlar { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Paylasim entity'sini Paylasimlar tablosuna bağla
            modelBuilder.Entity<Paylasim>(entity =>
            {
                entity.ToTable("Paylasimlar"); // Veritabanındaki tablo adı
                entity.HasKey(e => e.Id);

                // Kolon isimleri veritabanıyla aynı
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.AdSoyad).HasColumnName("AdSoyad").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasColumnName("Email").HasMaxLength(150);
                entity.Property(e => e.Icerik).HasColumnName("Icerik").IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Foto1).HasColumnName("Foto1");
                entity.Property(e => e.Foto2).HasColumnName("Foto2");
                entity.Property(e => e.Foto3).HasColumnName("Foto3");
                entity.Property(e => e.Foto4).HasColumnName("Foto4");
                entity.Property(e => e.ResimYolu).HasColumnName("ResimYolu");
                entity.Property(e => e.Onayli).HasColumnName("Onayli").HasDefaultValue(false);
            });

            // Admin entity'sini Admins tablosuna bağla
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admins"); // Veritabanındaki tablo adı
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Username).HasColumnName("Username").IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).HasColumnName("PasswordHash").IsRequired();
                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.LastLoginDate).HasColumnName("LastLoginDate");
                entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);

                // Username unique olsun
                entity.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}