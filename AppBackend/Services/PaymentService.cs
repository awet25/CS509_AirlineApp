using System.ComponentModel.Design;
using AppBackend.Interfaces;
using Stripe.BillingPortal;
using Stripe.Checkout;
using Checkout = Stripe.Checkout;
namespace AppBackend.Services
{

    public class PaymentService : IPaymentService
    {
       private readonly ITicketBookingRepository _bookingRepository;
       public PaymentService(ITicketBookingRepository ticketBookingRepository){
         _bookingRepository=ticketBookingRepository;
       }
     public async Task<string> CreateCheckoutSessionAsync(double amount, string sessionId, string successUrl, string cancelUrl,string BookingReference)
    {   
        
        var sessionGuid=Guid.Parse(sessionId);
        Console.WriteLine(sessionGuid.ToString(),BookingReference);
        var booking= await _bookingRepository.GetBookingBySessionAndReferenceAsync(sessionGuid,BookingReference);
        if (booking==null) throw new Exception("Booking not Found");
        var options = new  Checkout.SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<Checkout.SessionLineItemOptions>
            {
                new Checkout.SessionLineItemOptions
                {
                    PriceData = new Checkout.SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(amount * 100), // Stripe uses cents
                        ProductData = new Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Flight Booking"
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = successUrl + "?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = cancelUrl,
            Metadata = new Dictionary<string, string>
            {
                { "sessionId", sessionId.ToString() },
                {"bookingReference",booking.BookingReference?? throw new InvalidOperationException("booking refrence is misssing")}
            }
        };

        var service = new Checkout.SessionService();
        var session = await service.CreateAsync(options);
        return session.Url;
    }

    }
    }

