using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AirportTicketsBookingSystem;
using AirportTicketsBookingSystem.Data;
using AirportTicketsBookingSystem.Models;
using AirportTicketsBookingSystem.Services;

class Program
{
    static void Main(string[] args)
    {
        PassengerService passengerService = new PassengerService();
        BookingService bookingService = new BookingService();
        ManagerService managerService = new ManagerService();
        FlightImporter flightImporter = new FlightImporter();
        
        while (true)
        {
            Console.WriteLine("Welcome to the Airport Ticket Booking System");
            Console.WriteLine("1. Passenger Menu");
            Console.WriteLine("2. Manager Menu");
            Console.WriteLine("3. Exit");

            var mainOption = Console.ReadLine();
            switch (mainOption)
            {
                case "1":
                    PassengerMenu(passengerService, bookingService);
                    break;
                case "2":
                    ManagerMenu(managerService, flightImporter);
                    break;
                case "3":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void PassengerMenu(PassengerService passengerService, BookingService bookingService)
    {
        while (true)
        {
            Console.WriteLine("Passenger Menu:");
            Console.WriteLine("1. Search for Flights");
            Console.WriteLine("2. Book a Flight");
            Console.WriteLine("3. View Bookings");
            Console.WriteLine("4. Cancel a Booking");
            Console.WriteLine("5. Modify a Booking");
            Console.WriteLine("6. Back to Main Menu");

            var passengerOption = Console.ReadLine();
            switch (passengerOption)
            {
                case "1":
                    SearchFlights(passengerService);
                    break;
                case "2":
                    BookFlight(passengerService, bookingService);
                    break;
                case "3":
                    ViewBookings(bookingService);
                    break;
                case "4":
                    CancelBooking(bookingService);
                    break;
                case "5":
                    ModifyBooking(bookingService);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void ManagerMenu(ManagerService managerService, FlightImporter flightImporter)
    {
        while (true)
        {
            Console.WriteLine("Manager Menu:");
            Console.WriteLine("1. Filter Bookings");
            Console.WriteLine("2. Import Flights from CSV");
            Console.WriteLine("3. Back to Main Menu");

            var managerOption = Console.ReadLine();
            switch (managerOption)
            {
                case "1":
                    FilterBookings(managerService);
                    break;
                case "2":
                    ImportFlights(flightImporter);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void SearchFlights(PassengerService passengerService)
    {
        Console.WriteLine("Enter Departure Country:");
        string departureCountry = Console.ReadLine();

        Console.WriteLine("Enter Destination Country:");
        string destinationCountry = Console.ReadLine();

        Console.WriteLine("Enter Departure Date (yyyy-MM-dd) or press Enter to skip:");
        string dateInput = Console.ReadLine();
        DateTime? departureDate = null;
        if (!string.IsNullOrEmpty(dateInput))
        {
            departureDate = DateTime.Parse(dateInput);
        }

        var flights = passengerService.SearchFlights(departureCountry, destinationCountry, departureDate);
        if (flights.Count == 0)
        {
            Console.WriteLine("No flights found.");
            return;
        }

        foreach (var flight in flights)
        {
            Console.WriteLine(
                $"Flight ID: {flight.FlightId} From {flight.DepartureCountry} to {flight.DestinationCountry}, Date: {flight.DepartureDate}");
        }
    }

    static void BookFlight(PassengerService passengerService, BookingService bookingService)
    {
        Console.WriteLine("Enter Passenger ID:");
        string passengerId = Console.ReadLine().Trim();

        Console.WriteLine("Enter Full Name:");
        string fullName = Console.ReadLine().Trim();

        Console.WriteLine("Enter Email:");
        string email = Console.ReadLine().Trim();

        // Validate passenger information
        if (string.IsNullOrEmpty(passengerId) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email))
        {
            Console.WriteLine("Passenger information is incomplete. Please try again.");
            return;
        }

        // Create a new Passenger object
        Passenger passenger = new Passenger(passengerId, fullName, email);

        Console.WriteLine("Enter Flight ID to book:");
        string flightNumber = Console.ReadLine().Trim();

        // Validate flight number input
        if (string.IsNullOrEmpty(flightNumber))
        {
            Console.WriteLine("Flight number is required.");
            return;
        }

        // Search for the flight by flight number
        Flight flight = passengerService.GetFlightById(flightNumber);

        if (flight == null)
        {
            Console.WriteLine("Flight not found. Please ensure the flight number is correct.");
            return;
        }

        Console.WriteLine("Select Class (Economy, Business, First Class):");
        string classType = Console.ReadLine().Trim();

        // Validate class type
        if (string.IsNullOrEmpty(classType) ||
            !(classType.Equals("Economy", StringComparison.OrdinalIgnoreCase) ||
              classType.Equals("Business", StringComparison.OrdinalIgnoreCase) ||
              classType.Equals("First Class", StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Invalid class type. Please choose from Economy, Business, or First Class.");
            return;
        }

        // Proceed to book the flight
        try
        {
            passengerService.BookFlight(passenger, flight, classType);
            Console.WriteLine("Flight booked successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Booking failed: {ex.Message}");
        }
    }


    static void ViewBookings(BookingService bookingService)
    {
        Console.WriteLine("Enter Passenger ID:");
        string passengerId = Console.ReadLine();

        var bookings = bookingService.ViewBookings(passengerId);
        if (bookings.Count == 0)
        {
            Console.WriteLine("No bookings found for this passenger.");
            return;
        }

        foreach (var booking in bookings)
        {
            Console.WriteLine(
                $"Booking ID: {booking.BookingId}, Flight: {booking.FlightId}, Class: {booking.ClassType}, Price: {booking.Price}");
        }
    }

    static void CancelBooking(BookingService bookingService)
    {
        Console.WriteLine("Enter Booking ID to cancel:");
        string bookingId = Console.ReadLine().Trim();

        // Validate the input
        if (string.IsNullOrEmpty(bookingId))
        {
            Console.WriteLine("Booking ID cannot be empty. Please try again.");
            return;
        }

        // Check if the booking exists
        var booking = bookingService.ViewBookingById(bookingId);
        if (booking == null)
        {
            Console.WriteLine($"Booking with ID {bookingId} not found.");
            return;
        }

        // Confirm cancellation with the user
        Console.WriteLine(
            $"Booking found: Booking ID: {booking.BookingId}, Passenger: {booking.PassengerId}, Flight: {booking.FlightId}, Class: {booking.ClassType}, Price: {booking.Price:C}");
        Console.WriteLine("Are you sure you want to cancel this booking? (yes/no)");
        string confirmation = Console.ReadLine().Trim().ToLower();

        if (confirmation == "yes")
        {
            // Cancel the booking
            bookingService.CancelBooking(bookingId);
            Console.WriteLine($"Booking with ID {bookingId} has been successfully canceled.");
        }
        else
        {
            Console.WriteLine("Booking cancellation aborted.");
        }
    }


    static void ModifyBooking(BookingService bookingService)
    {
        Console.WriteLine("Enter Booking ID to modify:");
        string bookingId = Console.ReadLine().Trim();

        // Validate the input
        if (string.IsNullOrEmpty(bookingId))
        {
            Console.WriteLine("Booking ID cannot be empty. Please try again.");
            return;
        }

        // Check if the booking exists
        var booking = bookingService.ViewBookingById(bookingId);
        if (booking == null)
        {
            Console.WriteLine($"Booking with ID {bookingId} not found.");
            return;
        }

        // Display current booking details
        Console.WriteLine(
            $"Current Booking: Booking ID: {booking.BookingId}, Passenger: {booking.PassengerId}, Flight: {booking.FlightId}, Class: {booking.ClassType}, Price: {booking.Price:C}");

        // Ask for new class type
        Console.WriteLine("Enter new Class (Economy, Business, First Class):");
        string newClassType = Console.ReadLine().Trim();

        // Validate the class type
        if (newClassType != "Economy" && newClassType != "Business" && newClassType != "First Class")
        {
            Console.WriteLine("Invalid class type. Please enter either 'Economy', 'Business', or 'First Class'.");
            return;
        }

        // Confirm modification with the user
        Console.WriteLine($"Are you sure you want to change the class to {newClassType}? (yes/no)");
        string confirmation = Console.ReadLine().Trim().ToLower();

        if (confirmation == "yes")
        {
            // Modify the booking
            bookingService.ModifyBooking(bookingId, newClassType);
            Console.WriteLine("Booking modified successfully.");
        }
        else
        {
            Console.WriteLine("Modification aborted.");
        }
    }

    static void FilterBookings(ManagerService managerService)
    {
        Console.WriteLine("Enter Departure Country:");
        string departureCountry = Console.ReadLine();

        Console.WriteLine("Enter Destination Country:");
        string destinationCountry = Console.ReadLine();

        Console.WriteLine("Enter Departure Date (yyyy-MM-dd) or press Enter to skip:");
        string dateInput = Console.ReadLine();
        DateTime? departureDate = null;
        if (!string.IsNullOrEmpty(dateInput))
        {
            departureDate = DateTime.Parse(dateInput);
        }

        var filteredBookings = managerService.FilterBookings(departureCountry, destinationCountry, departureDate);
        if (filteredBookings.Count == 0)
        {
            Console.WriteLine("No bookings found with the specified criteria.");
            return;
        }

        foreach (var booking in filteredBookings)
        {
            Console.WriteLine(
                $"Booking ID: {booking.BookingId}, Passenger: {booking.PassengerId}, Flight: {booking.FlightId}");
        }
    }

    public static void ImportFlights(FlightImporter flightImporter)
    {
        Console.WriteLine("Enter the path to the CSV file:");
        string filePath = Console.ReadLine();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        var flights = flightImporter.ImportFlights(filePath);
        if (flights.Count > 0)
        {
            Console.WriteLine($"{flights.Count} flights imported successfully.");
        }
        else
        {
            Console.WriteLine("No flights were imported. Please check the CSV file.");
        }
    }
}