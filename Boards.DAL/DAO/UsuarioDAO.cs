using Boards.DAL.DAO.Base;
using Boards.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Boards.DAL.DAO
{
    public class UsuarioDAO : BaseDAO<Usuario>
    {
        public new void Add(Usuario usuario)
        {
            usuario.Email = usuario.Email.ToLower();
         
            if (string.IsNullOrEmpty(usuario.Tags))
                usuario.Tags = "";

            if (string.IsNullOrEmpty(usuario.Nome))
                usuario.Nome = "";

            if (!IsEmailValid(usuario.Email))
                throw new FormatException("E-mail inválido!");

            if (base.Get().FirstOrDefault(x => x.Email.ToLower() == usuario.Email) == null)
                base.Add(usuario);
            else
                throw new InvalidOperationException("Já existe um usuário com o e-mail informado!");
        }
        public new Usuario Get(string email)
        {
            return base.Get().FirstOrDefault(usuario => usuario.Email.ToLower() == email.ToLower());
        }

        private bool IsEmailValid(string emailaddress)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emailaddress);
            return match.Success;
        }
    }
}
