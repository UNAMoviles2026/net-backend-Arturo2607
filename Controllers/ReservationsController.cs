using Microsoft.AspNetCore.Mvc;
using reservations_api.DTOs.Requests;
using reservations_api.DTOs.Responses;
using reservations_api.Services;

namespace reservations_api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    /// <summary>List reservations for a calendar date (ordered by start time).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ReservationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ReservationResponse>>> GetByDate([FromQuery] DateOnly? date)
    {
        if (date is null)
        {
            return BadRequest(new ErrorResponse { Message = "Query parameter 'date' is required (YYYY-MM-DD)." });
        }

        var reservations = await _reservationService.GetByDateAsync(date.Value);
        return Ok(reservations);
    }

    /// <summary>Create a reservation after validating references and classroom availability.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReservationResponse>> Create([FromBody] CreateReservationRequest request)
    {
        var createdReservation = await _reservationService.CreateAsync(request);
        return Ok(createdReservation);
    }

    /// <summary>Delete a reservation by id.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        await _reservationService.DeleteByIdAsync(id);
        return NoContent();
    }
}
