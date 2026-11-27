
using Artistry.ClassroomSolution.DAL;
using Artistry.ClassroomSolution.Model;
using Microsoft.AspNetCore.Mvc;

namespace Artistry.ClassroomSolution.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntityController : ControllerBase
{
    private readonly EntityDAL _dal;

    public EntityController(EntityDAL dal)
    {
        _dal = dal;
    }

    // POST: api/Entity
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Entity entity)
    {
        try
        {
            var id = await _dal.CreateAsync(entity);

            return CreatedAtAction(
                nameof(SelectAll),
                new { id },
                new
                {
                    Success = true,
                    Message = "Entity created successfully.",
                    Id = id
                });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Success = false,
                Message = "Error creating entity.",
                Error = ex.Message
            });
        }
    }

    // PUT: api/Entity
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Entity entity)
    {
        try
        {
            var updated = await _dal.UpdateAsync(entity);

            if (!updated)
                return NotFound(new
                {
                    Success = false,
                    Message = "Entity not found."
                });

            return Ok(new
            {
                Success = true,
                Message = "Entity updated successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Success = false,
                Message = "Error updating entity.",
                Error = ex.Message
            });
        }
    }

    // DELETE: api/Entity/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _dal.DeleteAsync(id);

            if (!deleted)
                return NotFound(new
                {
                    Success = false,
                    Message = "Entity not found."
                });

            return Ok(new
            {
                Success = true,
                Message = "Entity deleted successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Success = false,
                Message = "Error deleting entity.",
                Error = ex.Message
            });
        }
    }

    // GET: api/Entity
    [HttpGet]
    public async Task<IActionResult> SelectAll()
    {
        try
        {
            var entities = await _dal.SelectAllAsync();

            return Ok(new
            {
                Success = true,
                Data = entities
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Success = false,
                Message = "Error retrieving entities.",
                Error = ex.Message
            });
        }
    }
}
