namespace PacificToursApp.Server.Services.HotelService
{
    // The IHotelService interface defines the contract for the hotel service.
    public interface IHotelService
    {
        // The GetHotelsAsync method signature. This method should return a ServiceResponse containing a list of all hotels.
        Task<ServiceResponse<List<Hotel>>> GetHotelsAsync();

        // The GetHotelByIdAsync method signature. This method should return a ServiceResponse containing a single hotel that matches the provided hotelId.
        Task<ServiceResponse<Hotel>> GetHotelByIdAsync(int hotelId);
    }
}