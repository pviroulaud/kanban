using System.Diagnostics;
using serviciosKanban.SRVC;
using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using serviciosKanban.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Kanban.Controllers
{

    public class UsuariosController : Controller
    {
        private readonly IusuarioSrvc _servicioUsuario;
        private readonly ILogger<HomeController> _logger;
        private readonly Ijwt _JWT;
        private int idUsuarioOperacion;

        public UsuariosController(ILogger<HomeController> logger,IusuarioSrvc servicioUsuario,Ijwt JWT)
        {
            _servicioUsuario=servicioUsuario;
            _logger = logger;
            _JWT = JWT;

            
        }

        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult listar()
        {
            var a = _servicioUsuario.listar();
            return Json(new
            {
                //draw = draw,
                recordsFiltered = a.Count,
                recordsTotal = a.Count,
                data = a
            });
            //return Accepted();
        }

        [HttpGet]
        [Authorize]
        public IActionResult obtener(int id)
        {
            var a = _servicioUsuario.obtener(id);
            return Json(new { success = true, data = a });
            //return Accepted();
        }

        [HttpPost]
        [Authorize]
        public IActionResult nuevo(nuevoUsuarioDTO nuevoUsuario)
        {
            idUsuarioOperacion = Convert.ToInt32(User.Claims.Where(t => t.Type == "Id").FirstOrDefault().Value);

            int id=_servicioUsuario.nuevo(idUsuarioOperacion,nuevoUsuario);        
            if (id>0)
            {
                return Json(new{success =true, data = new {id=id}});
            }
            else
            {
                return Json(new{success =false, data = new {id=0}});
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult actualizar(usuarioDTO usuario)
        {
            idUsuarioOperacion = Convert.ToInt32(User.Claims.Where(t => t.Type == "Id").FirstOrDefault().Value);

            int id=_servicioUsuario.actualizar(idUsuarioOperacion,usuario);        
            if (id>0)
            {
                return Json(new{success =true, data = new {id=id}});
            }
            else
            {
                return Json(new{success =false, data = new {id=0}});
            }
        }
        [HttpDelete]
        [Authorize]
        public IActionResult eliminar(int id)
        {
            idUsuarioOperacion = Convert.ToInt32(User.Claims.Where(t => t.Type == "Id").FirstOrDefault().Value);

            bool eliminado =_servicioUsuario.eliminar(idUsuarioOperacion,id);        
            if (id>0)
            {
                return Json(new{success =true, data = new {id=id}});
            }
            else
            {
                return Json(new{success =false, data = new {id=0}});
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}