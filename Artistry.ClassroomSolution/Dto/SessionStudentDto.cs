namespace Artistry.ClassroomSolution.Models;

public class SessionStudentDto
{
    public long AssociationId { get; set; }
    public long SessionId { get; set; }
    public long StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public bool Attendance { get; set; }
    public DateOnly? DateOfAttendance { get; set; }
}