using System.Web.Http;

namespace PacificToursApp.Server.Services.HotelService
{
    // The HotelService class provides methods for managing hotels. It implements the IHotelService interface.
    public class HotelService : IHotelService
    {
        // The _context field is a DataContext instance used to interact with the database.
        private readonly DataContext _context;

        // The constructor takes a DataContext instance and assigns it to the _context field.
        public HotelService(DataContext context)
        {
            _context = context;
        }

        // The GetHotelByIdAsync method retrieves a hotel by its ID.
        public async Task<ServiceResponse<Hotel>> GetHotelByIdAsync(int hotelId)
        {
            // A new ServiceResponse<Hotel> instance is created.
            var response = new ServiceResponse<Hotel>();
            // The hotel with the specified ID is retrieved from the database.
            var hotel = await _context.Hotels.FindAsync(hotelId);
            // If the hotel is not found, the Success property is set to false and a message is set.
            if (hotel == null)
            {
                response.Success = false;
                response.Message = "Hotel not found.";
            }
            // If the hotel is found, it is assigned to the Data property.
            else
            {
                response.Data = hotel;
            }
            // The ServiceResponse<Hotel> instance is returned.
            return response;
        }

        // The GetHotelsAsync method retrieves all hotels.
        public async Task<ServiceResponse<List<Hotel>>> GetHotelsAsync()
        {
            // A new ServiceResponse<List<Hotel>> instance is created and the Data property is assigned all hotels from the database.
            var response = new ServiceResponse<List<Hotel>>
            {
                Data = await _context.Hotels.ToListAsync()
            };

            // The ServiceResponse<List<Hotel>> instance is returned.
            return response;
        }
    }
}