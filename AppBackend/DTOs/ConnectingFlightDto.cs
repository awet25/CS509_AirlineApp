namespace AppBackend.DTOs
{
    public class ConnectingFlightDto
    {
    public int Flight1_Id { get; set; }
    public int Flight2_Id { get; set; }
    public string Flight1_DepartAirport { get; set; }
    public string ConnectingAirport { get; set; }
    public string Flight2_ArriveAirport { get; set; }
    public DateTime Flight1_DepartureDateTime { get; set; }
    public TimeSpan Flight1Duration { get; set; }
    public double Price { get; set; }
    public  TimeSpan Flight2Duration { get; set; }
    public DateTime Flight1_ArrivalDateTime { get; set; }
    public DateTime Flight2_DepartureDateTime { get; set; }
    public DateTime Flight2_ArrivalDateTime { get; set; }
    public TimeSpan ALLFlightDuration { get; set; }
    public TimeSpan TransitTime { get; set; }
    public string IndirectFlightNumber { get; set; }
    public string Flight1_Source { get; set; }
    public string Flight2_Source { get; set; }
    public string Currency {get; set;} ="USD";
    }
}