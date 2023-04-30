namespace Techies.Registration.Models;

public class Seat
{
    public int Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid EventId { get; set; }
    public byte[] Version { get; set; }
}
