using Microsoft.AspNetCore.Mvc;
using reservations_api.DTOs.Requests;
using reservations_api.DTOs.Responses;
using reservations_api.Services;

namespace reservations_api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class ClassroomsController : ControllerBase
{
    private readonly IClassroomService _classroomService;

    public ClassroomsController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ClassroomResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClassroomResponse>>> GetAll()
    {
        var classrooms = await _classroomService.GetAllAsync();
        return Ok(classrooms);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClassroomResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClassroomResponse>> GetById(Guid id)
    {
        var classroom = await _classroomService.GetByIdAsync(id);

        if (classroom is null)
        {
            return NotFound(new ErrorResponse { Message = "Classroom not found." });
        }

        return Ok(classroom);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClassroomResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClassroomResponse>> Create([FromBody] CreateClassroomRequest request)
    {
        var createdClassroom = await _classroomService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdClassroom.Id },
            createdClassroom);
    }
}
