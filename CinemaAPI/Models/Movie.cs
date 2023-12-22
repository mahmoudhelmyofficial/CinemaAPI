using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string StoreLine { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public byte[] Poster { get; set; }
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
    }
}
