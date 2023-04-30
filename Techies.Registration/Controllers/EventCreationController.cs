using Dapr;
using Microsoft.AspNetCore.Mvc;
using Techies.Registration.Infrastructure;
using Techies.Registration.Models;

namespace Techies.Registration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventCreationController : ControllerBase
    {
        private readonly RegistrationDbContext _dbContext;

        public EventCreationController(RegistrationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("event-created")]
        [Topic("events", "new-event-created")]
        public async Task<IActionResult> EventCreated([FromBody] EventDTO newEvent)
        {
            var eventToRegister = new Event
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                EventDate = newEvent.EventDate,
                NumberOfSeats = newEvent.NumberOfSeats
            };
            var seats = Enumerable.Range(0, newEvent.NumberOfSeats).Select(i => new Seat
            {
                EventId = eventToRegister.Id
            }).ToList();
            eventToRegister.Seats = seats;

            _dbContext.Events.Add(eventToRegister);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}