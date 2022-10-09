using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Boards.DTO
{
    public class Lead
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public bool VirouCliente { get; set; }
    }
}
