using RoomService.DTOs;
using RoomService.Entities;
using RoomService.Repositories;

namespace RoomService.Services;

public class InMemoryRoomService(
    IRoomRepository roomRepository,
    IReservationRepository reservationRepository) : IRoomService
{
    public IReadOnlyCollection<RoomDto> GetAll() => roomRepository.GetAll().Select(ToDto).ToArray();

    public IReadOnlyCollection<RoomDto> GetAvailable(DateTime startAt, DateTime endAt) => roomRepository
        .GetAll()
        .Where(room => reservationRepository.GetByRoomId(room.Id)
            .All(reservation => startAt >= reservation.EndAt || endAt <= reservation.StartAt))
        .Select(ToDto)
        .ToArray();

    public RoomDto? GetById(Guid id) => roomRepository.GetById(id) is { } room
        ? ToDto(room)
        : null;

    public RoomDto Create(CreateRoomDto request)
    {
        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Capacity = request.Capacity
        };

        roomRepository.Add(room);
        return ToDto(room);
    }

    public RoomDto? Update(Guid id, UpdateRoomDto request)
    {
        var room = roomRepository.GetById(id);
        if (room is null)
        {
            return null;
        }

        room.Name = request.Name.Trim();
        room.Capacity = request.Capacity;
        return ToDto(room);
    }

    public bool Delete(Guid id)
    {
        var room = roomRepository.GetById(id);
        if (room is null)
        {
            return false;
        }

        roomRepository.Remove(room);
        return true;
    }

    private RoomDto ToDto(Room room) => new()
    {
        Id = room.Id,
        Name = room.Name,
        Capacity = room.Capacity,
        Reservations = reservationRepository.GetByRoomId(room.Id).Select(reservation => new ReservationDto
        {
            Id = reservation.Id,
            UserId = reservation.UserId,
            StartAt = reservation.StartAt,
            EndAt = reservation.EndAt
        }).ToList()
    };
}
