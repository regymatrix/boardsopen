using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Boards.DAL;
using Boards.DAL.DAO;
using Boards.DTO;
using Boards.WebApp.Authentication;
using Boards.WebApp.Controllers.Base;
using Boards.WebApp.SignalR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Boards.WebApp.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuadroController : BaseController
    {
        private readonly IHubContext<StreamingHub> _streaming;

        public ILogger<QuadroController> _logger { get; set; }
        public QuadroController(IHubContext<StreamingHub> streaming, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ILogger<QuadroController> logger) : base(webHostEnvironment, configuration)
        {
            this._streaming = streaming;
            this._logger = logger;
        }

        [HttpPost]
        public async Task<JsonResult> AddUsuarioToBoard(string nomeUsuario, string emailUsuario, int idQuadro)
        {
            var jsonResult = new JsonResult(null);
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            CartaoDAO cartaoDAO = new CartaoDAO();
            QuadroDAO quadroDAO = new QuadroDAO();

            emailUsuario = emailUsuario.ToLower();

            try
            {
                var novoUsuario = new Usuario();
                novoUsuario.Email = emailUsuario;
                novoUsuario.Nome = nomeUsuario;
                novoUsuario.IsVIP = false;

                usuarioDAO.Add(novoUsuario);

                var novoCartao = new Cartao();
                novoCartao.Id_Usuario = novoUsuario.Id;
                novoCartao.Id_Quadro = idQuadro;

                cartaoDAO.Add(novoCartao);
                await UpdateCartoesQuadroStream(quadroDAO.Get(idQuadro));

                AuthHandler authHandler = new AuthHandler(HttpContext);
                authHandler.Login(emailUsuario);

                jsonResult.Value = 200;
                jsonResult.Value = "success";

                return jsonResult;
            }
            catch (InvalidOperationException exception)
            {
                try
                {
                    AuthHandler authHandler = new AuthHandler(HttpContext);
                    authHandler.Login(emailUsuario);

                    var quadroAtual = quadroDAO.Get(idQuadro);
                    if (quadroAtual.Cartoes.Where(x => x.Id_Usuario == GetUsuarioLogado().Id).Count() == 0)
                    {
                        var novoCartao = new Cartao();
                        novoCartao.Id_Usuario = GetUsuarioLogado().Id;
                        novoCartao.Id_Quadro = idQuadro;
                        cartaoDAO.Add(novoCartao);
                        await UpdateCartoesQuadroStream(quadroDAO.Get(idQuadro));
                    }

                    jsonResult.Value = 200;
                    jsonResult.Value = "success";

                    return jsonResult;
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogError(ex.Message);

                    jsonResult.Value = "O quadro atingiu o limite de cartões";
                    jsonResult.StatusCode = 500;

                    return jsonResult;
                }
            }
            catch (FormatException exception)
            {
                _logger.LogError(exception.Message);

                jsonResult.Value = exception.Message;
                jsonResult.StatusCode = 500;

                return jsonResult;
            }
            catch (OperationCanceledException exception)
            {
                _logger.LogError(exception.Message);

                jsonResult.Value = "O quadro atingiu o limite de cartões";
                jsonResult.StatusCode = 500;

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
        public JsonResult GetQuantidadeCartoes(int idQuadro)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                QuadroDAO quadroDAO = new QuadroDAO();

                var quadroAtual = quadroDAO.Get(idQuadro);

                jsonResult.Value = 200;
                jsonResult.Value = quadroAtual.Cartoes.Count();

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

        private async Task UpdateCartaoStream(Cartao cartao)
        {
            await _streaming.Clients.All.SendAsync($"ReadCartoesUpdate-{cartao.Id_Quadro}", new JsonResult(cartao));
        }

        private async Task UpdateCartoesQuadroStream(Quadro quadro)
        {
            await _streaming.Clients.All.SendAsync($"ReadCartoesQuadroUpdate-{quadro.Id}", new JsonResult(quadro));
        }


        [HttpPost]
        public JsonResult RemoverUsuario(int idQuadro, int idUsuario)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                CartaoDAO cartaoDAO = new CartaoDAO();

                var cartaoAtual = cartaoDAO.Get().FirstOrDefault(x => x.Id_Quadro == idQuadro && x.Id_Usuario == idUsuario);

                cartaoDAO.Delete(cartaoAtual);

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
        public JsonResult GetConteudoCartao(int idCartao)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                CartaoDAO cartaoDAO = new CartaoDAO();

                var cartaoAtual = cartaoDAO.Get(idCartao);

                jsonResult.Value = 200;
                jsonResult.Value = cartaoAtual.Conteudo;

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
        [Authorization]
        public async Task<JsonResult> UpdateCartao(dynamic data)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                CartaoDAO cartaoDAO = new CartaoDAO();

                var cartaoAtual = cartaoDAO.Get((int)data.idCartao);
                cartaoAtual.Conteudo = data.conteudo;

                cartaoDAO.Update(cartaoAtual);
                await UpdateCartaoStream(cartaoAtual);

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
        [HttpPost]
        public async Task<JsonResult> ParticiparQuadro(int idQuadro)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                QuadroDAO quadroDAO = new QuadroDAO();
                CartaoDAO cartaoDAO = new CartaoDAO();
                var novoCartao = new Cartao();
                novoCartao.Id_Usuario = GetUsuarioLogado().Id;
                novoCartao.Id_Quadro = idQuadro;

                cartaoDAO.Add(novoCartao);
                await UpdateCartoesQuadroStream(quadroDAO.Get(novoCartao.Id_Quadro));


                jsonResult.Value = 200;
                jsonResult.Value = "success";
                _logger.LogInformation($"{GetUsuarioLogado().Email} entrou no quadro de ID: {idQuadro}");

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
        public JsonResult ToggleAberturaQuadro(int idQuadro)
        {
            var jsonResult = new JsonResult(null);
            try
            {
                QuadroDAO quadroDAO = new QuadroDAO();
                var quadroAtual = quadroDAO.Get(idQuadro);
                if (GetUsuarioLogado().Id == quadroAtual.Id_Usuario)
                {
                    quadroAtual.Is_Aberto = quadroAtual.Is_Aberto == true ? false : true;

                    quadroDAO.Update(quadroAtual);

                    jsonResult.Value = 200;
                    jsonResult.Value = "success";
                    _logger.LogInformation($"{GetUsuarioLogado().Email} mudou o estado de abertura do quadro {quadroAtual.Id} - {quadroAtual.Is_Aberto}");

                    return jsonResult;
                }
                else
                {
                    _logger.LogInformation($"{GetUsuarioLogado().Email} tentou mudar o estado de abertuda de um quadro que não o pertence! {quadroAtual.Id} - {quadroAtual.Is_Aberto}");
                    throw new UnauthorizedAccessException("Você não poussi permissão para realizar essa operação");
                }
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
                jsonResult.Value = quadroDAO.GetQuadrosUsuario(usuarioLogado.Id).OrderByDescending(x => x.Data_Criacao).ToList();
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
            catch (InvalidOperationException exception)
            {
                ConfiguracaoDAO configuracaoDAO = new ConfiguracaoDAO();
                _logger.LogError($"{GetUsuarioLogado().Email} tentou criar mais de {configuracaoDAO.Get().QtdQuadros_Gratuitos} quadros");

                jsonResult.Value = exception.Message;
                jsonResult.StatusCode = 500;

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

    }
}
