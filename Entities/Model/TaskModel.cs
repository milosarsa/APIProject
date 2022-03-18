﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Entities.Model
{
    public class TaskModel : TaskBaseModel
    {

        [ReadOnly(true)]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string State { get; set; }
    }
}
