using Api.Contracts.Projects;
using Api.Data;
using Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Contracts.Common;
using System.Numerics;

namespace Api.Controllers;


[ApiController]
[Route("api/projects")]
public sealed class ProjectsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponse<ProjectResponse>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        if(page < 1)
            return Problem(title: "Invalid page", detail: "Page must be >= 1",statusCode: StatusCodes.Status400BadRequest);


        if (pageSize < 1 || pageSize > 100)
            return Problem(title: "Invalid pageSize", detail: "pageSize must be between 1 and 100", statusCode: StatusCodes.Status400BadRequest);

        var query = db.Projects.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt);

        var totalItems = await query.CountAsync(ct);
        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProjectResponse(x.Id, x.Name, x.Description, x.CreatedAt, x.UpdatedAt))
            .ToListAsync(ct);

        return Ok(new PagedResponse<ProjectResponse>(items, page, pageSize, totalItems, totalPages));
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