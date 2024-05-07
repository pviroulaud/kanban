using System.Diagnostics;
using serviciosKanban.SRVC;
using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using serviciosKanban.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Kanban.Controllers
{
    
    public class ProyectosController : Controller
    {
        private readonly IproyectoSrvc _servicioProyecto;
        private readonly ILogger<HomeController> _logger;
        private readonly Ijwt _JWT;
        private int idUsuarioOperacion;

        public ProyectosController(ILogger<HomeController> logger,IproyectoSrvc servicioProyecto,Ijwt JWT)
        {
            _servicioProyecto=servicioProyecto;
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
            var a = _servicioProyecto.listar();
            var ret= Json(new
            {
                //draw = draw,
                recordsFiltered = a.Count,
                recordsTotal = a.Count,
                data = a
            });
            return ret;
        }

        [HttpGet]
        [Authorize]
        public IActionResult obtener(int id)
        {
            var a = _servicioProyecto.obtener(id);
            return Json(new { success = true, data = a });
        }

        [HttpPost]
        [Authorize]
        public IActionResult nuevo(nuevoProyectoDTO nuevoProyecto)
        {
            idUsuarioOperacion = Convert.ToInt32(User.Claims.Where(t => t.Type == "Id").FirstOrDefault().Value);

            int id=_servicioProyecto.nuevo(idUsuarioOperacion,nuevoProyecto);        
            if (id>0)
            {
                return Json(new{success =true, data = new {id=id}});
                //return Created("nuevo",id);
            }
            else
            {
                return Json(new{success =false, data = new {id=0}});
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult actualizar(proyectoDTO proyecto)
        {
            idUsuarioOperacion = Convert.ToInt32(User.Claims.Where(t => t.Type == "Id").FirstOrDefault().Value);

            int id=_servicioProyecto.actualizar(idUsuarioOperacion,proyecto);        
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

            bool eliminado =_servicioProyecto.eliminar(idUsuarioOperacion,id);        
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