using Microsoft.AspNetCore.Mvc;
using reservations_api.DTOs.Requests;
using reservations_api.DTOs.Responses;
using reservations_api.Services;

namespace reservations_api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>Persist the device token for push notifications (FCM).</summary>
    [HttpPost("token")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MessageResponse>> SaveToken([FromBody] SaveDeviceTokenRequest request)
    {
        await _notificationService.SaveDeviceTokenAsync(request.UserId, request.DeviceToken);
        return Ok(new MessageResponse { Message = "Token saved." });
    }
}
