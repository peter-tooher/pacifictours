using PacificToursApp.Client.Pages;
using PacificToursApp.Shared;

namespace PacificToursApp.Client.Services.HotelService
{
    public class HotelService : IHotelService
    {
        private readonly HttpClient _http;
        public HotelService(HttpClient http)
        {
            _http = http;
        }

        public List<Hotel> Hotels { get; set; } = new List<Hotel>();
        public async Task<ServiceResponse<Hotel>> GetHotelById(int hotelId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Hotel>>($"api/hotel/{hotelId}");
            return result;
        }

        public async Task GetHotels()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Hotel>>>("api/Hotel");
            if (result != null && result.Data != null)
                Hotels = result.Data;
        }
    }
}
