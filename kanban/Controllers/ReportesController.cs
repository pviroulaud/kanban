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
        [HttpGet("EjecucionHoras")]
        public IActionResult EjecucionHoras()
        {

            return View("HorasEjecucion");
        }
        public IActionResult Index()
        {
            
            return View();
        }



        [HttpGet("EjecucionPorHora")]
        [Authorize]
        public IActionResult EjecucionPorHora(filtroReporte filtro)
        {
            List<int> usuarios = new List<int>();
            var a=_servicioReportes.horasPorDia(filtro.fechaDesde,filtro.fechaHasta,filtro.usuarioId,filtro.verCerrados);
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