using ArtistryApp.Model;
using ArtistryApp.Models;
using System.Net.Http.Json;
using System.Text.Json;

public class CalendarService
{
    private readonly HttpClient _httpClient;

    public CalendarService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CalendarEvent>> GetCalendarDataAsync()
    {
        var response = await _httpClient.GetAsync(
            "api/Session/calendar");

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<CalendarResponse>();

        if (result == null || !result.Success)
        {
            throw new Exception(
                result?.Message ?? "Unable to retrieve calendar data.");
        }

        return result.Data ?? new List<CalendarEvent>();
    }

   

public async Task<List<TeacherResponse>> GetTeachersAsync()
{
    // Get the Teacher entity ID
    var entityResponse = await _httpClient.GetAsync("api/Entity");
    entityResponse.EnsureSuccessStatusCode();

    var entities = await entityResponse.Content.ReadFromJsonAsync<
        ApiResponse<List<EntityResponse>>>(
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

    var teacherEntity = entities?.Data?.FirstOrDefault(
        x => string.Equals(
            x.Name, "Teacher",
            StringComparison.OrdinalIgnoreCase));

    if (teacherEntity == null)
        throw new Exception("Teacher entity not found.");

    // Get all users
    var userResponse = await _httpClient.GetAsync("api/User");
    userResponse.EnsureSuccessStatusCode();

    var users = await userResponse.Content.ReadFromJsonAsync<
        ApiResponse<List<TeacherResponse>>>(
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

    if (users?.Data == null)
        throw new Exception("Unable to retrieve users.");

    // Return only teachers
    return users.Data
        .Where(x => x.UserTypeId == teacherEntity.Id)
        .OrderBy(x => x.FirstName)
        .ToList();
}

public async Task CreateSessionAsync(SessionCreateRequest request)
{
    var response = await _httpClient.PostAsJsonAsync(
        "api/Session", request);

    if (!response.IsSuccessStatusCode)
    {
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }
}


}