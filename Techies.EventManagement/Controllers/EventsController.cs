using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace Techies.EventManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly DaprClient _client;

        public EventsController(DaprClient client)
        {
            _client = client;
        }

        // GET: api/events/{id}
        [HttpGet]
        public async Task<Event> Get(Guid id)
        {
            var eventRegistration = await _client.GetStateAsync<Event>("events", id.ToString());
            return eventRegistration;
        }

        // POST api/events
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Event newEvent)
        {
            if (newEvent == null || newEvent.Id == Guid.Empty || newEvent.NumberOfSeats < 1)
                return BadRequest();

            await _client.SaveStateAsync("events-store", newEvent.Id.ToString(), newEvent);
            await _client.PublishEventAsync<Event>("events-pubsub", "new-event-created", newEvent);

            return CreatedAtAction("Get", newEvent.Id);
        }
    }
}
