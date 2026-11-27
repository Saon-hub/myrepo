namespace ArtistryApp.Models
{
    public class CalendarEvent
    {
        public int Id { get; set; }
        public string? ClassName { get; set; }
        public string? Description { get; set; }
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public DateTime Date { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
    }
}