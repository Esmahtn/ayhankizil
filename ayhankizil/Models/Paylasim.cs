using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ayhankizil.Models
{
    [Table("Paylasimlar")] // Veritabanındaki tablo adı
    public class Paylasim
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("AdSoyad")]
        public string AdSoyad { get; set; }

        [StringLength(150)]
        [Column("Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(1000)]
        [Column("Icerik")]
        public string Icerik { get; set; }

        [Column("Foto1")]
        public string? Foto1 { get; set; }

        [Column("Foto2")]
        public string? Foto2 { get; set; }

        [Column("Foto3")]
        public string? Foto3 { get; set; }

        [Column("Foto4")]
        public string? Foto4 { get; set; }

        [Column("ResimYolu")]
        public string? ResimYolu { get; set; }

        [Column("Onayli")]
        public bool Onayli { get; set; } = false;

        // TARİH ALANI EKLENDİ
        [Column("EklenmeTarihi")]
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
    }
}