using Microsoft.AspNetCore.Mvc;
using RoomService.DTOs;
using RoomService.Services;
namespace RoomService.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<UserDto>> GetAll() => Ok(userService.GetAll());

    [HttpGet("{id:guid}")]
    public ActionResult<UserDto> GetById(Guid id)
    {
        var user = userService.GetById(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public ActionResult<UserDto> Create(CreateUserDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("Le nom et l'adresse e-mail sont obligatoires.");
        }

        var user = userService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<UserDto> Update(Guid id, UpdateUserDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("Le nom et l'adresse e-mail sont obligatoires.");
        }

        var user = userService.Update(id, request);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id) => userService.Delete(id) ? NoContent() : NotFound();
}
