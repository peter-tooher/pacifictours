namespace PacificToursApp.Server.Services.TourService
{
    public class TourService : ITourService
    {
        private readonly DataContext _context;

        public TourService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Tour>> GetTourByIdAsync(int tourId)
        {
            var response = new ServiceResponse<Tour>();
            var product = await _context.Tours.FindAsync(tourId);
            if (product == null)
            {
                response.Success = false;
                response.Message = "Tour not found.";
            }
            else
            {
                response.Data = product;
            }
            return response;
        }

        public async Task<ServiceResponse<List<Tour>>> GetToursAsync()
        {
            var response = new ServiceResponse<List<Tour>>
            {
                Data = await _context.Tours.ToListAsync()
            };

            return response;
        }
    }
}
