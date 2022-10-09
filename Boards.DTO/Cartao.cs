using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Boards.DTO
{
    public class Cartao
    {
        [Key]
        public int Id { get; set; }
        public int Id_Usuario{ get; set; }
        public int Id_Quadro{ get; set; }
        public string Conteudo { get; set; }
        public string BackgroundColor { get; set; }

        [ForeignKey("Id_Usuario")]
        public virtual Usuario Usuario { get; set; }        
        
        [JsonIgnore]
        [ForeignKey("Id_Quadro")]
        public virtual Quadro Quadro { get; set; }


        [NotMapped]
        public static string[] Includes = new[] { "Usuario", "Quadro" };
    }
}
