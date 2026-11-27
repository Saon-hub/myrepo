using System;
using System.Collections.Generic;
using System.Text;

namespace ArtistryApp.Model
{
    public class ClassStudent
    {
        public int AssociationId { get; set; }

        public int SessionId { get; set; }

        public int StudentId { get; set; }

        public string? StudentName { get; set; }

        public bool Attendance { get; set; }

        public DateTime? DateOfAttendance { get; set; }
    }
}
