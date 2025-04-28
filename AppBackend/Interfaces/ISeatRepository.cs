using AppBackend.Data;
using AppBackend.DTOs;

namespace AppBackend.Interfaces
{
  public interface ISeatRepository
  {
    Task<List<string>> GetAvailableSeatsForDirectFlightAsync(int flightId, string source,string direction);
    Task <List<FlightSeatMapDto>> GetAvailableSeatsForConnectedFlightAsync(int flight1Id,int flight2Id,string source1,string source2,string direction);
  
    Task <bool> HoldSeatForDirectFlightAsync(BookingDirectSeatsDto dto);
    Task <bool> HoldSeatForConnectingFlightAsync(BookingConnectingSeatsDto dto);
    Task ExprireStaleSeatHoldsAsync();
     Task<bool>ConfirmseatsAsync(Guid sessionId,int bookingId);
  }   
}