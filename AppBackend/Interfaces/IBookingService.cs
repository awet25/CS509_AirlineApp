using AppBackend.Models;

namespace AppBackend.Interfaces
{
 public interface IBookingService
 {
    Task SendConfirmationEmailAsync(TicketBooking booking);
 }   
}