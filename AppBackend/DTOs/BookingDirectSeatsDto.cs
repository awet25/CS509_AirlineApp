namespace AppBackend.DTOs
{
 public class BookingDirectSeatsDto
 {  
    public int FlightId { get; set; }
    public string FlightSource { get; set; }
    public string SeatNumber { get; set; }
    public Guid SessionId { get; set; }
    public string Direction { get; set; }
 }   
}