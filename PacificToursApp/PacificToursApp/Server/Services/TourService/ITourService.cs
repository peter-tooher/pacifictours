namespace PacificToursApp.Server.Services.TourService
{
    public interface ITourService
    {
        Task<ServiceResponse<List<Tour>>> GetToursAsync();
        Task<ServiceResponse<Tour>> GetTourByIdAsync(int tourId);
    }
}
