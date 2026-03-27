using Microsoft.EntityFrameworkCore;
using reservations_api.Data;
using reservations_api.Models.Entities;

namespace reservations_api.Repositories;

public class ClassroomRepository : IClassroomRepository
{
    private readonly AppDbContext _context;

    public ClassroomRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Classroom>> GetAllAsync()
    {
        return await _context.Classrooms
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Classroom?> GetByIdAsync(Guid id)
    {
        return await _context.Classrooms
            .AsNoTracking()
            .FirstOrDefaultAsync(classroom => classroom.Id == id);
    }

    public async Task<Classroom> AddAsync(Classroom classroom)
    {
        await _context.Classrooms.AddAsync(classroom);
        await _context.SaveChangesAsync();
        return classroom;
    }
}
