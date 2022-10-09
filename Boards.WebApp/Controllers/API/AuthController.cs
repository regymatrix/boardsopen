using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boards.DAL.DAO;
using Boards.DTO;
using Boards.WebApp.Authentication;
using Boards.WebApp.Controllers.Base;
using Boards.WebApp.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Boards.WebApp.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : BaseController
    {
        public ILogger<AuthController> _logger { get; set; }
        public AuthController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ILogger<AuthController> logger) : base(webHostEnvironment, configuration)
        {
            this._logger = logger;
        }

        [HttpPost]
        public JsonResult AccountInfo(string email)
        {
            var jsonResult = new JsonResult(null);
            AuthHandler authHelper = new AuthHandler(HttpContext);
            var usuarioDAO = new UsuarioDAO();
            try
            {
                var usuario = usuarioDAO.Get(email);
                jsonResult.StatusCode = 200;
                var novaConta = usuario == null;

                if (novaConta)
                {

                    var novoUsuario = new Usuario();
                    novoUsuario.Email = email;
                    novoUsuario.IsGod = false;
                    novoUsuario.IsVIP = false;

                    usuarioDAO.Add(novoUsuario);
                    authHelper.Login(email);
                    usuario = authHelper.GetUsuarioLogado();
                }


                if (novaConta)
                {
                    jsonResult.Value = new
                    {
                        idUsuario = MD5.CreateMD5(usuario.Id.ToString()),
                        requerNome = true,
                        aindaNaoTemSenha = true,
                        requerSenha = false,
                    };
                }
                else
                {
                    jsonResult.Value = new
                    {
                        idUsuario = MD5.CreateMD5(usuario.Id.ToString()),
                        requerNome = string.IsNullOrEmpty(usuario.Nome),
                        aindaNaoTemSenha = string.IsNullOrEmpty(usuario.Senha),
                        requerSenha = !string.IsNullOrEmpty(usuario.Senha),
                    };
                }




                return jsonResult;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                jsonResult.Value = "Erro fatal.";
                jsonResult.StatusCode = 500;

                return jsonResult;
            }
        }

        [HttpPost]
        public async Task<JsonResult> ResetPassword(string email)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                var usuarioDAO = new UsuarioDAO();
                var usuario = usuarioDAO.Get(email);
                var chaveReset = MD5.CreateMD5($"resetar-{usuario.Id}");
                await EmailHelper.Send(usuario.Email, usuario.Nome, "Boards by Margi - Resete sua senha", "", $"Olá {usuario.Nome}, se você deseja recriar sua senha por favor clique no link abaixo: https://boards.margiinnovation.com/Home/Reset?key={chaveReset}&email={usuario.Email}");
                jsonResult.StatusCode = 200;
                jsonResult.Value = "success";

                _logger.LogInformation($"O sistema enviou um e-mail para {usuario.Email} com uma solicitação de reset de senha.");
                return jsonResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"O sistema falhou ao tentar enviar uma solicitação de reset de senha, {ex.Message}");
                jsonResult.StatusCode = 500;
                jsonResult.Value = "error";

                return jsonResult;
            }
        }

        [HttpPost]
        public JsonResult Login(string email, string senha)
        {
            var jsonResult = new JsonResult(null);
            AuthHandler authHelper = new AuthHandler(HttpContext);
            var usuarioDAO = new UsuarioDAO();
            try
            {
                var usuario = usuarioDAO.Get(email);
                if (usuario != null)
                {
                    if (authHelper.Login(email, senha))
                    {
                        _logger.LogInformation($"{email} fez login com sucesso.");

                        jsonResult.StatusCode = 200;
                        jsonResult.Value = "success";
                        return jsonResult;
                    }
                    else if (string.IsNullOrEmpty(usuario.Senha))
                    {
                        jsonResult.Value = new
                        {
                            idUsuario = MD5.CreateMD5(usuario.Id.ToString()),
                            requerNome = string.IsNullOrEmpty(usuario.Nome),
                            requerSenha = string.IsNullOrEmpty(usuario.Senha)
                        };
                        return jsonResult;
                    }
                    else
                    {
                        _logger.LogError($"{usuario.Email} tentou fazer login usando a senha errada.");

                        jsonResult.Value = "E-mail ou senha incorretos.";
                        jsonResult.StatusCode = 501;

                        return jsonResult;
                    }
                }
                else
                {
                    var novoUsuario = new Usuario();
                    novoUsuario.Email = email;
                    novoUsuario.IsGod = false;
                    novoUsuario.IsVIP = false;

                    usuarioDAO.Add(novoUsuario);

                    jsonResult.StatusCode = 200;
                    jsonResult.Value = new
                    {
                        idUsuario = MD5.CreateMD5(novoUsuario.Id.ToString()),
                        requerNome = string.IsNullOrEmpty(novoUsuario.Nome),
                        requerSenha = string.IsNullOrEmpty(novoUsuario.Senha)
                    };
                    return jsonResult;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                jsonResult.Value = "Erro fatal.";
                jsonResult.StatusCode = 500;

                return jsonResult;
            }
        }

        [HttpPost]
        public JsonResult LoginNovoUsuario(string idUsuario, string nomeUsuario, string emailUsuario, string senhaUsuario)
        {
            var jsonResult = new JsonResult(null);
            AuthHandler authHelper = new AuthHandler(HttpContext);
            var usuarioDAO = new UsuarioDAO();
            try
            {
                authHelper.LoginNovoUsuario(idUsuario, nomeUsuario, emailUsuario, senhaUsuario);
                _logger.LogInformation($"{authHelper.GetUsuarioLogado().Email} fez login com sucesso.");

                jsonResult.StatusCode = 200;
                jsonResult.Value = "success";
                return jsonResult;
            }
            catch (UnauthorizedAccessException exception)
            {
                _logger.LogError(exception.InnerException.Message, exception.InnerException);

                jsonResult.Value = exception.Message;
                jsonResult.StatusCode = 500;

                return jsonResult;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.InnerException.Message, exception.InnerException);

                jsonResult.Value = exception.Message;
                jsonResult.StatusCode = 500;

                return jsonResult;
            }
        }

        [HttpPost]
        public JsonResult LoginAdministrador(string email, string senha)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                AuthHandler authHelper = new AuthHandler(HttpContext);
                authHelper.LoginAdministrador(email, senha);
                _logger.LogInformation($"O administrador {email} fez login com sucesso.");

                jsonResult.StatusCode = 200;
                jsonResult.Value = "success";
                return jsonResult;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.InnerException.Message, exception.InnerException);

                jsonResult.Value = exception.Message;
                jsonResult.StatusCode = 500;

                return jsonResult;
            }
        }

    }
}
