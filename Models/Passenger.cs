namespace AirportTicketsBookingSystem.Models;

public class Passenger
{
    public string PassengerId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }

    public Passenger(string passengerId, string fullName, string email)
    {
        PassengerId = passengerId;
        FullName = fullName;
        Email = email;
    }
    
}
