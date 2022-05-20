using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Model
{
    public class ProjectUpdateModel
    {
        [AllowNull]
        public string? Description { get; set; }

        [AllowNull]
        public string Code { get; set; }
    }
}
