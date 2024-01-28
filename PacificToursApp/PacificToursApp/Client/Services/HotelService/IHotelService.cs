namespace PacificToursApp.Client.Services.HotelService
{
    // The IHotelService interface defines the contract for the HotelService
    public interface IHotelService
    {
        // A list of Hotel objects that can be accessed and modified
        List<Hotel> Hotels { get; set; }

        // An asynchronous method that retrieves all hotels
        Task GetHotels();

        // An asynchronous method that takes a hotelId and returns a ServiceResponse of type Hotel
        Task<ServiceResponse<Hotel>> GetHotelById(int hotelId);
    }
}
