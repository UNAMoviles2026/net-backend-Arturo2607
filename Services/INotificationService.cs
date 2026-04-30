namespace reservations_api.Services;

public interface INotificationService
{
    Task SaveDeviceTokenAsync(Guid userId, string deviceToken);
    Task SendReservationConfirmedAsync(Guid userId);
}
