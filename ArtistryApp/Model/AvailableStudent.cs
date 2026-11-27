namespace ArtistryApp.Model;

public class AvailableStudent
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;
}

public class ApiResponse<T>
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public T? Data { get; set; }
}


public class UserCreateResponse
{
    public int Id { get; set; }
}


public class EntityResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int EntityTypeId { get; set; }
}