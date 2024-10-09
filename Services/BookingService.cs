using AirportTicketsBookingSystem.Data;
using AirportTicketsBookingSystem.Models;
using System.Linq;

namespace AirportTicketsBookingSystem.Services
{
    public class BookingService
    {
        private List<Booking> bookings;
        private List<Flight> flights; 

        public BookingService()
        {
            bookings = DataStorage.LoadBookings();
            flights = DataStorage.LoadFlights(); 
        }

      
        public List<Booking> ViewBookings(string passengerId)
        {
            return bookings.Where(b => b.PassengerId == passengerId).ToList();
        }

    
        public void CancelBooking(string bookingId)
        {
            var booking = bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking != null)
            {
                bookings.Remove(booking);
                DataStorage.SaveBookings(bookings);
            }
        }

   
        public void ModifyBooking(string bookingId, string newClassType)
        {
            var booking = bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking != null)
            {
                booking.ClassType = newClassType;

                var flight = flights.FirstOrDefault(f => f.FlightId == booking.FlightId);
                if (flight != null)
                {
                    booking.Price = newClassType switch
                    {
                        "Economy" => flight.EconomyPrice,
                        "Business" => flight.BusinessPrice,
                        "First Class" => flight.FirstClassPrice,
                        _ => throw new ArgumentException("Invalid class type")
                    };
                    DataStorage.SaveBookings(bookings);
                }
                else
                {
                    throw new InvalidOperationException("Flight not found for the booking.");
                }
            }
            else
            {
                throw new InvalidOperationException("Booking not found.");
            }
        }    
         
        
        public Booking ViewBookingById(string bookingId)
        {
            return bookings.FirstOrDefault(b => b.BookingId == bookingId);
        }

    }
}
