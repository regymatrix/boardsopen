using Boards.DAL.DAO.Base;
using Boards.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boards.DAL.DAO
{
    public class ConfiguracaoDAO : BaseDAO<Configuracao>
    {
        public new Configuracao Get() {
            var configuracao = base.Get().FirstOrDefault();
            if (configuracao == null)
            {
                configuracao = new Configuracao();
                configuracao.QtdQuadros_Gratuitos = Constants.MaxQuadrosUsuario;
                configuracao.QtdCartoes_Gratuitos = Constants.MaxQtdCartoes;
            }
            return configuracao;
        }
        public new void Add(Configuracao configuracao) {
            if (base.Get().Count() > 0)
            {
                configuracao.Id = 1;
                base.Update(configuracao);
            }
            else
            {
                base.Add(configuracao);
            }
        }

    }
}
