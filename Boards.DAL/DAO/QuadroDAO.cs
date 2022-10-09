using Boards.DAL.DAO.Base;
using Boards.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boards.DAL.DAO
{
    public class QuadroDAO : BaseDAO<Quadro>
    {
        public new Quadro Get(int id)
        {
            return base.GetWithIncludes(Quadro.Includes).First(x => x.Id == id);
        }
        public new Quadro Get(string url)
        {
            return base.GetWithIncludes(Quadro.Includes).First(x => x.Url == url);
        }
        public List<Quadro> GetQuadrosUsuario(int idUsuario)
        {
            return base.GetWithIncludes(Quadro.Includes).Where(x => x.Id_Usuario == idUsuario).ToList();
        }
        public new void Add(Quadro quadro)
        {
            ConfiguracaoDAO configuracaoDAO = new ConfiguracaoDAO();
            var usuario = new UsuarioDAO().Get(quadro.Id_Usuario);
            if (usuario.IsGod || GetQuadrosUsuario(quadro.Id_Usuario).Count < configuracaoDAO.Get().QtdQuadros_Gratuitos)
            {
                quadro.Data_Criacao = DateTime.Now;
                string randomString = null;

                do
                {
                    var currentRandomString = GenerateRandomString();
                    if (base.Get().Where(quadro => quadro.Url == currentRandomString).Count() == 0)
                    {
                        randomString = currentRandomString;
                    }
                } while (string.IsNullOrEmpty(randomString));

                quadro.Cartoes = new List<Cartao>();
                quadro.Cartoes.Add(new Cartao() { BackgroundColor = Constants.FLAT_COLORS[new Random().Next(Constants.FLAT_COLORS.Length - 1)], Id_Usuario = quadro.Id_Usuario });

                quadro.Url = randomString;
                base.Add(quadro);
            }
            else
            {
                throw new InvalidOperationException($"Você só pode criar no máximo {configuracaoDAO.Get().QtdQuadros_Gratuitos} quadros");
            }
           
        }
        private string GenerateRandomString()
        {

            int length = 14;
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();
            char letter;
            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            return str_build.ToString();
        }
    }
}
