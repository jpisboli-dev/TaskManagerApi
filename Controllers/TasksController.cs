using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

    // GET api/tasks?status=Pending&priority=High
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll(
        [FromQuery] TaskManagerApi.Models.TaskStatus? status,
        [FromQuery] TaskPriority? priority)
    {
        var query = _context.Tasks.Where(t => t.UserId == CurrentUserId);

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => ToDto(t))
            .ToListAsync();

        return Ok(tasks);
    }

    // GET api/tasks/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(int id)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == CurrentUserId);

        if (task is null) return NotFound();

        return Ok(ToDto(task));
    }

    // POST api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            UserId = CurrentUserId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, ToDto(task));
    }

    // PUT api/tasks/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == CurrentUserId);

        if (task is null) return NotFound();

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(ToDto(task));
    }

    // PATCH api/tasks/5/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] TaskManagerApi.Models.TaskStatus status)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == CurrentUserId);

        if (task is null) return NotFound();

        task.Status = status;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(ToDto(task));
    }

    // DELETE api/tasks/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == CurrentUserId);

        if (task is null) return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static TaskResponseDto ToDto(TaskItem t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        Status = t.Status.ToString(),
        Priority = t.Priority.ToString(),
        DueDate = t.DueDate,
        CreatedAt = t.CreatedAt,
        UpdatedAt = t.UpdatedAt
    };
}
