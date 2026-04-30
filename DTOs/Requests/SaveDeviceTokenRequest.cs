using System.ComponentModel.DataAnnotations;

namespace reservations_api.DTOs.Requests;

public sealed class SaveDeviceTokenRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MinLength(1)]
    public string DeviceToken { get; set; } = string.Empty;
}
