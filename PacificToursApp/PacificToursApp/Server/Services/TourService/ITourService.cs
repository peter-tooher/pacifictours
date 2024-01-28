namespace PacificToursApp.Server.Services.TourService
{
    // The ITourService interface defines the contract for the tour service.
    public interface ITourService
    {
        // The GetToursAsync method signature. This method should return a ServiceResponse containing a list of all tours.
        Task<ServiceResponse<List<Tour>>> GetToursAsync();

        // The GetTourByIdAsync method signature. This method should return a ServiceResponse containing a single tour that matches the provided tourId.
        Task<ServiceResponse<Tour>> GetTourByIdAsync(int tourId);
    }
}
