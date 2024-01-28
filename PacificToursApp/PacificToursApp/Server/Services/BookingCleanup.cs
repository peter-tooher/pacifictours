using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// The BookingCleanupService class is a hosted service that cleans up unpaid bookings.
public class BookingCleanupService : IHostedService, IDisposable
{
    // The _serviceProvider field is used to create a new scope and get a DataContext instance.
    private readonly IServiceProvider _serviceProvider;
    // The _timer field is used to schedule the cleanup task to run at regular intervals.
    private Timer _timer;

    // The constructor takes an IServiceProvider instance and assigns it to the _serviceProvider field.
    public BookingCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    // The StartAsync method is called when the application starts. It initializes the _timer field.
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // The cleanup task is scheduled to run immediately and then every 24 hours.
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(24));
        return Task.CompletedTask;
    }

    // The DoWork method is the cleanup task. It deletes unpaid bookings that are more than 28 days past their payment due date.
    private void DoWork(object state)
    {
        // A new scope is created to get a DataContext instance.
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            // Unpaid tour, hotel, and package bookings that are more than 28 days past their payment due date are selected.
            var tourBookingsToBeDeleted = context.TourBookings
                .Where(b => !b.Paid && b.PaymentDue.AddDays(28) <= DateTime.Now)
                .ToList();
            var hotelBookingsToBeDeleted = context.HotelBookings
                .Where(b => !b.Paid && b.PaymentDue.AddDays(28) <= DateTime.Now)
                .ToList();
            var packageBookingsToBeDeleted = context.PackageBookings
            .Where(b => !b.Paid && (b.TourPaymentDue.AddDays(28) <= DateTime.Now || b.HotelPaymentDue.AddDays(28) <= DateTime.Now))
            .ToList();

            // The selected bookings are deleted from the database.
            context.TourBookings.RemoveRange(tourBookingsToBeDeleted);
            context.HotelBookings.RemoveRange(hotelBookingsToBeDeleted);
            context.PackageBookings.RemoveRange(packageBookingsToBeDeleted);
            // The changes are saved to the database.
            context.SaveChanges();
        }
    }

    // The StopAsync method is called when the application is stopping. It stops the timer.
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    // The Dispose method is called when the application is stopping. It disposes of the timer.
    public void Dispose()
    {
        _timer?.Dispose();
    }
}
