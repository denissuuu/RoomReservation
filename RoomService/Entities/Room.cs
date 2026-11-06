namespace RoomService.Entities;

public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public List<Reservation> Reservations { get; set; } = [];
}
