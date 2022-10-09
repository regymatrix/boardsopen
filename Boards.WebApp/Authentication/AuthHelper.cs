using Boards.DAL.DAO;
using Boards.DTO;
using Boards.WebApp.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boards.WebApp.Authentication
{
    public class AuthHandler
    {
        private HttpContext HttpContext;
        public AuthHandler(HttpContext httpContext)
        {
            this.HttpContext = httpContext;
        }


        public void Login(string email)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            try
            {
                var usuario = usuarioDAO.Get(email);

                HttpContext.Session.Set("currentUserId", Encoding.UTF8.GetBytes(usuario.Id.ToString()));
                HttpContext.Session.Set("sessionToken", Encoding.UTF8.GetBytes(new Guid().ToString()));
            }
            catch (Exception ex)
            {
                throw new SystemException("Erro fatal", ex);
            }
        }


        public bool Login(string email, string senha)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            try
            {
                var usuario = usuarioDAO.Get(email);

                if (usuario.Senha == senha)
                {
                    HttpContext.Session.Set("currentUserId", Encoding.UTF8.GetBytes(usuario.Id.ToString()));
                    HttpContext.Session.Set("sessionToken", Encoding.UTF8.GetBytes(new Guid().ToString()));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new SystemException("Erro fatal", ex);
            }
        }

        public void LoginNovoUsuario(string idUsuario, string nomeUsuario, string emailUsuario, string senhaUsuario)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            try
            {
                var usuario = usuarioDAO.Get(emailUsuario);
                if (MD5.CreateMD5(usuario.Id.ToString()) == idUsuario)
                {
                    if (!string.IsNullOrEmpty(nomeUsuario))
                        usuario.Nome = nomeUsuario;

                    usuario.Senha = senhaUsuario;

                    usuarioDAO.Update(usuario);
                    HttpContext.Session.Set("currentUserId", Encoding.UTF8.GetBytes(usuario.Id.ToString()));
                    HttpContext.Session.Set("sessionToken", Encoding.UTF8.GetBytes(new Guid().ToString()));
                }
                else
                {
                    throw new UnauthorizedAccessException("Tentativa fraudulenta de atualizar conta de usuário");
                }
            }
            catch (Exception ex)
            {
                throw new SystemException("Erro fatal", ex);
            }

        }

        public void LoginAdministrador(string email, string senha)
        {
            try
            {
                AdministradorDAO usuarioDAO = new AdministradorDAO();
                var usuario = usuarioDAO.Get().First(x => x.Email == email && x.Senha == senha);

                HttpContext.Session.Set("currentUserId", Encoding.UTF8.GetBytes(usuario.Id.ToString()));
                HttpContext.Session.Set("sessionToken", Encoding.UTF8.GetBytes(new Guid().ToString()));
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Usuário e/ou Senha incorreto(s)", ex);
            }
            catch (Exception ex)
            {
                throw new SystemException("Erro fatal", ex);
            }

        }

        public Administrador GetAdministradorLogado()
        {
            try
            {
                AdministradorDAO administradorDAO = new AdministradorDAO();
                int idAdministradorLogado = Convert.ToInt32(Encoding.UTF8.GetString(HttpContext.Session.Get("currentUserId")));
                return administradorDAO.Get(idAdministradorLogado);
            }
            catch (Exception)
            {
                return new Administrador();
            }

        }

        public Usuario GetUsuarioLogado()
        {
            try
            {
                UsuarioDAO usuarioDAO = new UsuarioDAO();
                int idUsuarioLogado = Convert.ToInt32(Encoding.UTF8.GetString(HttpContext.Session.Get("currentUserId")));
                return usuarioDAO.Get(idUsuarioLogado);
            }
            catch (Exception)
            {
                return new Usuario();
            }

        }

        public bool IsUsuarioLogado()
        {
            return HttpContext.Session.Get("sessionToken") != null;
        }

        public void Logout()
        {
            HttpContext.Session.Remove("currentUserId");
            HttpContext.Session.Remove("sessionToken");
        }
    }
}
