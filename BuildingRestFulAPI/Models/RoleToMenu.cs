using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Models
{
    public class RoleToMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int RoleToMenuId { get; set; }

        public int MenuId { get; set; }

        public virtual Menu Menu { get; set; }

        public int RoleId { get; set; }

        public  virtual ManagementRole ManagementRole { get; set; }


    }
}
