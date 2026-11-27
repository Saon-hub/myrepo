using System;
using System.Collections.Generic;
using System.Text;

namespace ArtistryApp.Model
{
    

    public class TeacherResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public int UserTypeId { get; set; }

        public string TeacherName =>
            $"{FirstName} {MiddleName} {LastName}"
            .Replace("  ", " ")
            .Trim();
    }

    
    public class SessionCreateRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int TeacherId { get; set; }

        public string Date { get; set; } = string.Empty;

        public string StartTime { get; set; } = string.Empty;

        public string EndTime { get; set; } = string.Empty;

        public int CreatedBy { get; set; }
    }

}
