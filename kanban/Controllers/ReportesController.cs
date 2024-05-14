using System.Diagnostics;
using serviciosKanban.SRVC;
using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using serviciosKanban.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Kanban.Controllers
{

    public class ReportesController : Controller
    {
        private readonly IreportesSrvc _servicioReportes;
        private readonly ILogger<HomeController> _logger;
        private readonly Ijwt _JWT;
        private int idUsuarioOperacion;

        public ReportesController(ILogger<HomeController> logger,IreportesSrvc servicioReportes,Ijwt JWT)
        {
            _servicioReportes = servicioReportes;
            _logger = logger;
            _JWT = JWT;

            
        }
        
        [HttpGet("EstimacionSemanalIndex")]
        public IActionResult EstimacionSemanalIndex()
        {

            return View("EstimacionSemanal");
        }

        [HttpGet("EjecucionHoras")]
        public IActionResult EjecucionHoras()
        {

            return View("HorasEjecucion");
        }
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet("ActividadIncidencia")]
        [Authorize]
        public IActionResult ActividadIncidencia(int id)
        {

            var a = _servicioReportes.reporteActividadIncidencia(id);

            return Json(new { success = true, data = a });
            //return Accepted();
        }
        [HttpGet("ActividadTarea")]
        [Authorize]
        public IActionResult ActividadTarea(int id)
        {
            var a = _servicioReportes.reporteActividadTarea(id);

            return Json(new { success = true, data = a });
            //return Accepted();
        }

        [HttpGet("EstimacionSemanal")]
        [Authorize]
        public IActionResult EstimacionSemanal(filtroReporteSemanal filtro)
        {
            List<int> usuarios = new List<int>();
            List<string> sem = new List<string>();

            if (filtro.usuarioId != null)
            {
                foreach (var item in filtro.usuarioId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    usuarios.Add(Convert.ToInt32(item));
                }
            }
            
            //List<string> sem = new List<string>() { "2024-W19" };
            var a =_servicioReportes.planificacionSemana(filtro.semana, filtro.semanaHasta, usuarios);
             
            return Json(new { success = true, data = a });
            //return Accepted();
        }

        [HttpGet("EjecucionPorHora")]
        [Authorize]
        public IActionResult EjecucionPorHora(filtroReporte filtro)
        {
            List<int> usuarios = new List<int>();
           

            if (filtro.usuarioId!=null)
            {
                foreach (var item in filtro.usuarioId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    usuarios.Add(Convert.ToInt32(item));
                }
            }
            
            var a=_servicioReportes.horasPorDia(filtro.fechaDesde,filtro.fechaHasta,usuarios,filtro.verCerrados);
            return Json(new { success = true, data = a });
            //return Accepted();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}