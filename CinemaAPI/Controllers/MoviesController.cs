using CinemaAPI.DTOs;
using CinemaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.JSInterop.Infrastructure;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly List<string> _allowedExtensions = [".jpg", ".png"];
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesAsync()
        {
            var movies = await _context.Movies.Include(m => m.Genre)
                .OrderByDescending(r => r.Rate).ToListAsync();

            return Ok(movies);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieAsync(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (id == 0 || movie is null)
                return NotFound($"There's no movie matches this Id {id} !");

            return Ok(movie); 
        }

        [HttpPost]
        public async Task<IActionResult> AddMovieAsync([FromForm] MovieDTO dto)
        {
            if (dto.Poster is null)
                return BadRequest("Poster is required");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only '.png' & '.jpg' are allowed ");

            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

            if(!isValidGenre)
                return BadRequest("No Genre Found Matches your choice ..!");

            using var dataStream =new MemoryStream();

            await dto.Poster.CopyToAsync(dataStream);

            var movie = new Movie()
            {
                Title = dto.Title,
                Rate = dto.Rate,
                Year = dto.Year,
                StoreLine = dto.StoreLine,
                GenreId = dto.GenreId,
                Poster = dataStream.ToArray()
            };

            await _context.AddAsync(movie);
            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovieAsync(int id, [FromForm] MovieDTO dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = await _context.Movies.FindAsync(id);

            if (movie is null) return NotFound();

            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

            if (!isValidGenre)
                return BadRequest("No Genre Found Matches your choice ..!");

            if(dto.Poster is not null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only '.png' & '.jpg' are allowed ");

                using var dataStream = new MemoryStream();

                await dto.Poster.CopyToAsync(dataStream);
                
                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;
            movie.Rate = dto.Rate;
            movie.Year = dto.Year;
            movie.StoreLine = dto.StoreLine;
            movie.GenreId = dto.GenreId;

            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (id == 0 || movie is null)
                return NotFound($"There's no movie matches this Id {id} !");

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return Ok(movie);
        }
    }
}
