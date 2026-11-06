using RoomService.Entities;

namespace RoomService.Repositories;

public interface IRoomRepository
{
    IReadOnlyCollection<Room> GetAll();
    Room? GetById(Guid id);
    void Add(Room room);
    void Remove(Room room);
}
