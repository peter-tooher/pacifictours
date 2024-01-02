namespace PacificToursApp.Server.Services.TourService
{
    public interface ITourService
    {
        Task<ServiceResponse<List<Tour>>> GetToursAsync();
    }
}
