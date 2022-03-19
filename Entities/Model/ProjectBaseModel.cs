using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace Entities.Model
{
    public class ProjectBaseModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [AllowNull]
        public string? Description { get; set; }
    }
}
