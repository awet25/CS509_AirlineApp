using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Controllers{


[ApiController]
[Route("api/v1/[controller]")]
public class BookingController: ControllerBase
{
    private readonly ISeatRepository _seatRepository;
    private readonly ITicketBookingRepository _TicketRepository;
    private readonly ITicketBookingFlightRepository _ticketBookingFlightRepository;
    private readonly   AppDbContext _context;
    public BookingController(ITicketBookingFlightRepository ticketBookingFlightRepository,ISeatRepository seatRepo, ITicketBookingRepository ticketRepo,AppDbContext context){
        _seatRepository = seatRepo;
        _TicketRepository= ticketRepo;
        _context=context;
        _ticketBookingFlightRepository=ticketBookingFlightRepository;

    }

    [HttpGet("availableseat/direct/{flightId}")]
    public async Task<ActionResult<List<AvailableSeatDto>>> GetDirectSeats(  int flightId, [FromQuery] string source,[FromQuery] string direction)
    {   
        var seat= await _seatRepository.GetAvailableSeatsForDirectFlightAsync(flightId, source,direction);
        var seatDto=seat.Select(s => new AvailableSeatDto {SeatNumber=s} ).ToList();
        return Ok(seatDto);
    }

    [HttpGet("availableseat/connecting")]
    public async Task <ActionResult<List<AvailableSeatDto>>> GetConnectingSeats([FromQuery]int flight1Id, [FromQuery] int flight2Id,[FromQuery] string source1, [FromQuery] string source2,[FromQuery] string direction)
    {
        var seats=await _seatRepository.GetAvailableSeatsForConnectedFlightAsync(flight1Id, flight2Id,source1,source2,direction);
        return Ok(seats);
    }

    [HttpPost("selectseat/direct")]
    public async Task<IActionResult> BookDirectSeat([FromBody] BookingDirectSeatsDto dto)
    {
        var isHeld = await _seatRepository.HoldSeatForDirectFlightAsync(dto);
        if (!isHeld)
            return BadRequest("Seat already booked or invalid request.");

        return Ok("Seat booked successfully.");
    }
       [HttpPost("selectseat/connecting")]
    public async Task<IActionResult> BookConnectingSeat([FromBody] BookingConnectingSeatsDto dto)
    {
        var isHeld = await _seatRepository.HoldSeatForConnectingFlightAsync(dto);
        if (!isHeld)
            return BadRequest("Seat already booked or invalid request.");

        return Ok("Seat booked successfully.");
    }


    [HttpPost("info")]
    public async Task<IActionResult> SaveBookingInfo([FromBody] BookingInfoDto dto)
    {
        var ticketInfo=await _TicketRepository.AddBookingInfoAsync(dto);
        var ticketbookingflight=await _ticketBookingFlightRepository.AddTicketBookingFlightAsync(dto,ticketInfo.Id);
        return Ok( new {message ="booking info saved. Process to payment",BookingReference=ticketInfo.BookingReference});


    }

     
    
  
}


}