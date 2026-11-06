using RoomService.Entities;

namespace RoomService.Repositories;

public interface IUserRepository
{
    IReadOnlyCollection<User> GetAll();
    User? GetById(Guid id);
    void Add(User user);
    void Remove(User user);
}
