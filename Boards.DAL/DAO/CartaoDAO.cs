using Boards.DAL.DAO.Base;
using Boards.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boards.DAL.DAO
{
    public class CartaoDAO : BaseDAO<Cartao>
    {
        public new void Add(Cartao cartao) {
            QuadroDAO quadroDAO = new QuadroDAO();
            ConfiguracaoDAO configuracaoDAO = new ConfiguracaoDAO();
            var usuario = new UsuarioDAO().Get(quadroDAO.Get(cartao.Id_Quadro).Id_Usuario);
            if (usuario.IsGod || GetCartoesQuadro(cartao.Id_Quadro).Count < configuracaoDAO.Get().QtdCartoes_Gratuitos)
            {
                cartao.BackgroundColor = Constants.FLAT_COLORS[new Random().Next(Constants.FLAT_COLORS.Length - 1)];
                base.Add(cartao);
            }
            else
            {
                throw new OperationCanceledException($"Não é possível cadastrar mais de {configuracaoDAO.Get().QtdCartoes_Gratuitos} cartões");
            }
        }

        public List<Cartao> GetCartoesQuadro(int idQuadro)
        {
            return base.GetWithIncludes(Cartao.Includes).Where(x => x.Id_Quadro == idQuadro).ToList();
        }
    }
}
