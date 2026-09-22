using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Models;

public enum TaskStatus
{
    Pending,
    InProgress,
    Done
}

public enum TaskPriority
{
    Low,
    Medium,
    High
}

public class TaskItem
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Relacionamento com o dono da tarefa
    public int UserId { get; set; }
    public User? User { get; set; }
}
