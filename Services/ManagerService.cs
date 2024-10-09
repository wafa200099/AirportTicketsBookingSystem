using AirportTicketsBookingSystem.Data;
using AirportTicketsBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportTicketsBookingSystem.Services
{
    public class ManagerService
    {
        private List<Booking> bookings;
        private List<Flight> flights; 

        public ManagerService()
        {
            bookings = DataStorage.LoadBookings();
            flights = DataStorage.LoadFlights(); 
        }

        public List<Booking> FilterBookings(string departureCountry, string destinationCountry, DateTime? departureDate = null, string classType = null)
        {
            // Create a list to hold filtered bookings
            var filteredBookings = new List<Booking>();

            foreach (var booking in bookings)
            {
                var flight = flights.FirstOrDefault(f => f.FlightId == booking.FlightId); // Get the flight details

                if (flight != null && 
                    flight.DepartureCountry == departureCountry && 
                    flight.DestinationCountry == destinationCountry && 
                    (!departureDate.HasValue || flight.DepartureDate.Date == departureDate.Value.Date) && 
                    (string.IsNullOrEmpty(classType) || booking.ClassType == classType))
                {
                    filteredBookings.Add(booking);
                }
            }

            return filteredBookings;
        }
    }
}