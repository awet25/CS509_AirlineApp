
using AppBackend.Interfaces;

namespace AppBackend.Services
{
    public class ReservationCleanUp : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger <ReservationCleanUp > _logger ;

        public ReservationCleanUp (IServiceProvider serviceProvider,ILogger <ReservationCleanUp > logger){
            _serviceProvider = serviceProvider;
            _logger = logger;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {   
            while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var seatRepo = scope.ServiceProvider.GetRequiredService<ISeatRepository>();
            var ticketRepo=scope.ServiceProvider.GetRequiredService<ITicketBookingRepository>();

            try
            {
                await seatRepo.ExprireStaleSeatHoldsAsync();
                _logger.LogInformation("[ReservationCleanUp] Expired stale seat holds at {time}", DateTime.UtcNow);
                await ticketRepo.ExpireUnpaidBookingsAsync();
                _logger.LogInformation("[ReservationCleanUp] Cleaned up unpaid bookings at {time}", DateTime.UtcNow);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReservationCleanUp] Error while expiring stale seat holds");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // run every 5 minutes
        }

        }

     
    }

}