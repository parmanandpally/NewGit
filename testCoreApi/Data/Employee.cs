using System;
using System.Collections.Generic;

namespace testCoreApi.Data
{
    public partial class Employee
    {
        public short EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public short? DepartmentId { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? JoinDate { get; set; }

        public virtual Department? Department { get; set; }
    }
}
