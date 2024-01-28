namespace PacificToursApp.Server.Services.TourService
{
    // The TourService class provides methods for managing tours. It implements the ITourService interface.
    public class TourService : ITourService
    {
        // The _context field is a DataContext instance used to interact with the database.
        private readonly DataContext _context;

        // The constructor takes a DataContext instance and assigns it to the _context field.
        public TourService(DataContext context)
        {
            _context = context;
        }

        // The GetTourByIdAsync method retrieves a tour by its ID.
        public async Task<ServiceResponse<Tour>> GetTourByIdAsync(int tourId)
        {
            // A new ServiceResponse<Tour> instance is created.
            var response = new ServiceResponse<Tour>();
            // The tour with the specified ID is retrieved from the database.
            var tour = await _context.Tours.FindAsync(tourId);
            // If the tour is not found, the Success property is set to false and a message is set.
            if (tour == null)
            {
                response.Success = false;
                response.Message = "Tour not found.";
            }
            // If the tour is found, it is assigned to the Data property.
            else
            {
                response.Data = tour;
            }
            // The ServiceResponse<Tour> instance is returned.
            return response;
        }

        // The GetToursAsync method retrieves all tours.
        public async Task<ServiceResponse<List<Tour>>> GetToursAsync()
        {
            // A new ServiceResponse<List<Tour>> instance is created and the Data property is assigned all tours from the database.
            var response = new ServiceResponse<List<Tour>>
            {
                Data = await _context.Tours.ToListAsync()
            };

            // The ServiceResponse<List<Tour>> instance is returned.
            return response;
        }
    }
}
