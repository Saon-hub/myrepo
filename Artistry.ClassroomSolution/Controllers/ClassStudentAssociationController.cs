
using Artistry.ClassroomSolution.DAL;
using Artistry.ClassroomSolution.Model;
using Artistry.ClassroomSolution.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Artistry.ClassroomSolution.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassStudentAssociationController : ControllerBase
{
    private readonly ClassStudentAssociationDAL _associationDAL;
    private readonly ILogger<ClassStudentAssociationController> _logger;

    public ClassStudentAssociationController(
        ClassStudentAssociationDAL associationDAL,
        ILogger<ClassStudentAssociationController> logger)
    {
        _associationDAL = associationDAL;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAssociation(
        [FromBody] ClassStudentAssociationRequest request)
    {
        try
        {
            if (request.SessionId <= 0 || request.StudentId <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid SessionId or StudentId."
                });
            }

            long associationId =
                await _associationDAL.CreateAssociation(request);

            return StatusCode(201, new
            {
                Success = true,
                Message = "Student associated with session successfully.",
                Data = new
                {
                    AssociationId = associationId,
                    SessionId = request.SessionId,
                    StudentId = request.StudentId
                }
            });
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            return Conflict(new
            {
                Success = false,
                Message = "Student is already associated with this session."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error associating student with session.");

            return StatusCode(500, new
            {
                Success = false,
                Message = "Unable to associate student with session."
            });
        }
    }

    [HttpGet("session/{sessionId:long}")]
    public async Task<IActionResult> GetStudentsBySessionId(long sessionId)
    {
        try
        {
            if (sessionId <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid SessionId."
                });
            }

            var students =
                await _associationDAL.GetStudentsBySessionId(sessionId);

            return Ok(new
            {
                Success = true,
                Message = students.Count > 0
                    ? "Students retrieved successfully."
                    : "No students associated with this session.",
                Data = students
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error retrieving students for SessionId {SessionId}",
                sessionId);

            return StatusCode(500, new
            {
                Success = false,
                Message = "An error occurred while retrieving students."
            });
        }
    }
    [HttpPut("attendance")]
    public async Task<IActionResult> UpdateAttendance(
    [FromBody] UpdateAttendanceRequest request)
    {
        try
        {
            if (request.AssociationId <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid AssociationId."
                });
            }

            bool exists = await _associationDAL.UpdateAttendance(request);

            if (!exists)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Student association not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Attendance updated successfully.",
                Data = new
                {
                    request.AssociationId,
                    request.Attendance
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error updating attendance for AssociationId {AssociationId}",
                request.AssociationId);

            return StatusCode(500, new
            {
                Success = false,
                Message = "An error occurred while updating attendance."
            });
        }
    }

    [HttpGet("available-students/{sessionId:long}")]
    public async Task<IActionResult> GetAvailableStudents(long sessionId)
    {
        try
        {
            if (sessionId <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid SessionId."
                });
            }

            var students =
                await _associationDAL.GetAvailableStudentsBySession(sessionId);

            return Ok(new
            {
                Success = true,
                Message = "Available students retrieved successfully.",
                Data = students
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error retrieving available students for SessionId {SessionId}",
                sessionId);

            return StatusCode(500, new
            {
                Success = false,
                Message = "Unable to retrieve available students."
            });
        }
    }
}
