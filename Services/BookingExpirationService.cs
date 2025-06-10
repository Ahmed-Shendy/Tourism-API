using Microsoft.EntityFrameworkCore;

namespace Tourism_Api.Services
{
    public class BookingExpirationService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<BookingExpirationService> _logger;

        public BookingExpirationService(IServiceProvider services, ILogger<BookingExpirationService> logger)
        {
            _services = services;
            _logger = logger;
        }

        public async Task CheckAndExpireBookings()
        {
            try
            {
                using (var scope = _services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<TourismContext>();
                    var currentTime = DateTime.Now;
                    _logger.LogInformation($"Checking for expired bookings at {currentTime}");

                    // التحقق من الحجوزات التي مرت دقيقة واحدة على تاريخ الحجز
                    var expiredBookings = await dbContext.Users
                        .Where(b => b.TourguidId != null && b.Bocked_Date != null
                         && b.Bocked_Date.Value.AddHours(24) < DateTime.Now)
                        .ToListAsync();

                    _logger.LogInformation($"Found {expiredBookings.Count} expired bookings");

                    foreach (var booking in expiredBookings)
                    {
                        // إذا كان تاريخ الحجز قد مر عليه دقيقة واحدة، قم بتحديثه
                        //if (DateTime.Now > booking.Bocked_Date!.Value)
                        //{
                        //    _logger.LogInformation($"Booking {booking.Name} has expired. Updating...");
                        //}
                        //else
                        //{
                        //    _logger.LogInformation($"Booking {booking.Name} is still valid. Skipping update.");
                        //    continue;
                        //}
                        _logger.LogInformation($"Processing booking {booking.Name} with booking date {booking.Bocked_Date}");
                        booking.Tourguid = null;
                        booking.TourguidId = null;
                        booking.Bocked_Date = null;
                        dbContext.Users.Update(booking);
                    }

                    if (expiredBookings.Any())
                    {
                        var result = await dbContext.SaveChangesAsync();
                        _logger.LogInformation($"Successfully updated {result} bookings");
                    }
                    else
                    {
                        _logger.LogInformation("No bookings were updated.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking expired bookings");
                throw;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAndExpireBookings();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}