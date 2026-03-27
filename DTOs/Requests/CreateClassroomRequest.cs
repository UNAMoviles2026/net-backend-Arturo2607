using System.ComponentModel.DataAnnotations;

namespace reservations_api.DTOs.Requests;

public class CreateClassroomRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }

    [Required]
    [MaxLength(150)]
    public string Location { get; set; } = string.Empty;
}
