namespace ayhankizil.Models
{
    public class PaylasimViewModel
    {
        public List<Paylasim> Paylasimlar { get; set; } = new List<Paylasim>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 0;
    }
}