namespace AppBackend.Data
{
    
public class FlightSeatMapDto
{
    public int FlightId { get; set; }
    public string FlightSource { get; set; }
    public string Direction { get; set; }
    public int Leg { get; set; }
    public List<string> AvailableSeats { get; set; }
}
}