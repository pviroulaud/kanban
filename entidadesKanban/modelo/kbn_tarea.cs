using System;
using System.Collections.Generic;

namespace entidadesKanban.modelo
{
    public partial class kbn_tarea
    {
        public int id { get; set; }
        public string nombre { get; set; } = null!;
        public int tipoTareaId { get; set; }
        public int estadoId { get; set; }
        public int usuarioCreadorId { get; set; }
        public int? usuarioResponsableId { get; set; }
        public int incidenciaId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public decimal? estimacion { get; set; }
        public decimal? ejecucion { get; set; }
        public string? semanaDeEjecucionPlanificada { get; set; }
        public string? semanaDeEjecucionReal { get; set; }
        public string? descripcion { get; set; }

        public virtual kbn_estado estado { get; set; } = null!;
        public virtual kbn_incidencia incidencia { get; set; } = null!;
        public virtual kbn_tipoTarea tipoTarea { get; set; } = null!;
        public virtual kbn_usuario usuarioCreador { get; set; } = null!;
        public virtual kbn_usuario? usuarioResponsable { get; set; }
    }
}
