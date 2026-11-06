using RoomService.Entities;

namespace RoomService.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> users = [];

    public IReadOnlyCollection<User> GetAll() => users;

    public User? GetById(Guid id) => users.FirstOrDefault(user => user.Id == id);

    public void Add(User user) => users.Add(user);

    public void Remove(User user) => users.Remove(user);
}
