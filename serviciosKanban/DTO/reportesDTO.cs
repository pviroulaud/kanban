using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serviciosKanban.DTO
{
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
