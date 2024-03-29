﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Models.DataAccess
{
 
    public partial class Role
    {
        public Role()
        {
            EmployeeRole = new HashSet<EmployeeRole>();
        }

        public int Id { get; set; }
        public string Role1 { get; set; }

        public virtual ICollection<EmployeeRole> EmployeeRole { get; set; }
    }
}
