using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class AdminController : BaseController
    {
        public ILogger<AuthController> _logger { get; set; }
        public AdminController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ILogger<AuthController> logger) : base(webHostEnvironment, configuration)
        {
            this._logger = logger;
        }

     

    }
}
