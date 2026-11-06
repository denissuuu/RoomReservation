using RoomService.Entities;

namespace RoomService.Repositories;

public class InMemoryRoomRepository : IRoomRepository
{
    private readonly List<Room> rooms = [];

    public IReadOnlyCollection<Room> GetAll() => rooms;

    public Room? GetById(Guid id) => rooms.FirstOrDefault(room => room.Id == id);

    public void Add(Room room) => rooms.Add(room);

    public void Remove(Room room) => rooms.Remove(room);
}
