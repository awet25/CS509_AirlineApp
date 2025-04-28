using AppBackend.DTOs;
using AppBackend.Models;

namespace AppBackend.Interfaces
{

 public interface ITicketBookingRepository{
Task <TicketBooking> AddBookingInfoAsync(BookingInfoDto bookingInfoDto);
Task <TicketBooking> GetBookingBySessionAndReferenceAsync(Guid SessionId,string Bookingreference);
Task<bool> MarkBookingAsPaidAsync(Guid sessionId, string bookingReference);
Task <TicketBooking?>GetBookingBySessionIdAsync(Guid sessionId);
Task<bool> IsSeatTakenAsync(int flightId, string seatNumber);
Task UpdateBookingAsync(TicketBooking booking);
Task ExpireUnpaidBookingsAsync();

 }

}