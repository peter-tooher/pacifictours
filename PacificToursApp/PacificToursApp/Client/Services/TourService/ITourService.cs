using PacificToursApp.Shared;

namespace PacificToursApp.Client.Services.TourService
{
    public interface ITourService
    {
        List<Tour> Tours { get; set; }
        Task GetTours();
    }
}