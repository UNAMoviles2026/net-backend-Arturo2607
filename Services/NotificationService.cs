using reservations_api.Exceptions;
using reservations_api.Repositories;

namespace reservations_api.Services;

public class NotificationService : INotificationService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IUserRepository userRepository, ILogger<NotificationService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task SaveDeviceTokenAsync(Guid userId, string deviceToken)
    {
        var saved = await _userRepository.SaveDeviceTokenAsync(userId, deviceToken);
        if (!saved)
        {
            throw new NotFoundDomainException("User not found");
        }
    }

    public async Task SendReservationConfirmedAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null || string.IsNullOrWhiteSpace(user.DeviceToken))
        {
            return;
        }

        // Simulated FCM call for academic project setup.
        _logger.LogInformation(
            "FCM notification sent to token {DeviceToken}. Message: Reservation confirmed",
            user.DeviceToken);
    }
}
