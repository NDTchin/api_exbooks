using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExBookapi.Data;
using ExBookapi.Models;
using ExBookapi.DTOs;

namespace ExBookapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComicBooksController : ControllerBase
    {
        private readonly ComicSystemContext _context;

        public ComicBooksController(ComicSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComicBookDTO>>> GetComicBooks()
        {
            var comicBooks = await _context.ComicBooks.ToListAsync();

            var comicBookDTOs = comicBooks.Select(cb => new ComicBookDTO
            {
                ComicBookID = cb.ComicBookID,
                Title = cb.Title,
                Author = cb.Author,
                PricePerDay = cb.PricePerDay
            }).ToList();

            return Ok(comicBookDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComicBookDTO>> GetComicBook(int id)
        {
            var comicBook = await _context.ComicBooks.FirstOrDefaultAsync(cb => cb.ComicBookID == id);

            if (comicBook == null)
            {
                return NotFound();
            }

            var comicBookDTO = new ComicBookDTO
            {
                ComicBookID = comicBook.ComicBookID,
                Title = comicBook.Title,
                Author = comicBook.Author,
                PricePerDay = comicBook.PricePerDay
            };

            return Ok(comicBookDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ComicBookDTO>> CreateComicBook(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
            await _context.SaveChangesAsync();

            var comicBookDTO = new ComicBookDTO
            {
                ComicBookID = comicBook.ComicBookID,
                Title = comicBook.Title,
                Author = comicBook.Author,
                PricePerDay = comicBook.PricePerDay
            };

            return CreatedAtAction(nameof(GetComicBook), new { id = comicBook.ComicBookID }, comicBookDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComicBook(int id, ComicBook comicBook)
        {
            if (id != comicBook.ComicBookID)
            {
                return BadRequest();
            }

            _context.Entry(comicBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComicBookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComicBook(int id)
        {
            var comicBook = await _context.ComicBooks.FindAsync(id);

            if (comicBook == null)
            {
                return NotFound();
            }

            _context.ComicBooks.Remove(comicBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComicBookExists(int id)
        {
            return _context.ComicBooks.Any(cb => cb.ComicBookID == id);
        }
    }
}
