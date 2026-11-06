using RoomService.DTOs;

namespace RoomService.Services;

public interface IReservationService
{
    IReadOnlyCollection<ReservationDto> GetByRoomId(Guid roomId);
    ReservationDto? GetById(Guid id);
    ReservationCreateResult Create(Guid roomId, CreateReservationDto request);
    ReservationCancelResult Cancel(Guid reservationId, Guid userId);
}

public sealed record ReservationCreateResult(ReservationDto? Reservation, ReservationError Error)
{
    public bool Succeeded => Reservation is not null && Error == ReservationError.None;
}

public sealed record ReservationCancelResult(bool Succeeded, ReservationError Error);

public enum ReservationError
{
    None,
    RoomNotFound,
    UserNotFound,
    InvalidInterval,
    Conflict,
    ReservationNotFound,
    NotOwner
}
