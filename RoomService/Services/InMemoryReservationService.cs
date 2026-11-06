using RoomService.DTOs;
using RoomService.Entities;
using RoomService.Repositories;

namespace RoomService.Services;

public class InMemoryReservationService(
    IReservationRepository reservationRepository,
    IRoomRepository roomRepository,
    IUserRepository userRepository) : IReservationService
{
    public IReadOnlyCollection<ReservationDto> GetByRoomId(Guid roomId) => reservationRepository
        .GetByRoomId(roomId)
        .Select(ToDto)
        .ToArray();

    public ReservationDto? GetById(Guid id) => reservationRepository.GetById(id) is { } reservation
        ? ToDto(reservation)
        : null;

    public ReservationCreateResult Create(Guid roomId, CreateReservationDto request)
    {
        if (roomRepository.GetById(roomId) is null)
        {
            return new(null, ReservationError.RoomNotFound);
        }

        if (userRepository.GetById(request.UserId) is null)
        {
            return new(null, ReservationError.UserNotFound);
        }

        if (request.StartAt >= request.EndAt)
        {
            return new(null, ReservationError.InvalidInterval);
        }

        var hasConflict = reservationRepository.GetByRoomId(roomId)
            .Any(existing => request.StartAt < existing.EndAt && request.EndAt > existing.StartAt);
        if (hasConflict)
        {
            return new(null, ReservationError.Conflict);
        }

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = roomId,
            UserId = request.UserId,
            StartAt = request.StartAt,
            EndAt = request.EndAt
        };

        reservationRepository.Add(reservation);
        return new(ToDto(reservation), ReservationError.None);
    }

    public ReservationCancelResult Cancel(Guid reservationId, Guid userId)
    {
        var reservation = reservationRepository.GetById(reservationId);
        if (reservation is null)
        {
            return new(false, ReservationError.ReservationNotFound);
        }

        if (reservation.UserId != userId)
        {
            return new(false, ReservationError.NotOwner);
        }

        reservationRepository.Remove(reservation);
        return new(true, ReservationError.None);
    }

    private static ReservationDto ToDto(Reservation reservation) => new()
    {
        Id = reservation.Id,
        RoomId = reservation.RoomId,
        UserId = reservation.UserId,
        StartAt = reservation.StartAt,
        EndAt = reservation.EndAt
    };
}
