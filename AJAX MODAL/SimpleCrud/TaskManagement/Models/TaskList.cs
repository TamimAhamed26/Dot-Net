using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models;

public class TaskList
{
    public int Id { get; set; }
    [StringLength(50, ErrorMessage = "Title must be less than or equal to 50 characters.")]
    public string Title { get; set; }
    [StringLength(200, ErrorMessage = "Description must be less than or equal to 200 characters.")]
    public string? Description { get; set; }

    
    

}
