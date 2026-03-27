using Microsoft.AspNetCore.Mvc;
using reservations_api.DTOs.Requests;
using reservations_api.Services;

namespace reservations_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassroomsController : ControllerBase
{
    private readonly IClassroomService _classroomService;

    public ClassroomsController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var classrooms = await _classroomService.GetAllAsync();
        return Ok(classrooms);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var classroom = await _classroomService.GetByIdAsync(id);

        if (classroom is null)
        {
            return NotFound();
        }

        return Ok(classroom);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClassroomRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var createdClassroom = await _classroomService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdClassroom.Id },
            createdClassroom);
    }
}
