using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using serviciosKanban.SRVC;
using serviciosKanban.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Kanban.Controllers
{
    
    public class TareasController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IincidenciaSrvc _servicioIncidencia;
        private readonly IproyectoSrvc _servicioProyecto;
        private readonly ItareaSrvc _servicioTareas;
        private readonly Ijwt _JWT;
        private int idUsuarioOperacion;

        public TareasController(ILogger<HomeController> logger,IincidenciaSrvc servicioIncidencia,IproyectoSrvc servicioProyecto,ItareaSrvc servicioTareas, Ijwt JWT)
        {
            _logger = logger;
            _servicioIncidencia=servicioIncidencia;
            _servicioProyecto=servicioProyecto;
            _servicioTareas=servicioTareas;
            _JWT = JWT;

            
        }

        [HttpPost]
        [Authorize]
        public IActionResult listar([FromForm]filtroBusquedaDTO filtro)
        {
            
            var a = _servicioTareas.listar(filtro);
            
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
            var a = _servicioIncidencia.obtener(id);
            return Json(new { success = true, data = a });
            //return Accepted();
        }

        [HttpPost]
        [Authorize]
        public IActionResult nuevo(nuevaIncidenciaDTO nuevaIncidencia)
        {
            idUsuarioOperacion = Convert.ToInt32(User.Claims.Where(t => t.Type == "Id").FirstOrDefault().Value);

            int id=_servicioIncidencia.nuevo(idUsuarioOperacion,nuevaIncidencia);        
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
        public IActionResult actualizar(incidenciaDTO incidencia)
        {
            idUsuarioOperacion = Convert.ToInt32(User.Claims.Where(t => t.Type == "Id").FirstOrDefault().Value);

            int id=_servicioIncidencia.actualizar(idUsuarioOperacion,incidencia);        
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

            bool eliminado =_servicioIncidencia.eliminar(idUsuarioOperacion,id);        
            if (id>0)
            {
                return Json(new{success =true, data = new {id=id}});
            }
            else
            {
                return Json(new{success =false, data = new {id=0}});
            }
        }


        [HttpGet]
        [Authorize]
        public IActionResult listarTipoIncidencia()
        {
            var a = _servicioIncidencia.listarTipos();
            return Json(new
            {
                success=true,
                data=a
            });
            //return Accepted();
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult listarEstados()
        {
            var a = _servicioIncidencia.listarEstados();
            return Json(new
            {
                success=true,
                data=a
            });
            //return Accepted();
        }

        [HttpGet]
        [Authorize]
        public IActionResult listarProyectos()
        {
            var a = _servicioProyecto.listar();
            return Json(new
            {
                success=true,
                data=a
            });
        }


        [HttpGet]
        [Authorize]
        public IActionResult listarTipoTarea()
        {
            var a = _servicioTareas.listarTipos();
            return Json(new
            {
                success = true,
                data = a
            });
            //return Accepted();
        }
        [HttpGet]
        [Authorize]
        public IActionResult obtenerTarea(int id)
        {
            var a = _servicioTareas.obtener(id);
            return Json(new { success = true, data = a });
            //return Accepted();
        }

        [HttpPost]
        [Authorize]
        public IActionResult nuevaTarea(nuevaTareaDTO nuevaTarea)
        {
            idUsuarioOperacion = Convert.ToInt32(User.Claims.Where(t => t.Type == "Id").FirstOrDefault().Value);

            int id = _servicioTareas.nuevo(idUsuarioOperacion, nuevaTarea);
            if (id > 0)
            {
                return Json(new { success = true, data = new { id = id } });
            }
            else
            {
                return Json(new { success = false, data = new { id = 0 } });
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult actualizarTarea(tareaDTO tarea)
        {
            idUsuarioOperacion = Convert.ToInt32(User.Claims.Where(t => t.Type == "Id").FirstOrDefault().Value);

            int id = _servicioTareas.actualizar(idUsuarioOperacion, tarea);
            if (id > 0)
            {
                return Json(new { success = true, data = new { id = id } });
            }
            else
            {
                return Json(new { success = false, data = new { id = 0 } });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult registrarTiempo(registroTiempoDTO tiempo)
        {
            idUsuarioOperacion = Convert.ToInt32(User.Claims.Where(t => t.Type == "Id").FirstOrDefault().Value);

            int id = _servicioTareas.registrarTiempo(idUsuarioOperacion, tiempo);
            if (id > 0)
            {
                return Json(new { success = true, data = new { id = id } });
            }
            else
            {
                return Json(new { success = false, data = new { id = 0 } });
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}