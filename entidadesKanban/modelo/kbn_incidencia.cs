using System;
using System.Collections.Generic;

namespace entidadesKanban.modelo
{
    public partial class kbn_incidencia
    {
        public kbn_incidencia()
        {
            kbn_tarea = new HashSet<kbn_tarea>();
        }

        public int id { get; set; }
        public string nombre { get; set; } = null!;
        public int tipoIncidenciaId { get; set; }
        public int estadoId { get; set; }
        public int usuarioCreadorId { get; set; }
        public int usuarioResponsableId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int proyectoId { get; set; }
        public string? descripcion { get; set; }

        public virtual kbn_estado estado { get; set; } = null!;
        public virtual kbn_proyecto proyecto { get; set; } = null!;
        public virtual kbn_tipoIncidencia tipoIncidencia { get; set; } = null!;
        public virtual kbn_usuario usuarioCreador { get; set; } = null!;
        public virtual kbn_usuario usuarioResponsable { get; set; } = null!;
        public virtual ICollection<kbn_tarea> kbn_tarea { get; set; }
    }
}
