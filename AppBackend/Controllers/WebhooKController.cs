using AppBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

namespace AppBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ITicketBookingRepository _ticketBookingRepository;
        private readonly ILogger<WebhookController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISeatRepository _seatRepository;

        public WebhookController(
            ITicketBookingRepository ticketBookingRepository,
            ILogger<WebhookController> logger,
            IConfiguration configuration,
            ISeatRepository seatRepository)
        {
            _ticketBookingRepository = ticketBookingRepository;
            _logger = logger;
            _configuration = configuration;
            _seatRepository = seatRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var secret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], secret);

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var stripeSession = stripeEvent.Data.Object as Session;

                    if (stripeSession == null)
                    {
                        _logger.LogError("Stripe session object is null in webhook payload.");
                        return BadRequest();
                    }

                    // Retrieve full session including metadata
                    var service = new SessionService();
                    var session = await service.GetAsync(stripeSession.Id);

                    if (session.Metadata != null && session.Metadata.TryGetValue("sessionId", out var appSessionId))
                    {
                        var sessionGuid = Guid.Parse(appSessionId);
                        var booking = await _ticketBookingRepository.GetBookingBySessionIdAsync(sessionGuid);

                        if (booking != null)
                        {
                            booking.IsPaid = true;
                            await _ticketBookingRepository.UpdateBookingAsync(booking);

                            var success = await _seatRepository.ConfirmseatsAsync(sessionGuid);
                            _logger.LogInformation("Booking {bookingId} marked as paid. Seats confirmed: {confirmed}", booking.Id, success);
                        }
                        else
                        {
                            _logger.LogWarning("No booking found for sessionId: {sessionId}", appSessionId);
                        }
                    }
                    else
                    {
                        _logger.LogError("Metadata is null or missing sessionId in Stripe session.");
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError($"Stripe webhook error: {e.Message}");
                return BadRequest();
            }
        }
    }
}
