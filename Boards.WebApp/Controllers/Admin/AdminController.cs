using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace Boards.WebApp.Controllers
{

    public class AdminController : BaseController
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ILogger<AdminController> logger) : base(webHostEnvironment, configuration)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AdminAuthorization]
        public IActionResult Dashboard()
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            LeadDAO leadDAO = new LeadDAO();
            QuadroDAO quadroDAO = new QuadroDAO();
            CartaoDAO cartaoDAO = new CartaoDAO();

            ViewData["QtdUsuarios"] = usuarioDAO.Get().Count();
            ViewData["QtdLeads"] = leadDAO.Get().Count();
            ViewData["QtdQuadros"] = quadroDAO.Get().Count();
            ViewData["QtdCartoes"] = cartaoDAO.Get().Count();



            return View();
        }

        [AdminAuthorization]
        public IActionResult Configuracao()
        {
            return View();
        }

        [AdminAuthorization]
        public IActionResult Usuarios()
        {
            return View();
        }

        [AdminAuthorization]
        public IActionResult ExportarLeads()
        {
            return View();
        }

        [AdminAuthorization]
        public IActionResult ImportarClientesVIP()
        {
            return View();
        }

        [HttpPost]
        [AdminAuthorization]
        public IActionResult ImportarClientesVIP(IFormFile formFile)
        {
            int qtdImportados = 0;
            int qtdAtualizados = 0;
            int qtdErros = 0;
            foreach (var item in Helper.ReadAsList(formFile).Skip(1))
            {
                try
                {
                    var email = item.Split(';')[0];
                    var nome = item.Split(';')[1];
                    var tags = item.Split(';')[2];

                    UsuarioDAO usuarioDAO = new UsuarioDAO();
                    var novoUsuarioVIP = new Usuario();

                    if (usuarioDAO.Get().FirstOrDefault(x => x.Email.ToLower() == email.ToLower()) != null)
                        novoUsuarioVIP = usuarioDAO.Get(email.ToLower());

                    novoUsuarioVIP.Email = email;
                    novoUsuarioVIP.Nome = nome;
                    novoUsuarioVIP.IsVIP = true;
                    novoUsuarioVIP.Tags = tags;

                    if (usuarioDAO.Get(email.ToLower()) != null)
                    {
                        usuarioDAO.Update(novoUsuarioVIP);
                        qtdImportados++;
                    }
                    else
                    { 
                        usuarioDAO.Add(novoUsuarioVIP);
                        qtdAtualizados++;
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"{ex.Message} | Erro durante a tentativa de importar a linha {item}");

                    qtdErros++;
                    ViewData["TipoResultado"] = "error";
                    ViewData["Resultado"] = $"Erro durante a exportação";
                }
            }
            if (qtdErros == 0)
            {
                ViewData["TipoResultado"] = "success";
            }
            else if (qtdErros != 0 && qtdImportados > 0)
            {
                ViewData["TipoResultado"] = "warning";
            }
            else
            {
                ViewData["TipoResultado"] = "error";
            }

            ViewData["Resultado"] = $"Foram importados {qtdImportados} usuários com sucesso, {qtdAtualizados} atualizados e {qtdErros} aconteceram.";

            return View();
        }

        [Authorization]
        public IActionResult Logout()
        {
            AuthHandler authHandler = new AuthHandler(HttpContext);
            var adminLogado = GetAdminLogado();
            authHandler.Logout();
            _logger.LogInformation($"O administrador {adminLogado.Email} fez logoff com sucesso.");

            return RedirectToAction("Index", "Admin");
        }



    }

    public static class Helper
    {
        public static List<string> ReadAsList(this IFormFile file)
        {
            var result = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.Add(reader.ReadLine());
            }
            return result;
        }
    }
}
