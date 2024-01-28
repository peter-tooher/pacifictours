namespace PacificToursApp.Client.Services.TourService
{
    // The TourService class implements the ITourService interface
    public class TourService : ITourService
    {
        // A private readonly HttpClient field to make HTTP requests
        private readonly HttpClient _http;

        // The constructor takes a HttpClient and assigns it to the _http field
        public TourService(HttpClient http)
        {
            _http = http;
        }

        // A list of Tour objects that can be accessed and modified publicly
        public List<Tour> Tours { get; set; } = new List<Tour>();

        // An asynchronous method that takes a tourId and returns a ServiceResponse of type Tour
        // It makes a GET request to the "api/tour/{tourId}" endpoint and deserializes the response to a ServiceResponse of type Tour
        public async Task<ServiceResponse<Tour>> GetTourById(int tourId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Tour>>($"api/tour/{tourId}");
            return result;
        }

        // An asynchronous method that makes a GET request to the "api/tour" endpoint
        // It deserializes the response to a ServiceResponse of type List<Tour> and assigns the Data property of the response to the Tours property
        public async Task GetTours()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Tour>>>("api/tour");
            if (result != null && result.Data != null)
                Tours = result.Data;
        }
    }
}