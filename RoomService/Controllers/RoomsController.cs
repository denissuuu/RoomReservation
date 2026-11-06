using Microsoft.AspNetCore.Mvc;
using RoomService.DTOs;
using RoomService.Services;

namespace RoomService.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController(IRoomService roomService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<RoomDto>> GetAll() => Ok(roomService.GetAll());

    [HttpGet("available")]
    public ActionResult<IReadOnlyCollection<RoomDto>> GetAvailable(DateTime startAt, DateTime endAt)
    {
        if (startAt >= endAt)
        {
            return BadRequest("Le début du créneau doit être antérieur à sa fin.");
        }

        return Ok(roomService.GetAvailable(startAt, endAt));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<RoomDto> GetById(Guid id)
    {
        var room = roomService.GetById(id);
        return room is null ? NotFound() : Ok(room);
    }

    [HttpPost]
    public ActionResult<RoomDto> Create(CreateRoomDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Capacity <= 0)
        {
            return BadRequest("Le nom est obligatoire et la capacité doit être supérieure à zéro.");
        }

        var room = roomService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpGet("{roomId:guid}/reservations")]
    public ActionResult<IReadOnlyCollection<ReservationDto>> GetReservations(
        Guid roomId,
        [FromServices] IReservationService reservationService)
    {
        if (roomService.GetById(roomId) is null)
        {
            return NotFound();
        }

        return Ok(reservationService.GetByRoomId(roomId));
    }

    [HttpPost("{roomId:guid}/reservations")]
    public ActionResult<ReservationDto> CreateReservation(
        Guid roomId,
        CreateReservationDto request,
        [FromServices] IReservationService reservationService)
    {
        var result = reservationService.Create(roomId, request);
        return result.Error switch
        {
            ReservationError.RoomNotFound => NotFound("La salle demandée n'existe pas."),
            ReservationError.UserNotFound => NotFound("L'utilisateur demandé n'existe pas."),
            ReservationError.InvalidInterval => BadRequest("Le début du créneau doit être antérieur à sa fin."),
            ReservationError.Conflict => Conflict("La salle est déjà réservée sur ce créneau."),
            _ => CreatedAtAction(
                nameof(ReservationsController.GetById),
                "Reservations",
                new { id = result.Reservation!.Id },
                result.Reservation)
        };
    }

    [HttpPut("{id:guid}")]
    public ActionResult<RoomDto> Update(Guid id, UpdateRoomDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Capacity <= 0)
        {
            return BadRequest("Le nom est obligatoire et la capacité doit être supérieure à zéro.");
        }

        var room = roomService.Update(id, request);
        return room is null ? NotFound() : Ok(room);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id) => roomService.Delete(id) ? NoContent() : NotFound();
}