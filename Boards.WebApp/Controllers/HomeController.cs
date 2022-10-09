using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Boards.WebApp.Models;
using Boards.WebApp.Controllers.Base;
using Microsoft.AspNetCore.Hosting;
using Boards.WebApp.Authentication;
using Microsoft.Extensions.Configuration;
using Boards.DAL.DAO;
using Boards.WebApp.Helpers;

namespace Boards.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ILogger<HomeController> logger) : base(webHostEnvironment, configuration)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["SucessoInicioReset"] = Request.Query["SucessoInicioReset"].ToString();
            return View();
        }


        public IActionResult Reset(string key, string email)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            if (MD5.CreateMD5($"resetar-{usuarioDAO.Get(email).Id}") == key)
            {
                var usuarioAtual = usuarioDAO.Get(email);
                usuarioAtual.Senha = null;
                usuarioDAO.Update(usuarioAtual);
                ViewData["SucessoInicioReset"] = true;
            }
            else
            {
                ViewData["SucessoInicioReset"] = false;
            }
            return RedirectToAction("Index", "Home", new { SucessoInicioReset = ViewData["SucessoInicioReset"]});
        }

        [Authorization]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorization]
        public IActionResult Logout()
        {
            AuthHandler authHandler = new AuthHandler(HttpContext);
            authHandler.Logout();
            _logger.LogInformation($"{GetUsuarioLogado().Email} fez logoff com sucesso.");
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
