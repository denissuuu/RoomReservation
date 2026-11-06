using RoomService.Entities;

namespace RoomService.Repositories;

public interface IReservationRepository
{
    IReadOnlyCollection<Reservation> GetAll();
    Reservation? GetById(Guid id);
    IReadOnlyCollection<Reservation> GetByRoomId(Guid roomId);
    void Add(Reservation reservation);
    void Remove(Reservation reservation);
}
