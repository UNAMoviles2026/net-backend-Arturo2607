using reservations_api.Models.Entities;

namespace reservations_api.Repositories;

public interface IClassroomRepository
{
    Task<List<Classroom>> GetAllAsync();
    Task<Classroom?> GetByIdAsync(Guid id);
    Task<Classroom> AddAsync(Classroom classroom);
}
