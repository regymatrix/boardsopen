using Boards.DTO;
using Boards.WebApp.Authentication;
using Boards.WebApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boards.WebApp.Controllers.Base
{
    public class BaseController : Controller
    {
        public IWebHostEnvironment WebHostEnvironment { get; set; }
        public IConfiguration Configuration { get; set; }

        public Administrador AdministradorLogado { get; set; }
        public Usuario UsuarioLogado { get; set; }
        public bool OnlyAuthorized { get; set; }
        public BaseController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.WebHostEnvironment = webHostEnvironment;
            this.OnlyAuthorized = false;


           
        }
        public Administrador GetAdminLogado()
        {
            AuthHandler authHandler = new AuthHandler(HttpContext);
            AdministradorLogado = authHandler.GetAdministradorLogado();
            return AdministradorLogado;
        }

        public Usuario GetUsuarioLogado() {
            AuthHandler authHandler = new AuthHandler(HttpContext);
            UsuarioLogado = authHandler.GetUsuarioLogado();
            return UsuarioLogado;
        }

        public BaseController(IWebHostEnvironment webHostEnvironment, bool onlyAuthorized)
        {
            this.WebHostEnvironment = webHostEnvironment;
            this.OnlyAuthorized = onlyAuthorized;
        }



        public new IActionResult View()
        {
            return GetDefaultView();
        }


        private IActionResult GetDefaultView()
        {
            var authHandler = new AuthHandler(HttpContext);
            var defaultViewModel = new DefaultViewModel()
            {
                IsLogado = authHandler.IsUsuarioLogado(),
                UsuarioLogado = authHandler.GetUsuarioLogado(),
                IsProduction = WebHostEnvironment.EnvironmentName == "Production",
                Versao = Configuration["Version"]
            };

            return base.View(defaultViewModel);
        }
    }
}
