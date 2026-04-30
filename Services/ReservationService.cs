using reservations_api.DTOs.Requests;
using reservations_api.DTOs.Responses;
using reservations_api.Exceptions;
using reservations_api.Mappers;
using reservations_api.Models.Entities;
using reservations_api.Repositories;

namespace reservations_api.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IClassroomRepository _classroomRepository;
    private readonly INotificationService _notificationService;

    public ReservationService(
        IReservationRepository reservationRepository,
        IUserRepository userRepository,
        IClassroomRepository classroomRepository,
        INotificationService notificationService)
    {
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _classroomRepository = classroomRepository;
        _notificationService = notificationService;
    }

    public async Task<ReservationResponse> CreateAsync(CreateReservationRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
        {
            throw new BadRequestDomainException("User not found.");
        }

        var classroom = await _classroomRepository.GetByIdAsync(request.ClassroomId);
        if (classroom is null)
        {
            throw new BadRequestDomainException("Classroom not found.");
        }

        var existingReservations = await _reservationRepository.GetByClassroomAndDateAsync(
            request.ClassroomId,
            request.Date);

        if (HasOverlap(request.StartTime, request.EndTime, existingReservations))
        {
            throw new ConflictDomainException("Time conflict with another reservation.");
        }

        var reservation = ReservationMapper.ToEntity(request);
        var createdReservation = await _reservationRepository.AddAsync(reservation);
        await _notificationService.SendReservationConfirmedAsync(createdReservation.UserId);

        return ReservationMapper.ToResponse(createdReservation);
    }

    public async Task<List<ReservationResponse>> GetByDateAsync(DateOnly date)
    {
        var reservations = await _reservationRepository.GetByDateAsync(date);
        return reservations.Select(ReservationMapper.ToResponse).ToList();
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var deleted = await _reservationRepository.DeleteByIdAsync(id);
        if (!deleted)
        {
            throw new NotFoundDomainException("Reservation not found.");
        }
    }

    private static bool HasOverlap(TimeOnly startTime, TimeOnly endTime, List<Reservation> existingReservations)
    {
        return existingReservations.Any(r =>
            startTime < r.EndTime && endTime > r.StartTime);
    }
}
