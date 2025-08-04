using System.ComponentModel.DataAnnotations;

namespace ayhankizil.Models
{
    public class Paylasim
    {
        public int Id { get; set; }

        [Required]
        public string? AdSoyad { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Icerik { get; set; }

        // Fotoğraf dosya adları veya yolları
        public string? Foto1 { get; set; }
        public string? Foto2 { get; set; }
        public string? Foto3 { get; set; }
        public string? Foto4 { get; set; }
    }
}
