using Artistry.ClassroomSolution.DAL;
using Artistry.ClassroomSolution.Models;
using Microsoft.AspNetCore.Mvc;

namespace Artistry.ClassroomSolution.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private readonly SessionDAL _sessionDAL;
    private readonly ILogger<SessionController> _logger;

    public SessionController(
        SessionDAL sessionDAL,
        ILogger<SessionController> logger)
    {
        _sessionDAL = sessionDAL;
        _logger = logger;
    }

    [HttpGet("calendar")]
    public async Task<IActionResult> GetCalendarData()
    {
        try
        {
            var sessions = await _sessionDAL.GetCalendarData();

            return Ok(new
            {
                Success = true,
                Message = "Calendar data retrieved successfully.",
                Data = sessions
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving calendar data.");

            return StatusCode(500, new
            {
                Success = false,
                Message = "An error occurred while retrieving calendar data."
            });
        }
    }


    [HttpPost]
    public async Task<IActionResult> CreateSession(
        [FromBody] SessionCreateRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Session name is required."
                });
            }

            if (request.TeacherId <= 0 || request.CreatedBy <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid TeacherId or CreatedBy."
                });
            }

            if (!DateOnly.TryParseExact(
                request.Date,
                "yyyy-MM-dd",
                out var sessionDate))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid date. Expected yyyy-MM-dd."
                });
            }

            if (!TimeOnly.TryParseExact(
                request.StartTime,
                "HH:mm",
                out var startTime))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid StartTime. Expected HH:mm."
                });
            }

            if (!TimeOnly.TryParseExact(
                request.EndTime,
                "HH:mm",
                out var endTime))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid EndTime. Expected HH:mm."
                });
            }

            if (endTime <= startTime)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "EndTime must be greater than StartTime."
                });
            }

            request.Date = sessionDate.ToString("yyyy-MM-dd");
            request.StartTime = startTime.ToString("HH:mm:ss");
            request.EndTime = endTime.ToString("HH:mm:ss");

            long sessionId =
                await _sessionDAL.CreateSession(request);

            return StatusCode(201, new
            {
                Success = true,
                Message = "Session created successfully.",
                Data = new
                {
                    SessionId = sessionId
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating session.");

            return StatusCode(500, new
            {
                Success = false,
                Message = "Unable to create session."
            });
        }
    }

}