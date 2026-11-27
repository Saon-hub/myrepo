using System;
using System.Collections.Generic;
using System.Text;

namespace ArtistryApp.Model
{
    public class ClassStudentResponse
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public List<ClassStudent> Data { get; set; } = new();
    }
}
