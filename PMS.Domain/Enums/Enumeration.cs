using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Enums
{
    public static class Enumeration
    {
        public enum TaskStatus { NotStarted, InProgress, Completed, Overdue, Cancelled, OnHold }
        public enum PriorityLevel { Low, Medium, High, Urgent }
        public enum ProjectStatus { Inactive, OnGoing, Cancelled, Critical, Completed }
        public enum Privacy { Private, Public }
        public enum AppRole { Owner, Admin, Member }

    }
}
