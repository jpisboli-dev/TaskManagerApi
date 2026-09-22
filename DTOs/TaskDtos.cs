using System.ComponentModel.DataAnnotations;
using TaskManagerApi.Models;

namespace TaskManagerApi.DTOs;

public class CreateTaskDto
{
    [Required, MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateTime? DueDate { get; set; }
}

public class UpdateTaskDto
{
    [Required, MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public TaskManagerApi.Models.TaskStatus Status { get; set; }

    public TaskPriority Priority { get; set; }

    public DateTime? DueDate { get; set; }
}

public class TaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
