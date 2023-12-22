using CinemaAPI.DTOs;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetGenresAsync()
        {
            var genres= await _context.Genres.OrderBy(n=>n.Name).ToListAsync();

            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
                return NotFound();

            return Ok(genre);
        }
                
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, GenreDTO dto)
        {
            if (id > 0)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

                if (genre is not null)
                {
                    genre.Name = dto.Name;

                    await _context.SaveChangesAsync();

                    return Ok(genre);
                }
                return NotFound($"No Genre matches this Id {id} ");
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddGenreAsync(GenreDTO dto)
        {
            if (dto.Name is not null)
            {
                Genre genre = new() { Name = dto.Name };

                await _context.Genres.AddAsync(genre);

                _context.SaveChanges();

                return Ok(genre);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenreAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre is null)
                return NotFound();

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return Ok(genre);
        }
    }
}
