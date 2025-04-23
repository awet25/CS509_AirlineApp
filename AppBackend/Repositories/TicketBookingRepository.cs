using System.Data;
using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Repositories
{
    public class TicketBookingRepository : ITicketBookingRepository
    {  
        private readonly AppDbContext _context;
        private readonly ILogger<TicketBookingRepository> _logger;
        public TicketBookingRepository(AppDbContext context, ILogger<TicketBookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
       
        public async Task<TicketBooking> AddBookingInfoAsync(BookingInfoDto bookingInfoDto)
        {
            var booking= new TicketBooking{
                SessionId=bookingInfoDto.SessionId,
                FirstName=bookingInfoDto.FirstName,
                LastName=bookingInfoDto.LastName,
                Email=bookingInfoDto.Email,
                PhoneNumber=bookingInfoDto.PhoneNumber,
                BookingTime=DateTime.UtcNow,
                IsPaid=false,
                Price=bookingInfoDto.Price,
                Gender=bookingInfoDto.Gender,
                DateOfBirth=bookingInfoDto.DateOfBirth,
            };
         var bookingInfo = _context.TicketBookings.Add(booking);
         await _context.SaveChangesAsync();
         return bookingInfo.Entity;

        }

        public  async Task ExpireUnpaidBookingsAsync()
        {
             var cutoff=DateTime.UtcNow.AddMinutes(-15);
         var expired=await _context.TicketBookings.
         Include(b=>b.Flights).Where(b=>!b.IsPaid && b.BookingTime<cutoff).ToListAsync();
         if(expired.Any())
         {
            _context.TicketBookingFlights.RemoveRange(expired.SelectMany(b=>b.Flights));
            _context.TicketBookings.RemoveRange(expired);
            await _context.SaveChangesAsync(); 
        }
        }

        public async Task<TicketBooking?> GetBookingBySessionIdAsync(Guid sessionId)
        {
            return  await _context.TicketBookings.Include(b=>b.Flights)
            .FirstOrDefaultAsync(b=>b.SessionId==sessionId);
        }

        public async Task<bool> IsSeatTakenAsync(int flightId, string seatNumber)
        {
           return await _context.BookedSeats.AnyAsync(bs =>
        bs.FlightId == flightId &&
        bs.SeatNumber == seatNumber &&
        (bs.IsConfirmed || bs.HoldExpiresAt > DateTime.UtcNow));
        }

        public async Task UpdateBookingAsync(TicketBooking booking)
        {   _logger.LogWarning("Saving booking update to database...");
           _context.TicketBookings.Update(booking);
           await _context.SaveChangesAsync();
           _logger.LogWarning("Saved booking update to database.");
        }
    }
}