using PacificToursApp.Shared;

namespace PacificToursApp.Client.Services.TourService
{
    // The ITourService interface defines the contract for the TourService
    public interface ITourService
    {
        // A list of Tour objects that can be accessed and modified
        List<Tour> Tours { get; set; }

        // An asynchronous method that retrieves all tours
        Task GetTours();

        // An asynchronous method that takes a tourId and returns a ServiceResponse of type Tour
        Task<ServiceResponse<Tour>> GetTourById(int tourId);
    }
}