using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Model
{
    public class TaskUpdateModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [AllowNull]
        public string? Description { get; set; }
    }
}
