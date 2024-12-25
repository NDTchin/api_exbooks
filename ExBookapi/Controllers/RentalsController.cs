using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExBookapi.Data;
using ExBookapi.Models;
using ExBookapi.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace ExBookapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly ComicSystemContext _context;

        public RentalsController(ComicSystemContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<RentalDTO>> RentComicBooks(Rental rental)
        {
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            if (rental.RentalDetails == null)
            {
                rental.RentalDetails = new List<RentalDetail>();
            }
            foreach (var rentalDetail in rental.RentalDetails)
            {
                rentalDetail.RentalID = rental.RentalID; 
                _context.RentalDetails.Add(rentalDetail);  
            }
            await _context.SaveChangesAsync(); 

            var rentalDTO = new RentalDTO
            {
                RentalID = rental.RentalID,
                CustomerID = rental.CustomerID,
                RentalDate = rental.RentalDate,
                ReturnDate = rental.ReturnDate ?? DateTime.Now, 
                Status = rental.Status,
                RentalDetails = rental.RentalDetails.Select(rd => new RentalDetailDTO
                {
                    RentalDetailID = rd.RentalDetailID,
                    ComicBookID = rd.ComicBookID,
                    Quantity = rd.Quantity,
                    PricePerDay = rd.PricePerDay,
                    Title = rd.ComicBook.Title,
                    Author = rd.ComicBook.Author
                }).ToList()
            };

            return CreatedAtAction(nameof(GetRental), new { id = rental.RentalID }, rentalDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalDTO>> GetRental(int id)
        {
            var rental = await _context.Rentals
                .Include(r => r.Customer) 
                .Include(r => r.RentalDetails) 
                    .ThenInclude(rd => rd.ComicBook) 
                .FirstOrDefaultAsync(r => r.RentalID == id);

            if (rental == null)
            {
                return NotFound();
            }

            var rentalDTO = new RentalDTO
            {
                RentalID = rental.RentalID,
                CustomerID = rental.CustomerID,
                RentalDate = rental.RentalDate,
                ReturnDate = rental.ReturnDate ?? DateTime.Now, 
                Status = rental.Status,
                RentalDetails = rental.RentalDetails.Select(rd => new RentalDetailDTO
                {
                    RentalDetailID = rd.RentalDetailID,
                    ComicBookID = rd.ComicBookID,
                    Quantity = rd.Quantity,
                    PricePerDay = rd.PricePerDay,
                    Title = rd.ComicBook.Title,
                    Author = rd.ComicBook.Author
                }).ToList()
            };

            return Ok(rentalDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRental(int id, Rental rental)
        {
            if (id != rental.RentalID)
            {
                return BadRequest();
            }

            _context.Entry(rental).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalExists(id))
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
        public async Task<IActionResult> DeleteRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(r => r.RentalID == id);
        }
    }
}
