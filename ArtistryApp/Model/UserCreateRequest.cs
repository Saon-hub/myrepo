

namespace ArtistryApp.Model;

public class UserCreateRequest
{
    public int Id { get; set; } = 0;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public string? Comments { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string LoginId { get; set; } = string.Empty;

    public int UserTypeId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; } = 0;

    public DateTime ModifiedDate { get; set; }
}
