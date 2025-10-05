using System.ComponentModel.DataAnnotations;

namespace AspireTodoApp.Web.Models.Forms;

public class TodoForm
{
    [Required][StringLength(100)] public string? Title { get; set; }
    [Required] public bool IsComplete { get; set; }
    [StringLength(3000)] public string? Description { get; set; }
}