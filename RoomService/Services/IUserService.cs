using RoomService.DTOs;

namespace RoomService.Services;

public interface IUserService
{
    IReadOnlyCollection<UserDto> GetAll();
    UserDto? GetById(Guid id);
    UserDto Create(CreateUserDto request);
    UserDto? Update(Guid id, UpdateUserDto request);
    bool Delete(Guid id);
}
