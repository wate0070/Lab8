using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Lab5.Models.DataAccess
{
    [ModelMetadataType(typeof(RoleMetaData))]
    public partial class Role
    {
    }
}
