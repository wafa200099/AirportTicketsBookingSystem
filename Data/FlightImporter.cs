using AirportTicketsBookingSystem.Models;

namespace AirportTicketsBookingSystem.Data;

public class FlightImporter
{
    public List<Flight> ImportFlights(string filePath)
    {
        var flights = new List<Flight>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var data = line.Split(',');
            if (data.Length != 9)
            {
                Console.WriteLine($"Skipping invalid line: {line}");
                continue;
            }

            if (!DateTime.TryParse(data[3], out var departureDate) ||
                !decimal.TryParse(data[6], out var economyPrice) ||
                !decimal.TryParse(data[7], out var businessPrice) ||
                !decimal.TryParse(data[8], out var firstClassPrice))
            {
                Console.WriteLine($"Skipping line due to parsing error: {line}");
                continue;
            }

            var flight = new Flight(
                data[0].Trim(),
                data[1].Trim(),
                data[2].Trim(),
                departureDate,
                data[4].Trim(),
                data[5].Trim(),
                economyPrice,
                businessPrice,
                firstClassPrice
            );

            flights.Add(flight);
        }
        return flights;
    }

}