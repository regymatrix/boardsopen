using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boards.DAL.DAO;
using Boards.DTO;
using Boards.WebApp.Authentication;
using Boards.WebApp.Controllers.Base;
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
    public class UsuarioController : BaseController
    {
        public ILogger<QuadroController> _logger { get; set; }
        public UsuarioController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ILogger<QuadroController> logger) : base(webHostEnvironment, configuration)
        {
            this._logger = logger;
        }

       

        [HttpPost]
        [Authorization]
        public JsonResult AddUsuario(string nomeUsuario, string emailUsuario)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                UsuarioDAO usuarioDAO = new UsuarioDAO();

                var novoUsuario = new Usuario();
                novoUsuario.Email = emailUsuario;
                novoUsuario.Nome = nomeUsuario;
                novoUsuario.IsVIP = false;

                usuarioDAO.Add(novoUsuario);

                jsonResult.Value = 200;
                jsonResult.Value = "success";

                return jsonResult;
            }
            catch (InvalidOperationException exception)
            {
                _logger.LogError($"{emailUsuario} tentou criar uma nova conta.");

                AuthHandler authHandler = new AuthHandler(HttpContext);
                authHandler.Login(emailUsuario);

                jsonResult.Value = 200;
                jsonResult.Value = "success";

                return jsonResult;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                jsonResult.Value = "Erro! Por favor tente novamente mais tarde";
                jsonResult.StatusCode = 500;

                return jsonResult;
            }
        }


        [HttpGet]
        public JsonResult GetQuadro(int id)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                QuadroDAO quadroDAO = new QuadroDAO();

                jsonResult.Value = 200;
                jsonResult.Value = quadroDAO.Get(id);

                _logger.LogInformation($"O sistema recuperou com sucesso o quadro {id}");

                return jsonResult;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                jsonResult.Value = "Erro! Por favor tente novamente mais tarde";
                jsonResult.StatusCode = 500;

                return jsonResult;
            }
        }

        [Authorization]
        [HttpGet]
        public JsonResult GetQuadros()
        {
            var jsonResult = new JsonResult(null);
            try
            {
                QuadroDAO quadroDAO = new QuadroDAO();

                var usuarioLogado = GetUsuarioLogado();

                jsonResult.Value = 200;
                jsonResult.Value = quadroDAO.GetQuadrosUsuario(usuarioLogado.Id);
                _logger.LogInformation($"O sistema listou todos os quadros do usuário {usuarioLogado.Email}");

                return jsonResult;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                jsonResult.Value = "Erro! Por favor tente novamente mais tarde";
                jsonResult.StatusCode = 500;

                return jsonResult;
            }
        }

        [Authorization]
        [HttpPost]
        public JsonResult AddQuadro(string quadroNome)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                QuadroDAO quadroDAO = new QuadroDAO();

                var usuarioLogado = GetUsuarioLogado();
                var quadro = new Quadro
                {
                    Id_Usuario = usuarioLogado.Id,
                    Is_Aberto = false,
                    Nome = quadroNome
                };

                quadroDAO.Add(quadro);

                _logger.LogInformation($"{usuarioLogado.Email} criou um quadro com o nome {quadroNome}");
                jsonResult.StatusCode = 200;
                jsonResult.Value = quadro.Url;

                return jsonResult;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                jsonResult.Value = "Erro! Por favor tente novamente mais tarde";
                jsonResult.StatusCode = 500;

                return jsonResult;
            }
        }

        [Authorization]
        [HttpPost]
        public JsonResult DeleteQuadro(int idQuadro)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                QuadroDAO quadroDAO = new QuadroDAO();

                var usuarioLogado = GetUsuarioLogado();
                var quadroAtual = quadroDAO.Get(idQuadro);

                quadroDAO.Delete(idQuadro);

                _logger.LogInformation($"{usuarioLogado.Email} apagou um quadro com o nome {quadroAtual.Nome} e ID: {quadroAtual.Id}");
                jsonResult.StatusCode = 200;
                jsonResult.Value = "success";

                return jsonResult;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                jsonResult.Value = "Erro! Por favor tente novamente mais tarde";
                jsonResult.StatusCode = 500;

                return jsonResult;
            }
        }


        [HttpPost]
        [AdminAuthorization]
        public JsonResult Delete(Usuario usuario)
        {
            var jsonResult = new JsonResult(null);
            var usuarioDao = new UsuarioDAO();
            try
            {
                usuarioDao.Delete(usuario);
                _logger.LogInformation($"Removeu o usuário de ID: {usuario.Id} e nome {usuario.Nome}");

                jsonResult.StatusCode = 200;
                jsonResult.Value = "success";
                return jsonResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Falhou ao remover o usuário de ID: {usuario.Id} e nome {usuario.Nome}");
                _logger.LogError(ex.Message);

                jsonResult.StatusCode = 500;
                jsonResult.Value = "error";
                return jsonResult;
            }
        }

        [HttpPost]
        [AdminAuthorization]
        public JsonResult Update(Usuario usuario)
        {
            var jsonResult = new JsonResult(null);
            var usuarioDao = new UsuarioDAO();
            try
            {
                usuarioDao.Update(usuario);
                _logger.LogInformation($"Atualizou informações do usuário de ID: {usuario.Id}");

                jsonResult.StatusCode = 200;
                jsonResult.Value = "success";
                return jsonResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Falhou ao atualizar o usuário de ID: {usuario.Id}");
                _logger.LogError(ex.Message);

                jsonResult.StatusCode = 500;
                jsonResult.Value = "error";
                return jsonResult;
            }
        }

        [HttpGet]
        [AdminAuthorization]
        public JsonResult Get(string pesquisa, int qtd)
        {
            if (!string.IsNullOrEmpty(pesquisa))
                return new JsonResult(new UsuarioDAO().Get()
              .Where(x =>
                     x.Nome.ToLower().Contains(pesquisa.ToLower()) ||
                     x.Email.ToLower().Contains(pesquisa.ToLower()) ||
                     x.Tags.ToLower().Contains(pesquisa.ToLower()))
                     .Take(qtd));
            else
                return new JsonResult(new UsuarioDAO().Get().Take(qtd));
        }
    }
}

