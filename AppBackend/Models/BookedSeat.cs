using AppBackend.DTOs;
using System.ComponentModel.DataAnnotations.Schema;
namespace AppBackend.Models
{
    public class BookedSeat
    {
        public int Id { get; set; }
        public FlightTypes FlightType { get; set; }
        public int FlightId { get; set; }
        public string FlightSource { get; set; }
        public int? Leg { get; set; }
        public string SeatNumber    { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime HoldExpiresAt { get; set;}
        public string Direction { get; set; } // "outbound" or "return"
        public Guid SessionId { get; set; }
        
        public int? TicketBookingId{get;set;}
        
        [ForeignKey(nameof(TicketBookingId))]
        public TicketBooking TicketBooking {get;set;}

        
    }
}