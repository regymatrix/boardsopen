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
using Boards.DAL.DAO;
using Microsoft.Extensions.Configuration;

namespace Boards.WebApp.Controllers
{
   
    public class QuadroController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public QuadroController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ILogger<HomeController> logger) : base(webHostEnvironment, configuration)
        {
            _logger = logger;
        }

        [Route("[controller]/{url}")]
        public IActionResult Index(string url)
        {
            
            QuadroDAO quadroDAO = new QuadroDAO();
            var quadroAtual = quadroDAO.Get(url);

            ViewData["IdQuadroAtual"] = quadroAtual.Id;
            ViewData["TituloQuadro"] = quadroAtual.Nome;
            ViewData["IdAutorQuadro"] = quadroAtual.Usuario.Id;

            return View();
        }

  

       
      
    }
}
