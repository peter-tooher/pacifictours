using PacificToursApp.Client.Pages;
using PacificToursApp.Shared;

namespace PacificToursApp.Client.Services.HotelService
{
    // The HotelService class implements the IHotelService interface
    public class HotelService : IHotelService
    {
        // A private readonly HttpClient field to make HTTP requests
        private readonly HttpClient _http;

        // The constructor takes a HttpClient and assigns it to the _http field
        public HotelService(HttpClient http)
        {
            _http = http;
        }

        // A list of Hotel objects that can be accessed and modified publicly
        public List<Hotel> Hotels { get; set; } = new List<Hotel>();

        // An asynchronous method that takes a hotelId and returns a ServiceResponse of type Hotel
        // It makes a GET request to the "api/hotel/{hotelId}" endpoint and deserializes the response to a ServiceResponse of type Hotel
        public async Task<ServiceResponse<Hotel>> GetHotelById(int hotelId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Hotel>>($"api/hotel/{hotelId}");
            return result;
        }

        // An asynchronous method that makes a GET request to the "api/Hotel" endpoint
        // It deserializes the response to a ServiceResponse of type List<Hotel> and assigns the Data property of the response to the Hotels property
        public async Task GetHotels()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Hotel>>>("api/Hotel");
            if (result != null && result.Data != null)
                Hotels = result.Data;
        }
    }
}
