using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Boards.DTO
{
    public class Quadro
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int Id_Usuario { get; set; }
        public string Nome { get; set; }
        public DateTime Data_Criacao { get; set; }
        public bool Is_Aberto { get; set; }

        [JsonIgnore]
        [ForeignKey("Id_Usuario")]
        public virtual Usuario Usuario { get; set; }

        public virtual List<Cartao> Cartoes { get; set; }


        [NotMapped]
        public static string[] Includes = new[] { "Usuario", "Cartoes", "Cartoes.Usuario" };

    }
}
