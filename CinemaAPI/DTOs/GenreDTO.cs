using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTOs
{
    public class GenreDTO
    {
        [Length(3,50)]
        public string? GenreName { get; set; }
    }
}
