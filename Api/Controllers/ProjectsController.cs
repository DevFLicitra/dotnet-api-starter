using Api.Contracts.Projects;
using Api.Data;
using Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;


[ApiController]
[Route("api/projects")]
public sealed class ProjectsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProjectResponse>>> GetAll(CancellationToken ct)
    {
        var items = await db.Projects.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ProjectResponse(x.Id, x.Name, x.Description, x.CreatedAt, x.UpdatedAt))
            .ToListAsync(ct);

        return Ok(items);

    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectResponse>> GetById(Guid id ,CancellationToken ct)
    {
        var item = await db.Projects.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ProjectResponse(x.Id, x.Name, x.Description, x.CreatedAt, x.UpdatedAt))
            .FirstOrDefaultAsync(ct);

        if (item == null) return NotFound();
        return Ok(item);

    }


    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create(ProjectCreateRequest request, CancellationToken ct)
    {
        var entity = new Project
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,

        };

        db.Projects.Add(entity);
        await db.SaveChangesAsync(ct);

        var resp = new ProjectResponse(entity.Id, entity.Name, entity.Description, entity.CreatedAt, entity.UpdatedAt);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resp);
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id , ProjectUpdateRequest request, CancellationToken ct)
    {
        var entity = await db.Projects.FirstOrDefaultAsync(x => x.Id == id,ct);
        if (entity == null) return NotFound();

        entity.Name = request.Name.Trim();
        entity.Description = request.Description?.Trim();
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync(ct);
        return NoContent();

    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {

        var entity = await db.Projects.FirstOrDefaultAsync(x => x.Id == id,ct);
        if (entity == null) return NotFound();

        db.Projects.Remove(entity);
        await db.SaveChangesAsync(ct);
        return NoContent();
    }


}