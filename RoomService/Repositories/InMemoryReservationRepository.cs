using RoomService.Entities;

namespace RoomService.Repositories;

public class InMemoryReservationRepository : IReservationRepository
{
    private readonly List<Reservation> reservations = [];

    public IReadOnlyCollection<Reservation> GetAll() => reservations;

    public Reservation? GetById(Guid id) => reservations.FirstOrDefault(reservation => reservation.Id == id);

    public IReadOnlyCollection<Reservation> GetByRoomId(Guid roomId) => reservations
        .Where(reservation => reservation.RoomId == roomId)
        .ToArray();

    public void Add(Reservation reservation) => reservations.Add(reservation);

    public void Remove(Reservation reservation) => reservations.Remove(reservation);
}
