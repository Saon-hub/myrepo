namespace ArtistryApp.Models
{
    public class CalendarResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<CalendarEvent> Data { get; set; } = new();
    }
}