namespace PacificToursApp.Client.Services
{ 
// The BookingService class is responsible for managing bookings
    public class BookingService
    {
        // The current booking being processed
        public PackageBookings CurrentBooking { get; private set; }

        // Method to create a new booking
        public void CreateNewBooking()
        {
            CurrentBooking = new PackageBookings();
        }

        // Method to clear the current booking
        public void ClearBooking()
        {
            CurrentBooking = null;
        }

        // The current discount applied to the booking
        public decimal CurrentDiscount { get; private set; }

        // Method to update the hotel options of the current booking
        public void UpdateHotelOptions(int userId, int hotelId, string hotelName, string suiteOption, DateTime checkIn, DateTime checkOut, decimal deposit, decimal price)
        {
            decimal discount = 0;

            // Determine the discount based on the suite option
            switch (suiteOption)
            {
                case "Family":
                    discount = 0.4m;
                    break;
                case "Double":
                    discount = 0.2m;
                    break;
                case "Single":
                    discount = 0.1m;
                    break;
            }

            CurrentDiscount = discount;

            // Calculate the total price based on the number of days and the price per day
            int numberOfDays = (checkOut - checkIn).Days;
            decimal totalPrice = price * numberOfDays;
            decimal discountedPrice = totalPrice * (1 - discount);
            decimal finalPrice = discountedPrice - deposit;

            // Update the current booking with the hotel options
            CurrentBooking.UserId = userId;
            CurrentBooking.HotelId = hotelId;
            CurrentBooking.HotelName = hotelName;
            CurrentBooking.SuiteOption = suiteOption;
            CurrentBooking.HotelCheckIn = checkIn;
            CurrentBooking.HotelCheckOut = checkOut;
            CurrentBooking.HotelDeposit = deposit;
            CurrentBooking.HotelPrice = finalPrice;
            CurrentBooking.HotelPaymentDue = checkIn.AddDays(-28);
            CurrentBooking.TotalPrice = finalPrice + (CurrentBooking.TourPrice);
        }

        // Method to update the tour options of the current booking
        public void UpdateTourOptions(int tourId, string tourName, DateTime checkIn, decimal deposit, decimal price)
        {
            decimal discountedTourPrice = price * (1 - CurrentDiscount);
            decimal finalPrice = discountedTourPrice - deposit;

            // Update the current booking with the tour options
            CurrentBooking.TourId = tourId;
            CurrentBooking.TourName = tourName;
            CurrentBooking.TourCheckIn = checkIn;
            CurrentBooking.TourDeposit = deposit;
            CurrentBooking.TourPrice = finalPrice;
            CurrentBooking.TourPaymentDue = checkIn.AddDays(-28);
            CurrentBooking.TotalPrice = finalPrice + (CurrentBooking.HotelPrice);
        }

        // Method to return the current booking
        public PackageBookings ReturnBooking()
        {
            return CurrentBooking;
        }
    }
}
