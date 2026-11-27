
using Artistry.ClassroomSolution.Dto;
using Artistry.ClassroomSolution.Model;
using Artistry.ClassroomSolution.Models;
using MySqlConnector;
using System.Data;

namespace Artistry.ClassroomSolution.DAL;

public class ClassStudentAssociationDAL
{
    private readonly DbConnectionFactory _connectionFactory;

    public ClassStudentAssociationDAL(
        DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> CreateAssociation(
        ClassStudentAssociationRequest request)
    {
        await using var connection =
            await _connectionFactory.CreateConnectionAsync();

        //await connection.OpenAsync();

        await using var command =
            new MySqlCommand(
                "create_class_student_association",
                connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue(
            "p_SessionId", request.SessionId);

        command.Parameters.AddWithValue(
            "p_StudentId", request.StudentId);

        await using var reader =
            await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return reader.GetInt64(
                reader.GetOrdinal("AssociationId"));
        }

        throw new Exception(
            "Student association creation failed.");
    }

    public async Task<List<SessionStudentDto>> GetStudentsBySessionId(long sessionId)
    {
        var students = new List<SessionStudentDto>();

        await using var connection = await _connectionFactory.CreateConnectionAsync();
        //await connection.OpenAsync();

        await using var command = new MySqlCommand(
            "get_students_by_session_id", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("p_SessionId", sessionId);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            students.Add(new SessionStudentDto
            {
                AssociationId = reader.GetInt64("AssociationId"),
                SessionId = reader.GetInt64("SessionId"),
                StudentId = reader.GetInt64("StudentId"),
                StudentName = reader.GetString("StudentName"),
                Attendance = reader.GetBoolean("Attendance"),
                DateOfAttendance = reader.IsDBNull(
                    reader.GetOrdinal("DateOfAttendance"))
                    ? null
                    : DateOnly.FromDateTime(
                        reader.GetDateTime("DateOfAttendance"))
            });
        }

        return students;
    }

    public async Task<bool> UpdateAttendance(UpdateAttendanceRequest request)
    {
        await using var connection = await _connectionFactory.CreateConnectionAsync();
        //await connection.OpenAsync();

        await using var command = new MySqlCommand(
            "update_student_attendance", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue(
            "p_AssociationId", request.AssociationId);

        command.Parameters.AddWithValue(
            "p_Attendance", request.Attendance);

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result) == 1;
    }

    public async Task<List<AvailableStudentDto>>
    GetAvailableStudentsBySession(long sessionId)
    {
        var students = new List<AvailableStudentDto>();

        await using var connection = await _connectionFactory.CreateConnectionAsync();
        

        await using var command = new MySqlCommand(
            "get_available_students_by_session", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("p_SessionId", sessionId);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            students.Add(new AvailableStudentDto
            {
                StudentId = reader.GetInt64("StudentId"),
                StudentName = reader.GetString("StudentName")
            });
        }

        return students;
    }
}
