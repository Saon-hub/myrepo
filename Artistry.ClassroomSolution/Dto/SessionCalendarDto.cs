namespace Artistry.ClassroomSolution.Dto
{
    public class SessionCalendarDto
    {
        public long Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
    }
}
