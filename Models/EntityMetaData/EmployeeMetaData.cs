using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lab5.Models.DataAccess
{
    public class EmployeeMetadata
    {
        [Display(Name = "Employee ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee Name is Required.")]
        [RegularExpression(@"[a-zA-Z]+\s+[a-zA-Z]+", 
            ErrorMessage = "Must be first Name followed by Last Name.")]
        [Display(Name = "Employee Name")]
        public string Name { get; set;}

        [Required(ErrorMessage = "Network ID is Required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage ="Must be more than 3 characters.")]
        [Display(Name = "Network ID")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Password must be more than 5 characters.")]
        
        public string Password { get; set; }

    } }
