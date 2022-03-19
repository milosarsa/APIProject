using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public static class Enums
    {
        public static string GetName(this Enum @enum) 
        {
            Type type = @enum.GetType();

            return type.GetEnumName(@enum) ?? String.Empty;
        }

        
    }

    
}