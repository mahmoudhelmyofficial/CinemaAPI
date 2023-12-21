namespace CinemaAPI.DTOs
{
    public class MovieDTO
    {
        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(2000)]
        public string? StoreLine { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public IFormFile? Poster { get; set; }
        public int GenreId { get; set; }
    }
}
