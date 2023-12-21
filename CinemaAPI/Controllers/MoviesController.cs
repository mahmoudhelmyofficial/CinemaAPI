using CinemaAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesAsync()
        {
            var movies = await _context.Movies.ToListAsync();

            return Ok(movies);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (id == 0 || movie is null)
                return NotFound($"There's no movie matches this Id {id} !");

            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovieAsync(MovieDTO dto)
        {


            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovieAsync()
        {


            return Ok();
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
