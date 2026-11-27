
using Artistry.ClassroomSolution.DAL;
using Artistry.ClassroomSolution.Model;
using Microsoft.AspNetCore.Mvc;

namespace Artistry.ClassroomSolution.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserDAL _dal;

    public UserController(UserDAL dal)
    {
        _dal = dal;
    }

    // POST: api/User
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        try
        {
            var id = await _dal.CreateAsync(user);

            return CreatedAtAction(
                nameof(SelectAll),
                new { id },
                new
                {
                    Success = true,
                    Message = "User created successfully.",
                    Id = id
                });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Success = false,
                Message = "Error creating user.",
                Error = ex.Message
            });
        }
    }

    // PUT: api/User
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] User user)
    {
        try
        {
            var updated = await _dal.UpdateAsync(user);

            if (!updated)
                return NotFound(new
                {
                    Success = false,
                    Message = "User not found."
                });

            return Ok(new
            {
                Success = true,
                Message = "User updated successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Success = false,
                Message = "Error updating user.",
                Error = ex.Message
            });
        }
    }

    // DELETE: api/User/1
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var deleted = await _dal.DeleteAsync(id);

            if (!deleted)
                return NotFound(new
                {
                    Success = false,
                    Message = "User not found."
                });

            return Ok(new
            {
                Success = true,
                Message = "User deleted successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Success = false,
                Message = "Error deleting user.",
                Error = ex.Message
            });
        }
    }

    // GET: api/User
    [HttpGet]
    public async Task<IActionResult> SelectAll()
    {
        try
        {
            var users = await _dal.SelectAllAsync();

            return Ok(new
            {
                Success = true,
                Data = users
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Success = false,
                Message = "Error retrieving users.",
                Error = ex.Message
            });
        }
    }
}
