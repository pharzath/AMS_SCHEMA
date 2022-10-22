﻿using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class StagingTaskGroup
    {
        public StagingTaskGroup()
        {
            StagingTaskGroupTasks = new HashSet<StagingTaskGroupTask>();
            StagingTaskGroupUsers = new HashSet<StagingTaskGroupUser>();
        }

        public int TaskGroupId { get; set; }
        public string TaskGroupCodeName { get; set; } = null!;
        public Guid TaskGroupGuid { get; set; }
        public string? TaskGroupDescription { get; set; }

        public virtual ICollection<StagingTaskGroupTask> StagingTaskGroupTasks { get; set; }
        public virtual ICollection<StagingTaskGroupUser> StagingTaskGroupUsers { get; set; }
    }
}
