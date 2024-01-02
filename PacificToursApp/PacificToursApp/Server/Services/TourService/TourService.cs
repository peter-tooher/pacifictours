namespace PacificToursApp.Server.Services.TourService
{
    public class TourService : ITourService
    {
        private readonly DataContext _context;

        public TourService(DataContext context)
        {
            _context = context;
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
