using Artistry.ClassroomSolution.Dto;
using Artistry.ClassroomSolution.Models;
using MySqlConnector;
using System.Data;

namespace Artistry.ClassroomSolution.DAL;

public class SessionDAL
{
    private readonly DbConnectionFactory _connectionFactory;

    public SessionDAL(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<SessionCalendarDto>> GetCalendarData()
    {
        var sessions = new List<SessionCalendarDto>();

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();    
     

        //await connection.OpenAsync();

        await using var command =
            new MySqlCommand("get_calendar_data", connection);

        command.CommandType = CommandType.StoredProcedure;

        await using var reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            sessions.Add(new SessionCalendarDto
            {
                Id = reader.GetInt64(
                    reader.GetOrdinal("Id")),

                ClassName = reader.GetString(
                    reader.GetOrdinal("ClassName")),

                Description = reader.IsDBNull(
                    reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Description")),

                TeacherId = reader.GetInt64(
                    reader.GetOrdinal("TeacherId")),

                TeacherName = reader.GetString(
                    reader.GetOrdinal("TeacherName")),

                Date = DateOnly.FromDateTime(
                    reader.GetDateTime(
                        reader.GetOrdinal("Date"))),

                StartTime = reader.GetString(
                    reader.GetOrdinal("StartTime")),

                EndTime = reader.GetString(
                    reader.GetOrdinal("EndTime"))
            });
        }

        return sessions;
    }


    public async Task<long> CreateSession(
        SessionCreateRequest request)
    {
        await using var connection =
            await _connectionFactory.CreateConnectionAsync();


        //await connection.OpenAsync();

        await using var command =
            new MySqlCommand("create_session", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue(
            "p_Name", request.Name);

        command.Parameters.AddWithValue(
            "p_Description",
            (object?)request.Description ?? DBNull.Value);

        command.Parameters.AddWithValue(
            "p_TeacherId", request.TeacherId);

        command.Parameters.Add(
            "p_Date", MySqlDbType.Date
        ).Value = DateTime.ParseExact(
            request.Date,
            "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture);

        command.Parameters.Add(
            "p_StartTime", MySqlDbType.Time
        ).Value = TimeSpan.Parse(request.StartTime);

        command.Parameters.Add(
            "p_EndTime", MySqlDbType.Time
        ).Value = TimeSpan.Parse(request.EndTime);

        command.Parameters.AddWithValue(
            "p_CreatedBy", request.CreatedBy);

        await using var reader =
            await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return reader.GetInt64(
                reader.GetOrdinal("SessionId"));
        }

        throw new Exception(
            "Session creation failed. No SessionId returned.");
    }

}