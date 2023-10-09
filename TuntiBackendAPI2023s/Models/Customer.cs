using System;
using System.Collections.Generic;

namespace TuntiBackendAPI2023s.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Timesheets = new HashSet<Timesheet>();
            WorkAssignments = new HashSet<WorkAssignment>();
        }

        public int IdCustomer { get; set; }
        public string CustomerName { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Timesheet> Timesheets { get; set; }
        public virtual ICollection<WorkAssignment> WorkAssignments { get; set; }
    }
}
