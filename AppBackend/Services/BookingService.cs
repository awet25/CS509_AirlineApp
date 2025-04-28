using AppBackend.Interfaces;
using AppBackend.Models;

namespace AppBackend.Services
{
    public class BookingService : IBookingService
    {  

        private readonly IEMailService _emailService;
        public BookingService(IEMailService eMailService){
             _emailService=eMailService;
        }
        public async Task SendConfirmationEmailAsync(TicketBooking booking)
        {   
             var seatInfo= string.Join(",", booking.BookedSeats?
             .Where(bs=>bs.IsConfirmed)
             .Select(bs=>bs.SeatNumber)??Enumerable.Empty<string>());
            var subject="Your Flight Booking Confirmation";
            var body=$@"
            <p>Dear {booking.FirstName} {booking.LastName}, </p>
            <p> Your flight has been successfully booked and paid an amount of {booking.Price}.</p>
            <p><strong>Confirmation Code: </strong>{booking.ConfirmationCode}</p>
            <p>Thank you for choosing Flying with us.</p>";
            await _emailService.SendEmail(booking.Email,subject,body);
        }
    }
}