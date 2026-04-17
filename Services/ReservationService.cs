using reservations_api.DTOs.Requests;
using reservations_api.DTOs.Responses;
using reservations_api.Mappers;
using reservations_api.Models.Entities;
using reservations_api.Repositories;

namespace reservations_api.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;

    public ReservationService(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<ReservationResponse> CreateAsync(CreateReservationRequest request)
    {
        if (request.EndTime <= request.StartTime)
        {
            throw new ArgumentException("End time must be after start time.");
        }

        var existingReservations = await _reservationRepository.GetByClassroomAndDateAsync(request.ClassroomId, request.Date);
        
        if (HasOverlap(request.StartTime, request.EndTime, existingReservations))
        {
            throw new InvalidOperationException("The classroom is already reserved for the specified time.");
        }
        
        var reservation = ReservationMapper.ToEntity(request);
        var createdReservation = await _reservationRepository.AddAsync(reservation);
        
        return ReservationMapper.ToResponse(createdReservation);
    }

    private bool HasOverlap(TimeOnly startTime, TimeOnly endTime, List<Reservation> existingReservations)
    {
        return existingReservations.Any(r => startTime < r.EndTime && endTime > r.StartTime);
    }
}
