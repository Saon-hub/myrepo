using Artistry.ClassroomSolution.DAL;
using Artistry.ClassroomSolution.Model;
using Microsoft.AspNetCore.Mvc;

namespace Artistry.ClassroomSolution.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntityTypeController : ControllerBase
{
    private readonly EntityTypeDAL _dal;

    public EntityTypeController(EntityTypeDAL dal)
    {
        _dal = dal;
    }

    [HttpPost]
    public async Task<IActionResult> Create(EntityType entityType)
    {
        var id = await _dal.CreateAsync(entityType);
        return Ok(new { Id = id });
    }

    [HttpPut]
    public async Task<IActionResult> Update(EntityType entityType)
    {
        var updated = await _dal.UpdateAsync(entityType);
        return updated ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _dal.DeleteAsync(id);
        return deleted ? Ok() : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> SelectAll()
    {
        var result = await _dal.SelectAllAsync();
        return Ok(result);
    }
}