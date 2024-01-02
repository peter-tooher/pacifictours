namespace PacificToursApp.Server.Services.HotelService
{
    public interface IHotelService
    {
        Task<ServiceResponse<List<Hotel>>> GetHotelsAsync();
    }
}