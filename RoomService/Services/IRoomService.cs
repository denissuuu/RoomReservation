using RoomService.DTOs;

namespace RoomService.Services;

public interface IRoomService
{
    IReadOnlyCollection<RoomDto> GetAll();
    IReadOnlyCollection<RoomDto> GetAvailable(DateTime startAt, DateTime endAt);
    RoomDto? GetById(Guid id);
    RoomDto Create(CreateRoomDto request);
    RoomDto? Update(Guid id, UpdateRoomDto request);
    bool Delete(Guid id);
}
