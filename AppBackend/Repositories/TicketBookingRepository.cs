using System.Data;
using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using AppBackend.util;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace AppBackend.Repositories
{
    public class TicketBookingRepository : ITicketBookingRepository
    {  
        private readonly AppDbContext _context;
        private readonly ISeatRepository _seatRepository;
        private readonly IBookingService _bookingService;
        private readonly ILogger<TicketBookingRepository> _logger;
        public TicketBookingRepository(AppDbContext context, ILogger<TicketBookingRepository> logger,
        
        ISeatRepository seatRepository, IBookingService bookingService
        )
        {
            _context = context;
            _logger = logger;
            _bookingService=bookingService;
            _bookingService=bookingService;
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
                BookingReference=$"BR-{Guid.NewGuid().ToString().Substring(0,8).ToUpper()}"
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

        public async Task<TicketBooking> GetBookingBySessionAndReferenceAsync(Guid SessionId, string Bookingreference)
        {
            return await _context.TicketBookings
            .FirstOrDefaultAsync(T=>T.SessionId==SessionId  && T.BookingReference==Bookingreference);
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

        public async Task<bool> MarkBookingAsPaidAsync(Guid sessionId, string bookingReference)
        {
             var booking = await GetBookingBySessionAndReferenceAsync(sessionId, bookingReference);
            if (booking == null) return false;

           booking.IsPaid = true;
           booking.ConfirmationCode = ConfirmationNumberGenerator.Generate();
           await _context.SaveChangesAsync();
           return true;
        }

        public async Task UpdateBookingAsync(TicketBooking booking)
        {   _logger.LogWarning("Saving booking update to database...");
           _context.TicketBookings.Update(booking);
           await _context.SaveChangesAsync();
           _logger.LogWarning("Saved booking update to database.");
        }
    }
}