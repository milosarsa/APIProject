using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entity
{
    public class ProjectEntity : Entity
    {
        public string Description { get; set; }
        public string Tasks { get; set; }
        public string Code { get; set; }

        public int State { get; set; }
    }

    
}
