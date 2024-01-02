﻿using System.Web.Http;

namespace PacificToursApp.Server.Services.HotelService
{
    public class HotelService : IHotelService
    {
        private readonly DataContext _context;

        public HotelService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<Hotel>>> GetHotelsAsync()
        {
            var response = new ServiceResponse<List<Hotel>>
            {
                Data = await _context.Hotels.Include(h => h.SingleSuite).Include(h => h.DoubleSuite).Include(h => h.FamilySuite).ToListAsync()
            };

            return response;
        }
    }
}