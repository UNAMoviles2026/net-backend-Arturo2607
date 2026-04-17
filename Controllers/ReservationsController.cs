using Microsoft.AspNetCore.Mvc;
using reservations_api.DTOs.Requests;
using reservations_api.Services;

namespace reservations_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var createdReservation = await _reservationService.CreateAsync(request);
            return CreatedAtAction(
                nameof(Create),
                new { id = createdReservation.Id },
                createdReservation);
        }
        catch (ArgumentException ex)
        {
            if (ex.Message.Contains("End time must be after start time"))
            {
                return BadRequest(new { error = ex.Message });
            }

            if (ex.Message.Contains("The classroom is already reserved for the specified time"))
            {
                return Conflict(new { error = ex.Message });
            }

            throw;
        }
    }
}
