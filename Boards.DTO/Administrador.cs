using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boards.DTO
{
    public class Administrador
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }

    }
}
