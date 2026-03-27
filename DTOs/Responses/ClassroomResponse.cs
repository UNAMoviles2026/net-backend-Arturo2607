namespace reservations_api.DTOs.Responses;

public class ClassroomResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Location { get; set; } = string.Empty;
}
