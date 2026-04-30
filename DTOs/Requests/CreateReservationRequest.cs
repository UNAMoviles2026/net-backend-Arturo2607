using System.ComponentModel.DataAnnotations;

namespace reservations_api.DTOs.Requests;

public sealed class CreateReservationRequest : IValidatableObject
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid ClassroomId { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "EndTime must be after StartTime.",
                [nameof(EndTime), nameof(StartTime)]);
        }
    }
}
