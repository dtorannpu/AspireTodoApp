using System.ComponentModel.DataAnnotations;

namespace AspireTodoApp.Web.Models.Forms;

public class CreateTodoForm
{
    [Required] [StringLength(100)] public string? Name { get; set; }
    [Required] public bool IsComplete { get; set; }
}