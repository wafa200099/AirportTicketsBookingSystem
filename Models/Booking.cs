namespace AirportTicketsBookingSystem.Models
{
    public class Booking
    {
        public string BookingId { get; set; }
        public string PassengerId { get; set; }
        public string FlightId { get; set; } 
        public string ClassType { get; set; } 
        public decimal Price { get; set; }
        public DateTime BookingDate { get; set; }


        public Booking(string bookingId, string passengerId, string flightId, string classType, decimal price, DateTime bookingDate)
        {
            BookingId = bookingId;
            PassengerId = passengerId; 
            FlightId = flightId; 
            ClassType = classType;
            Price = price;
            BookingDate = bookingDate;
        }

    }
}