using ArtistryApp.Model;
using System.Net.Http.Json;
using System.Text.Json;

namespace ArtClass.Mobile.Services;

public class ClassStudentService
{
    private readonly HttpClient _httpClient;

    public ClassStudentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ClassStudent>> GetStudentsBySessionAsync(
        int sessionId)
    {
        var response = await _httpClient.GetAsync(
            $"api/ClassStudentAssociation/session/{sessionId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<ClassStudentResponse>();

        if (result == null || !result.Success)
        {
            throw new Exception(
                result?.Message ?? "Unable to retrieve students.");
        }

        return result.Data ?? new List<ClassStudent>();
    }

    public async Task<bool> UpdateAttendanceAsync(
    int associationId,
    bool attendance)
    {
        var request = new
        {
            AssociationId = associationId,
            Attendance = attendance
        };

        var response = await _httpClient.PutAsJsonAsync(
            "api/ClassStudentAssociation/attendance",
            request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            throw new Exception(error);
        }

        return true;
    }

    public async Task<bool> AddStudentToSessionAsync(
    int sessionId,
    int studentId)
    {
        var request = new
        {
            SessionId = sessionId,
            StudentId = studentId
        };

        var response = await _httpClient.PostAsJsonAsync(
            "api/ClassStudentAssociation",
            request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            throw new Exception(error);
        }

        return true;
    }

    public async Task<List<AvailableStudent>>
    GetAvailableStudentsBySessionAsync(int sessionId)
    {
        var response = await _httpClient.GetAsync(
            $"api/ClassStudentAssociation/available-students/{sessionId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<AvailableStudent>>>(
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return result?.Data ?? new List<AvailableStudent>();
    }
    public async Task CreateStudentAsync(UserCreateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/User", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }

    public async Task<int> GetStudentEntityIdAsync()
    {
        var response = await _httpClient.GetAsync("api/Entity");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<
            ApiResponse<List<EntityResponse>>>(
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        var studentEntity = result?.Data?.FirstOrDefault(
            x => string.Equals(
                x.Name,
                "Student",
                StringComparison.OrdinalIgnoreCase));

        if (studentEntity == null)
            throw new Exception("Student entity not found.");

        return studentEntity.Id;
    }

}