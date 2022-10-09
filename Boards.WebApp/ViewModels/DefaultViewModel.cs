using Boards.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boards.WebApp.ViewModels
{
    public class DefaultViewModel
    {
        public bool IsLogado { get; set; }

        public Usuario UsuarioLogado { get; set; }

        public bool IsProduction { get; set; }
        public string Versao { get; set; }
    }
}
