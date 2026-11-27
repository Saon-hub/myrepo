
namespace Artistry.ClassroomSolution.Models;

public class SessionCreateRequest
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public long TeacherId { get; set; }

    public string Date { get; set; } = string.Empty;

    public string StartTime { get; set; } = string.Empty;

    public string EndTime { get; set; } = string.Empty;

    public long CreatedBy { get; set; }
}
