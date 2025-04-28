using AppBackend.DTOs;
using AppBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
 public class PaymentContoller:ControllerBase
 {
    private readonly IPaymentService _paymentService;
    private readonly ITicketBookingRepository _ticketBookingRepository;
    public PaymentContoller(IPaymentService paymentService, ITicketBookingRepository ticketBookingRepository)
    {
     _paymentService=paymentService;
     _ticketBookingRepository=ticketBookingRepository;   
    }

    [HttpPost("create-session")]
    public async Task<IActionResult> CreateSession([FromBody] CreateStripeSessionRequestDto request)
    {
        if (!Guid.TryParse(request.SessionId, out var sessionGuid))
        return BadRequest("Invalid session ID.");

    var booking = await _ticketBookingRepository.GetBookingBySessionAndReferenceAsync(sessionGuid, request.BookingReference);
    Console.WriteLine(booking);
    if (booking == null)
        return NotFound("Booking not found.");

    var url = await _paymentService.CreateCheckoutSessionAsync(
        booking.Price,
        booking.SessionId.ToString(), 
        "http://localhost:5173/success?session_id={CHECKOUT_SESSION_ID}"
,
        "http://localhost:5173/cancel",
         booking.BookingReference!
    );
            return Ok(new { url });
        }
    }

 }   
