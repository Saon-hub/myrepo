namespace Artistry.ClassroomSolution.Model
{
    public class User
    {
        public long Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Comments { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? LoginId { get; set; }
        public long? UserTypeId { get; set; }

        public DateTime CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
