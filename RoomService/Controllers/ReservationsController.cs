using Microsoft.AspNetCore.Mvc;
using RoomService.DTOs;
using RoomService.Services;

namespace RoomService.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationsController(IReservationService reservationService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public ActionResult<ReservationDto> GetById(Guid id)
    {
        var reservation = reservationService.GetById(id);
        return reservation is null ? NotFound() : Ok(reservation);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Cancel(Guid id, [FromQuery] Guid userId)
    {
        var result = reservationService.Cancel(id, userId);
        return result.Error switch
        {
            ReservationError.ReservationNotFound => NotFound(),
            ReservationError.NotOwner => Forbid(),
            _ => NoContent()
        };
    }
}