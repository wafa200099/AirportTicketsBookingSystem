namespace AirportTicketsBookingSystem.Models;

public class Flight
{
    public string FlightId { get; set; }
    public string DepartureCountry { get; set; }
    public string DestinationCountry { get; set; }
    public DateTime DepartureDate { get; set; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public decimal EconomyPrice { get; set; }
    public decimal BusinessPrice { get; set; }
    public decimal FirstClassPrice { get; set; }

    public Flight(string flightId, string departureCountry, string destinationCountry, DateTime departureDate, 
        string departureAirport, string arrivalAirport, decimal economyPrice, decimal businessPrice, 
        decimal firstClassPrice)
    {
        FlightId = flightId;
        DepartureCountry = departureCountry;
        DestinationCountry = destinationCountry;
        DepartureDate = departureDate;
        DepartureAirport = departureAirport;
        ArrivalAirport = arrivalAirport;
        EconomyPrice = economyPrice;
        BusinessPrice = businessPrice;
        FirstClassPrice = firstClassPrice;
    }

}