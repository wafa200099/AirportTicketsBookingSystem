using AirportTicketsBookingSystem.Data;
using AirportTicketsBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportTicketsBookingSystem.Services
{
    public class PassengerService
    {
        private List<Flight> flights;
        private List<Booking> bookings;

        public PassengerService()
        {
            flights = DataStorage.LoadFlights();
            bookings = DataStorage.LoadBookings();
        }

        public List<Flight> SearchFlights(string departureCountry, string destinationCountry, DateTime? departureDate = null)
        {
            // Check if flights are loaded
            if (flights == null || flights.Count == 0)
            {
                Console.WriteLine("No flights available.");
                return new List<Flight>(); // Return an empty list if no flights are available
            }

            var foundFlights = flights.Where(f =>
                f != null && // Ensure that the flight object is not null
                f.DepartureCountry != null && f.DestinationCountry != null && // Check that properties are not null
                f.DepartureCountry.Trim().Equals(departureCountry.Trim(), StringComparison.OrdinalIgnoreCase) &&
                f.DestinationCountry.Trim().Equals(destinationCountry.Trim(), StringComparison.OrdinalIgnoreCase) &&
                (!departureDate.HasValue || f.DepartureDate.Date == departureDate.Value.Date)).ToList();

            // Provide feedback to the user
            if (foundFlights.Count > 0)
            {
                Console.WriteLine($"{foundFlights.Count} flight(s) found from {departureCountry} to {destinationCountry}" +
                                  $"{(departureDate.HasValue ? $" on {departureDate.Value.ToShortDateString()}" : "")}.");
            }
            else
            {
                Console.WriteLine($"No flights found from {departureCountry} to {destinationCountry}" +
                                  $"{(departureDate.HasValue ? $" on {departureDate.Value.ToShortDateString()}" : "")}.");
            }

            return foundFlights;
        }


  
        public void BookFlight(Passenger passenger, Flight flight, string classType)
        {
            if (passenger == null || string.IsNullOrEmpty(passenger.PassengerId)) 
                throw new ArgumentNullException(nameof(passenger), "Passenger or PassengerId cannot be null.");
            if (flight == null || string.IsNullOrEmpty(flight.FlightId)) 
                throw new ArgumentNullException(nameof(flight), "Flight or FlightId cannot be null.");
            if (string.IsNullOrEmpty(classType)) 
                throw new ArgumentException("Class type cannot be null or empty.", nameof(classType));

            decimal price = classType switch
            {
                "Economy" => flight.EconomyPrice,
                "Business" => flight.BusinessPrice,
                "First Class" => flight.FirstClassPrice,
                _ => throw new ArgumentException("Invalid class type")
            };

            var booking = new Booking(Guid.NewGuid().ToString(), passenger?.PassengerId, flight.FlightId, classType, price, DateTime.Now);
            bookings.Add(booking);

   
            DataStorage.SaveBookings(bookings);
        }      
         

        public Flight GetFlightById(string flightId)
        {
            return flights.FirstOrDefault(f => f.FlightId == flightId) ?? throw new InvalidOperationException();
        }

    }
}
