namespace AppBackend.DTOs
{
  public class CreateStripeSessionRequestDto
  {
    public string SessionId { get; set; }
        public string BookingReference { get; set; }
  }   
}