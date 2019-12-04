using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lab5.Models.DataAccess
{
    [ModelMetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
        [NotMapped]
        public List<Role> Roles
        {
            get
            {
                List<Role> roles = new List<Role>();
                using (StudentRecordContext context = new StudentRecordContext())
                {
                    roles = (from r in context.Role where context.EmployeeRole.Any(er => er.RoleId == r.Id && er.EmployeeId == this.Id) select r).ToList<Role>();
                }
                return roles;
            }
        }
    }
}
