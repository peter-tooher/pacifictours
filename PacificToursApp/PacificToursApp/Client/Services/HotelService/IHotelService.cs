﻿namespace PacificToursApp.Client.Services.HotelService
{
    public interface IHotelService
    {
        List<Hotel> Hotels { get; set; }
        Task GetHotels();
    }
}