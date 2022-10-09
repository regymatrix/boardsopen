using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Boards.DTO
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }
        public string Nome { get; set; }

        public string Tags { get; set; }

        public bool IsVIP { get; set; }
        public bool IsGod { get; set; }

        [JsonIgnore]
        public virtual List<Quadro> Quadros { get; set; }
        public string Senha { get; set; }

        [NotMapped]
        public static string[] Includes = new[] { "Quadros" };
    }
}
