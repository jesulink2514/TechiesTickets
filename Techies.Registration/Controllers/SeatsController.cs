using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Techies.Registration.Infrastructure;
using Techies.Registration.Models;

namespace Techies.Registration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly RegistrationDbContext _dbContext;

        public SeatsController(RegistrationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{eventId}/user/{userId}")]
        public async Task<IActionResult> Get(Guid eventId, Guid userId)
        {
            var seat = await _dbContext.Seats.Where(s => s.EventId == eventId && s.UserId == userId).ToListAsync();
            if (!seat.Any()) return NotFound();

            return Ok(seat);
        }

        [HttpPost]
        public async Task<IActionResult> Post(SeatBookingIntentionDTO seat)
        {
            var eventToRegister = await _dbContext.Events.Include(e => e.Seats).FirstOrDefaultAsync(e => e.Id == seat.EventId);

            if (eventToRegister == null) return NotFound("Event not found");

            var result = ReserveSeat(eventToRegister, seat.UserId);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await _dbContext.Entry(eventToRegister).ReloadAsync();
                var source = _dbContext.Entry(eventToRegister).Collection(e => e.Seats);
                if (source.CurrentValue != null)
                {
                    foreach (var item in source.CurrentValue)
                        source.EntityEntry.Context.Entry(item).State = EntityState.Detached;
                    source.CurrentValue = null;
                }
                source.IsLoaded = false;
                source.Load();

                result = ReserveSeat(eventToRegister, seat.UserId);

                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest("The seat you are trying to reserve was taken.");
                }
            }

            return result;
        }

        private IActionResult ReserveSeat(Event eventToRegister, Guid userId)
        {
            if (!eventToRegister.HasAvailableSeat) return BadRequest("Event doesn't have an available seat");

            var seatToBook = eventToRegister.GetNextAvailableSeat();

            if (seatToBook == null) return BadRequest("Event doesn't have an available seat");

            seatToBook.UserId = userId;

            return CreatedAtAction("Get", new { eventId = eventToRegister.Id, userId = userId }, seatToBook);
        }
    }
}