using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Model
{
    public class TaskBaseModel
    {

        [Required(ErrorMessage = "Project Id is required")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [AllowNull]
        public string? Description { get; set; }
    }
}
