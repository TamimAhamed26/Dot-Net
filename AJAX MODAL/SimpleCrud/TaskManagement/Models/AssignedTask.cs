using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models;

public class AssignedTask
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime SubmitDate { get; set; }
    public string Status { get; set; } = "Pending";
    [StringLength(50, ErrorMessage = "Remarks must be less than or equal to 50 characters.")]
    public string? Remarks { get; set; }
    public TaskList Task { get; set; } = null!;
    public User User { get; set; } = null!;


}
