namespace Techies.Registration.Models;

public class Event
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int NumberOfSeats { get; set; }
    public DateTime EventDate { get; set; }
    public virtual ICollection<Seat> Seats { get; set; }
    public int AvailableSeats => Seats.Count(s => s.UserId == null);
    public bool HasAvailableSeat => Seats.Any(s => s.UserId == null);
    public Seat? GetNextAvailableSeat()
    {
        return Seats?.FirstOrDefault(s => s.UserId == null);
    }
}