using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enums
{
    public enum ProjectState
    {
        Open,
        Closed,
        Archived
    }

    public enum TaskState
    {
        Pending,
        Active,
        Paused,
        Completed,
        Abandoned,
        Archived
    }

    public enum UserRoles
    {
        Admin,
        Manager,
        User,
        Guest,
        Unauthorized //Not used
    }

    
}