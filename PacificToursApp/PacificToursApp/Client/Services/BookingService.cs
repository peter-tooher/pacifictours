namespace PacificToursApp.Client.Services
{ 
    public class BookingService
    {
        public PackageBookings CurrentBooking { get; private set; }

        public void CreateNewBooking()
        {
            CurrentBooking = new PackageBookings();
        }

        public void ClearBooking()
        {
            CurrentBooking = null;
        }


        public decimal CurrentDiscount { get; private set; }

        public void UpdateHotelOptions(int userId, int hotelId, string hotelName, string suiteOption, DateTime checkIn, DateTime checkOut, decimal deposit, decimal price)
        {
            decimal discount = 0;

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

            int numberOfDays = (checkOut - checkIn).Days;
            decimal totalPrice = price * numberOfDays;
            decimal discountedPrice = totalPrice * (1 - discount);
            decimal finalPrice = discountedPrice - deposit;

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

        public void UpdateTourOptions(int tourId, string tourName, DateTime checkIn, decimal deposit, decimal price)
        {
            decimal discountedTourPrice = price * (1 - CurrentDiscount);
            decimal finalPrice = discountedTourPrice - deposit;

            CurrentBooking.TourId = tourId;
            CurrentBooking.TourName = tourName;
            CurrentBooking.TourCheckIn = checkIn;
            CurrentBooking.TourDeposit = deposit;
            CurrentBooking.TourPrice = finalPrice;
            CurrentBooking.TourPaymentDue = checkIn.AddDays(-28);
            CurrentBooking.TotalPrice = finalPrice + (CurrentBooking.HotelPrice);
        }

        public PackageBookings ReturnBooking()
        {
            return CurrentBooking;
        }
    }
}
