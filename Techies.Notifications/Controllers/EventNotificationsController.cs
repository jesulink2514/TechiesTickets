using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace Techies.Notifications.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventNotificationsController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<EventNotificationsController> _logger;

        public EventNotificationsController(DaprClient daprClient,
            ILogger<EventNotificationsController> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        [HttpPost("event-created")]
        [Topic("events", "new-event-created")]
        public async Task<IActionResult> EventCreated([FromBody] Event newEvent)
        {
            _logger.LogInformation($"New event created - {newEvent.Id}");

            var mailMetadata = new Dictionary<string, string>{
                {"emailTo","jesus@somostechies.com"},
                {"subject","Event Creation Notification"}
            };

            await _daprClient.InvokeBindingAsync("notifications-smtp", "create", $"Event created - {newEvent.Name}", mailMetadata);
            return Ok();
        }
    }
}