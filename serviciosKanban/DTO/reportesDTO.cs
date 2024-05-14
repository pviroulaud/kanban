using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serviciosKanban.DTO
{
    public class reporteActividadDTO
    {
        public int id { get; set; }
        public int incidenciaId { get; set; }
        public string nombreIncidencia { get; set; }        
        public int usuarioId { get; set; }
        public string nombreUsuario { get; set; }
        public int tareaId { get; set; }
        public string nombreTarea { get; set; }
        public int estadoTareaId { get; set; }
        public string nombreEstadoTarea { get; set; }
        public decimal? ejecucion { get; set; }
        public string? descripcion { get; set; }
        public DateTime fechaEjecucion { get; set; }
        public DateTime fechaRegistro { get; set; }

    }
    public class reporteDiarioDTO
    {
        public DateTime fecha { get; set; }

        public decimal? totalEjecucion { get; set; }
        public decimal? totalBloqueo { get; set; }
        public List<tareaCompletaDTO> tareas { get; set; } = new List<tareaCompletaDTO>();
    }
    public class reportePeriodoDTO
    {
        public DateTime fechaDesde { get; set; }
        public DateTime fechaHasta { get; set; }
        public decimal? totalEstimacion { get; set; }
        public decimal? totalEjecucion { get; set; }
        public decimal? totalBloqueo { get; set; }
        public List<reporteDiarioDTO> detalleDiario { get; set; } = new List<reporteDiarioDTO>();
    }

    public class reporteSemanaDTO
    {
        public string  semanaEjecucionPlanificada { get; set; }
        public DateTime fechaDesde { get; set; }
        public DateTime fechaHasta { get; set; }
        public decimal? estimacionTotal { get; set; }
        public List<detalleReporteSemanaDTO> detalle { get; set; } = new List<detalleReporteSemanaDTO>();
    }
    public class detalleReporteSemanaDTO
    {
        public int incidenciaId { get; set; }
        public string nombreIncidencia { get; set; }
        public int tareaId { get; set; }
        public string nombreTarea { get; set; }
        public int? usuarioResponsableId { get; set; }
        public string nombreUsuarioResponsable { get; set; }
        public decimal? estimacion { get; set; }
    }
}
