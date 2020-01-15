using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Models
{
    public class Management
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatorId { get; set; }

        public int RoleId { get; set; }
        public virtual ManagementRole ManagementRole { get; set; }

    }
}
