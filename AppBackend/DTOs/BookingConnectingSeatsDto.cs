namespace AppBackend.DTOs
{
   public class BookingConnectingSeatsDto
   {


    public int Flight1Id { get; set; }
    public int Flight2Id { get; set; }
    public string Flight1Sournce { get; set; }
    public string Flight2Source { get; set; }
    public string Seat1 { get; set; }
    public string Seat2 {get;set;}
    public Guid SessionId   {get; set; } 
    public string Direction { get; set; }

   }   
public class AvailableSeatDto
{
    public string SeatNumber { get; set; }
}
  
  public enum FlightTypes{
    Direct=1,
    Connecting=2

  }

  

}