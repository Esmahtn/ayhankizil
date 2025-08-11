using System.Collections.Generic;

namespace ayhankizil.Models
{
    public class PaylasimViewModel
    {
        public List<Paylasim> Paylasimlar { get; set; } = new List<Paylasim>();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
    }
}
