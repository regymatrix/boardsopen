using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boards.DTO
{
    public class Configuracao
    {
        [Key]
        public int Id { get; set; }

        public int QtdCartoes_Gratuitos { get; set; }
        public int QtdQuadros_Gratuitos { get; set; }
    }
}
