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
}
