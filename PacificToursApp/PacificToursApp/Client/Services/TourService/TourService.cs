namespace PacificToursApp.Client.Services.TourService
{
    public class TourService : ITourService
    {
        private readonly HttpClient _http;
        public TourService(HttpClient http)
        {
            _http = http;
        }

        public List<Tour> Tours { get; set; } = new List<Tour>();

        public async Task<ServiceResponse<Tour>> GetTourById(int tourId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Tour>>($"api/tour/{tourId}");
            return result;
        }

        public async Task GetTours()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Tour>>>("api/tour");
            if (result != null && result.Data != null) 
                Tours = result.Data;
        }
    }
}