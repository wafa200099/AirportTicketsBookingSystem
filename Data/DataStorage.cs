using System.Text.Json;
using System.IO;
using AirportTicketsBookingSystem.Models;
using System.Collections.Generic;

namespace AirportTicketsBookingSystem.Data
{
    public static class DataStorage
    {
        private static string FlightsFilePath = "C:\\Users\\wafaalomour\\RiderProjects\\AirportTicketsBookingSystem\\AirportTicketsBookingSystem\\Data\\flights.json";
        private static string BookingsFilePath = "C:\\Users\\wafaalomour\\RiderProjects\\AirportTicketsBookingSystem\\AirportTicketsBookingSystem\\Data\\bookings.json";
        private static string PassengersFilePath = "C:\\Users\\wafaalomour\\RiderProjects\\AirportTicketsBookingSystem\\AirportTicketsBookingSystem\\Data\\passengers.json"; 


        public static void SaveFlights(List<Flight> flights)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(flights);
                File.WriteAllText(FlightsFilePath, jsonData);
                Console.WriteLine($"Flights saved successfully at: {Path.GetFullPath(FlightsFilePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving flights: {ex.Message}");
            }
        }


        public static List<Flight> LoadFlights()
        {
            if (!File.Exists(FlightsFilePath))
            {
                Console.WriteLine($"Flights file not found at: {Path.GetFullPath(FlightsFilePath)}");
                return new List<Flight>();
            }

            var jsonData = File.ReadAllText(FlightsFilePath);
            var flights = JsonSerializer.Deserialize<List<Flight>>(jsonData) ?? new List<Flight>();

            Console.WriteLine("Loaded Flights:");
            foreach (var flight in flights)
            {
                Console.WriteLine(
                    $"FlightId: {flight.FlightId}, Departure: {flight.DepartureCountry}, Destination: {flight.DestinationCountry}, Date: {flight.DepartureDate}");
            }

            return flights;
        }

        public static void SaveBookings(List<Booking> bookings)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(bookings);
                File.WriteAllText(BookingsFilePath, jsonData);
                Console.WriteLine($"Bookings saved successfully at: {Path.GetFullPath(BookingsFilePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving bookings: {ex.Message}");
            }
        }

        public static List<Booking> LoadBookings()
        {
            if (!File.Exists(BookingsFilePath))
            {
                Console.WriteLine($"Bookings file not found at: {Path.GetFullPath(BookingsFilePath)}");
                return new List<Booking>();
            }

            var jsonData = File.ReadAllText(BookingsFilePath);
            var bookings = JsonSerializer.Deserialize<List<Booking>>(jsonData) ?? new List<Booking>();

            Console.WriteLine("Loaded Bookings:");
            foreach (var booking in bookings)
            {
                Console.WriteLine(
                    $"BookingId: {booking.BookingId}, PassengerId: {booking.PassengerId}, FlightId: {booking.FlightId}, Class: {booking.ClassType}, Price: {booking.Price}, Date: {booking.BookingDate}");
            }

            return bookings;
        }


        public static void SavePassengers(List<Passenger> passengers)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(passengers);
                File.WriteAllText(PassengersFilePath, jsonData);
                Console.WriteLine($"Passengers saved successfully at: {Path.GetFullPath(PassengersFilePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving passengers: {ex.Message}");
            }
        }

        public static List<Passenger> LoadPassengers()
        {
            if (!File.Exists(PassengersFilePath))
            {
                Console.WriteLine($"Passengers file not found at: {Path.GetFullPath(PassengersFilePath)}");
                return new List<Passenger>();
            }

            var jsonData = File.ReadAllText(PassengersFilePath);
            var passengers = JsonSerializer.Deserialize<List<Passenger>>(jsonData) ?? new List<Passenger>();

            Console.WriteLine("Loaded Passengers:");
            foreach (var passenger in passengers)
            {
                Console.WriteLine(
                    $"PassengerId: {passenger.PassengerId}, Name: {passenger.FullName}, Email: {passenger.Email}");
            }

            return passengers;
        }
    }
}
