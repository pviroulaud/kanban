using System.Diagnostics;
using serviciosKanban.SRVC;
using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using serviciosKanban.DTO;

namespace Kanban.Controllers
{

    public class LoginController : Controller
    {
        private readonly IusuarioSrvc _servicioUsuario;
        private readonly ILogger<HomeController> _logger;

        public LoginController(ILogger<HomeController> logger,IusuarioSrvc servicioUsuario)
        {
            _servicioUsuario=servicioUsuario;
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult auth(loginDTO credenciales)
        {
            respuestaLoginDTO cred= _servicioUsuario.autenticar(credenciales);        
            if (cred.autorizado)
            {
                return Json(new{success =true, data = cred});
            }
            else{
                return Json(new{success =false});
            }
            
            
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}