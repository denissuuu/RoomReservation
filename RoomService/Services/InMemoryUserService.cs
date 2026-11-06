using RoomService.DTOs;
using RoomService.Entities;
using RoomService.Repositories;

namespace RoomService.Services;

public class InMemoryUserService(IUserRepository userRepository) : IUserService
{
    public IReadOnlyCollection<UserDto> GetAll() => userRepository.GetAll().Select(ToDto).ToArray();

    public UserDto? GetById(Guid id) => userRepository.GetById(id) is { } user
        ? ToDto(user)
        : null;

    public UserDto Create(CreateUserDto request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = request.Email.Trim()
        };

        userRepository.Add(user);
        return ToDto(user);
    }

    public UserDto? Update(Guid id, UpdateUserDto request)
    {
        var user = userRepository.GetById(id);
        if (user is null)
        {
            return null;
        }

        user.Name = request.Name.Trim();
        user.Email = request.Email.Trim();
        return ToDto(user);
    }

    public bool Delete(Guid id)
    {
        var user = userRepository.GetById(id);
        if (user is null)
        {
            return false;
        }

        userRepository.Remove(user);
        return true;
    }

    private static UserDto ToDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email
    };
}
